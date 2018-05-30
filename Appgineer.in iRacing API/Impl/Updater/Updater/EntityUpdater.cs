// -----------------------------------------------------
//
// Distributed under GNU GPLv3.
//
// -----------------------------------------------------
//
// Copyright (c) 2018, appgineering.com
// All rights reserved.
// 
// This file is part of the Appgineer.in iRacing API.
//
// -----------------------------------------------------

using System;
using System.Linq;
using AiRAPI.Data;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Results;
using AiRAPI.Impl.Entity;
using AiRAPI.Impl.Event;
using AiRAPI.Impl.Lap;
using AiRAPI.Impl.Results;
using AiRAPI.Impl.Session;

namespace AiRAPI.Impl.Updater.Updater
{
    internal sealed class EntityUpdater : UpdaterModule
    {
        private const float SpeedCalcInterval = 0.3f;

        internal EntityUpdater(DataUpdater updater) : base(updater) { }

        internal void Update(SessionResult session, Simulation sim)
        {
            foreach (var result in session.Results.OfType<EntitySessionResult>())
            {
                if (result.Entity.Results.CurrentResult == null || result.Entity.Results.CurrentResult.Session.Id != session.Id)
                    ((EntityResults)result.Entity.Results).CurrentResult = result;

                var prevPos = result.PrevTrackPct;
                var curPos = sim.Telemetry.CarIdxLapDistPct[result.Entity.CarIdx];
                var now = Updater.CurrentTime - curPos / (1 + curPos - prevPos) * (Updater.CurrentTime - Updater.PrevTime);
                var movement = result.Entity.Car.Movement as CarMovement;
                
                var prevLap = result.PrevLapNumber;

                var curLap = sim.Telemetry.CarIdxLapCompleted[result.Entity.CarIdx];

                result.PrevLapNumber = curLap;
                result.PrevTrackPct = curPos;
                result.PrevTrackPctUpdate = Updater.CurrentTime;

                if (Updater.CurrentTime <= result.PrevTrackSpeedPctUpdate || Math.Abs(curPos - prevPos) < 10E-6)
                {
                    movement.Speed = 0;
                    movement.IsAccelerating = false;
                    movement.IsBraking = true;
                    goto CalcPitStop;
                }

                var diff = Updater.CurrentTime - result.PrevTrackSpeedPctUpdate;
                if (diff >= SpeedCalcInterval)
                {
                    CalcSpeed(movement, curPos, result.PrevTrackSpeedPct, diff, sim.Session.Track.Length);
                    result.PrevTrackSpeedPct = curPos;
                    result.PrevTrackSpeedPctUpdate = Updater.CurrentTime;
                }

                movement.Gear = sim.Telemetry.CarIdxGear[result.Entity.CarIdx];
                movement.Rpm = (int)sim.Telemetry.CarIdxRPM[result.Entity.CarIdx];

                if (!result.Finished && sim.Telemetry.CarIdxTrackSurface[result.Entity.CarIdx] != TrackLocation.NotInWorld)
                {
                    result.CurrentTrackPct = sim.Telemetry.CarIdxLap[result.Entity.CarIdx] + sim.Telemetry.CarIdxLapDistPct[result.Entity.CarIdx] - 1;
                    movement.TrackPct = (float)result.CurrentTrackPct % 1;

                    // Update current lap properties
                    UpdateCurrentLap(result, session);
                    UpdateSector(sim, session, result, curPos, prevPos);
                    
                    //if (curPos < 0.1 && prevPos > 0.9 && movement.Speed > 0)
                    if (curLap > prevLap)
                        OnFinishCrossing(sim.Telemetry, result, now, session, curLap + 1);
                }

                if (result.CurrentLap.Number + result.CurrentLap.GapLaps > session.FinishLine
                    && sim.Telemetry.CarIdxTrackSurface[result.Entity.CarIdx] != TrackLocation.NotInWorld && session.Type == SessionType.Race && !result.Finished)
                {
                    result.Finished = true;
                    result.CurrentTrackPct = Math.Floor(result.CurrentTrackPct) + 0.0064 - 0.0001 * result.Position;
                    ((CarMovement)result.Entity.Car.Movement).TrackPct = (float)result.CurrentTrackPct % 1;
                }

                UpdateTrackLocation(sim, result, sim.Telemetry.CarIdxTrackSurface[result.Entity.CarIdx]);

                CalcPitStop:
                var inPitStall = movement.TrackLocation == TrackLocation.InPitStall;
                movement.IsInPits = sim.Telemetry.CarIdxOnPitRoad[result.Entity.CarIdx] || inPitStall || movement.TrackLocation == TrackLocation.NotInWorld;
                if (movement.TrackLocation == TrackLocation.NotInWorld || session.Type != SessionType.Race || session.State != SessionState.Racing)
                    continue;

                if (movement.IsInPits && result.CurrentLap != null)
                    result.CurrentLap.WasOnPitRoad = true;

                result.Stint = (result.CurrentLap?.Number ?? 0) - (result.CurrentPitStop?.LapNumber ?? 0);
                if (!result.PitLaneEntryTime.HasValue)
                {
                    if (movement.IsInPits)
                    {
                        result.PitLaneEntryTime = now;

                        var stop = new PitStop { LapNumber = result.CurrentLap.Number };
                        result.PitStopsInt.Add(stop);
                        result.CurrentPitStop = stop;

                        var ev = new SessionEvent(result.CurrentLap.ReplayPosition, "Pit Stop on Lap " + result.CurrentLap.Number,
                            result.Entity, sim.CameraManager.CurrentGroup, session.Type, SessionEventType.Pit, result.CurrentLap.Number);
                        lock (sim.SharedCollectionLock)
                        {
                            ((Session.Session)sim.Session).SessionEventsInt.Add(ev);
                        }
                        sim.Triggers.Push(new TriggerInfo { CarIdx = result.Entity.CarIdx, Type = EventType.PitIn });
                    }
                }
                else
                {
                    var stop = result.CurrentPitStop as PitStop;
                    stop.PitLaneTime = now - result.PitLaneEntryTime.Value;
                    if (!result.PitStopStartTime.HasValue)
                    {
                        if (inPitStall && Math.Abs(movement.Speed) <= 0.1F)
                        {
                            result.PitStopStartTime = now;
                            stop.PitStopTime = 0;
                        }
                    }
                    else
                    {
                        stop.PitStopTime = now - result.PitStopStartTime.Value;
                        if (!inPitStall)
                        {
                            if (result.PitStopEndTime.HasValue && Math.Abs(result.PitStopEndTime.Value - now) < 10E-1)
                                continue;

                            if (!result.HasIncrementedCounter)
                            {
                                result.PitStopCount++;
                                result.HasIncrementedCounter = true;
                            }

                            stop.LapNumber = result.CurrentLap.Number;
                            result.PitStopStartTime = null;
                            result.PitStopEndTime = now;
                        }
                    }

                    if (!movement.IsInPits)
                    {
                        result.PitLaneExitTime = now;
                        result.HasIncrementedCounter = false;

                        stop.PitLaneTime = result.PitLaneExitTime.Value - result.PitLaneEntryTime.Value;
                        result.PitLaneEntryTime = null;

                        sim.Triggers.Push(new TriggerInfo { CarIdx = result.Entity.CarIdx, Type = EventType.PitOut });
                    }
                }
            }
        }

        private static void CalcSpeed(CarMovement movement, double curPos, double prevPos, double diff, float trackLength)
        {
            var prevSpeed = movement.Speed;
            var posDiff = curPos - prevPos;
            if (curPos < 0.1 && prevPos > 0.9)
                posDiff += 1;

            var speed = posDiff * trackLength / diff;
            if (speed > 111)
                return;

            movement.Speed = (float)speed;
            movement.IsAccelerating = speed >= prevSpeed && speed > 1;
            movement.IsBraking = speed < prevSpeed - 0.1;
        }

        private void OnFinishCrossing(ITelemetry telemetry, EntitySessionResult result, double now, SessionResult session, int newLapNumber)
        {
            // Complete last sector
            var sector = new CompletedSector(result.CurrentLap.CurrentSector, now);
            result.CurrentLap.SectorsInt.Add(sector);

            // Complete this lap and set it as previous lap
            result.PreviousLap = new CompletedLap(result.CurrentLap, now);
            result.LastLapTime = result.PreviousLap.Time;

            if (session.State != SessionState.ParadeLaps && !result.Finished)
            {
                // Start a new lap
                result.CurrentLap.BeginTime = now;
                result.CurrentLap.Number = newLapNumber;
                result.CurrentLap.ReplayPosition = (int) (telemetry.SessionTime * 60 + Updater.TimeOffset);
                result.CurrentLap.SectorsInt.Clear();

                result.CurrentLap.WasOnPitRoad = result.Entity.Car.Movement.IsInPits;

                // Set the caution state (can be FALSE if no caution during lap start)
                result.CurrentLap.WasUnderCaution = session.Flags.CheckBits(SessionFlags.Caution,
                    SessionFlags.CautionWaving);

                // Update current sector
                result.CurrentLap.CurrentSector.BeginTime = now;
                result.CurrentLap.CurrentSector.Index = 0;
                result.CurrentLap.CurrentSector.ReplayPosition = (int) (telemetry.SessionTime * 60 + Updater.TimeOffset);
            }
        }

        private void UpdateCurrentLap(EntitySessionResult result, SessionResult session)
        {
            result.CurrentLap.WasUnderCaution |= session.Flags.CheckBits(SessionFlags.Caution, SessionFlags.CautionWaving);
            result.CurrentLap.Position = result.LivePosition;
            result.CurrentLap.ClassPosition = result.LiveClassPosition;
            result.CurrentLap.Gap = session.Type == SessionType.Race ? result.LiveGap : (result.CurrentLap.Time - session.FastestLapTime);
            result.CurrentLap.GapLaps = session.Type == SessionType.Race ? result.LiveGapLaps : 0;
        }

        private void UpdateSector(ISimulation sim, SessionResult session, EntitySessionResult result, double curPos, double prevPos)
        {
            if (session.State != SessionState.Racing)
                return;

            var track = sim.Session.Track;
            if (track.SelectedSectors.Count <= 0)
                return;

            var i = 0;
            foreach (var location in track.SelectedSectors.OrderBy(s => s.Index).Select(s => s.Location))
            {
                if (curPos <= location || i <= result.CurrentLap.CurrentSector.Index)
                {
                    i++;
                    continue;
                }

                var now = Updater.CurrentTime - (curPos - location) * (curPos - prevPos);
                var sector = new CompletedSector(result.CurrentLap.CurrentSector, now);
                result.CurrentLap.SectorsInt.Add(sector);

                result.CurrentLap.CurrentSector.BeginTime = now;
                result.CurrentLap.CurrentSector.Index = i;
                result.CurrentLap.CurrentSector.ReplayPosition = (int) (sim.Telemetry.SessionTime * 60 + Updater.TimeOffset);

                break;
            }
        }

        private static void UpdateTrackLocation(Simulation sim, IEntitySessionResult result, TrackLocation newLocation)
        {
            if (newLocation == result.Entity.Car.Movement.TrackLocation)
                return;

            ((CarMovement)result.Entity.Car.Movement).TrackLocation = newLocation;
            if (newLocation == TrackLocation.OffTrack)
            {
                var ev = new SessionEvent(result.CurrentLap.ReplayPosition, "Off Track", result.Entity,
                    sim.CameraManager.CurrentGroup, result.Session.Type, SessionEventType.OffTrack, result.CurrentLap.Number);
                lock (sim.SharedCollectionLock)
                {
                    ((Session.Session)sim.Session).SessionEventsInt.Add(ev);
                }
                sim.Triggers.Push(new TriggerInfo { CarIdx = result.Entity.CarIdx, Type = EventType.OffTrack });
            }
            else if (newLocation == TrackLocation.NotInWorld)
            {
                sim.Triggers.Push(new TriggerInfo { CarIdx = result.Entity.CarIdx, Type = EventType.NotInWorld });
            }
        }
    }
}
