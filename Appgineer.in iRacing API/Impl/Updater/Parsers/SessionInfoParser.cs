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
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Lap;
using AiRAPI.Data.Session;
using AiRAPI.Impl.Lap;
using AiRAPI.Impl.Results;
using AiRAPI.Impl.Session;
using AiRAPI.Impl.Utils;
using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Updater.Parsers
{
    internal sealed class SessionInfoParser : Parser
    {
        private static readonly Dictionary<string, SessionType> SessionTypeMap = new Dictionary<string, SessionType>()
        {
            {"Warmup", SessionType.Warmup },
            {"Offline Testing", SessionType.Practice},
            {"Practice", SessionType.Practice},
            {"Open Qualify", SessionType.Qualify},
            {"Lone Qualify", SessionType.Qualify},
            {"Race", SessionType.Race}
        };

        private static readonly Dictionary<string, SessionSubType> SessionSubTypeMap = new Dictionary<string, SessionSubType>()
            {
                {"", SessionSubType.Default },
                {"Heat", SessionSubType.Heat},
                {"Consolation", SessionSubType.Consolation},
                {"Feature", SessionSubType.Feature}
            };

        private readonly DataUpdater _updater;

        internal SessionInfoParser(DataUpdater updater)
        {
            _updater = updater;
        }
        
        internal override void Parse(YamlMappingNode root, Simulation sim)
        {
            var sessions = root.GetList("SessionInfo.Sessions");
            var currentSessionNumber = sim.Telemetry.SessionNum;

            Debug.WriteLine("> Start parsing sessions.");

            foreach (var session in sessions.Children.OfType<YamlMappingNode>())
            {
                Debug.WriteLine($"> Updating session {session.GetInt("SessionNum")}.");
                UpdateSession(root, session, sim, currentSessionNumber);
            }

            var results = sim.Session.SessionResults.FirstOrDefault(r => r.Id == currentSessionNumber) as SessionResult;
            TriggerFastestLap(sim, results);
            
            UpdatePositions(sim.Session as Session.Session, results);
        }

        private void UpdateSession(YamlMappingNode root, YamlMappingNode session, Simulation sim, int currentSessionNumber)
        {
            var number = session.GetInt("SessionNum");
            if (number >= int.MaxValue || number < 0)
                return;

            var sessionResult = sim.Session.SessionResults.FirstOrDefault(r => r.Id == number);
            if (sessionResult == null)
            {
                Debug.WriteLine($">  Session number {number} does not yet exist, CREATING.");
                CreateSession(root, session, sim, number, currentSessionNumber);
            }
            else
            {
                Debug.WriteLine($">  Session number {number} already exists.");
                if (!(sessionResult is SessionResult result))
                    return;

                // Always update all previous sessions in case they are Skipped!
                Debug.WriteLine($">  Updating session {number}.");
                UpdateSession(session, sim, result, currentSessionNumber);

                if (sessionResult.Id == currentSessionNumber)
                {
                    Debug.WriteLine($">   Updating entities for session {number}");
                    UpdateEntitites(root, session, sim, result);
                }
            }
        }

        private static void CreateSession(YamlMappingNode root, YamlMappingNode session, Simulation sim, int number, int currentSessionNumber)
        {
            Debug.WriteLine($">> CreateSession: {number}");

            var sessionResult = new SessionResult { Id = number };
            
            var laps = session.GetString("SessionLaps");
            sessionResult.LapsTotal = laps == "unlimited" ? int.MaxValue : int.Parse(laps);
            var time = session.GetString("SessionTime");
            sessionResult.SessionLength = time == "unlimited" ? double.MaxValue : float.Parse(time.Substring(0, time.Length - 4), CultureInfo.InvariantCulture);

            sessionResult.Type = SessionTypeMap[session.GetString("SessionType")];
            var subType = session.GetString("SessionSubType");
            sessionResult.SubType = subType != null ? SessionSubTypeMap[subType.Trim()] : SessionSubType.Default;

            sessionResult.Name = session.GetString("SessionName");
            if (string.IsNullOrWhiteSpace(sessionResult.Name)) sessionResult.Name = sessionResult.Type.ToString();

            sessionResult.FinishLine = sessionResult.Type == SessionType.Race ? sessionResult.LapsTotal : int.MaxValue;

            if (sessionResult.FinishLine <= 0)
                sessionResult.FinishLine = int.MaxValue;

            UpdateSession(session, sim, sessionResult, currentSessionNumber);
            UpdateEntitites(root, session, sim, sessionResult);
            
            lock (sim.SharedCollectionLock)
            {
                Debug.WriteLine($">>>> ADDING NEW SESSION: ID: {sessionResult.Id}, Name: {sessionResult.Name}");
                ((Session.Session)sim.Session).SessionResultsInt.Add(sessionResult);
            }
        }

        private static void UpdateSession(YamlMappingNode session, Simulation sim, SessionResult sessionResult, int currentSessionNumber)
        {
            Debug.WriteLine($">> UpdateSession: {sessionResult.Id}");

            if (sessionResult.Id != session.GetInt("SessionNum"))
                return;
            
            sessionResult.HasStarted = sessionResult.Id <= currentSessionNumber;
            sessionResult.HasFinished = sessionResult.Id < currentSessionNumber;
            sessionResult.IsCurrent = sessionResult.Id == currentSessionNumber;

            if (sessionResult.State != SessionState.Racing && sessionResult.Type == SessionType.Race)
            {
                sessionResult.Cautions = session.GetInt("ResultsNumCautionFlags");
                sessionResult.CautionLaps = session.GetInt("ResultsNumCautionLaps");
            }

            sessionResult.Skipped = session.GetBool("SessionSkipped");
            sessionResult.RunGroupsUsed = session.GetBool("SessionRunGroupsUsed");

            sessionResult.LeadChanges = session.GetInt("ResultsNumLeadChanges");
            sessionResult.LapsCompleted = session.GetInt("ResultsLapsComplete");
            sessionResult.LapsRemaining = sessionResult.LapsTotal - sessionResult.LapsCompleted;

            try
            {
                // Nick: this is suddenly a list of one item?
                //var fastestLap = session.GetMap("ResultsFastestLap");
                var fastestLap = session.GetList("ResultsFastestLap")?.FirstOrDefault() as YamlMappingNode;
                if (fastestLap?.Children?.Count > 0)
                {
                    var entity = sim.Session.Entities.FirstOrDefault(e => e.CarIdx == fastestLap.GetInt("CarIdx"));
                    if (entity != null)
                    {
                        sessionResult.FastestLapDriver = entity.CurrentDriver;
                        sessionResult.FastestLapNumber = fastestLap.GetInt("FastestLap");
                    }

                    var newFastestLapTime = fastestLap.GetFloat("FastestTime");
                    if (Math.Abs(sessionResult.FastestLapTime - newFastestLapTime) <= 10E-6)
                        return;

                    sessionResult.PrevFastestLapTime = sessionResult.FastestLapTime;
                    sessionResult.FastestLapTime = newFastestLapTime;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private static void UpdateEntitites(YamlMappingNode root, YamlMappingNode session, Simulation sim, SessionResult sessionResult)
        {
            Debug.WriteLine($">> UpdateEntities: {sessionResult.Id}");
            
            if (session.GetInt("SessionNum") != sessionResult.Id)
                return;

            // If not done yet, check if we have qualify positions for this session
            if (!sessionResult.HasQualifyPositions)
            {
                var qualifyPositions = session.GetList("QualifyPositions");
                if (qualifyPositions?.Children?.Count > 0)
                {
                    sessionResult.HasQualifyPositions = true;
                    foreach (var qp in qualifyPositions.Children.OfType<YamlMappingNode>())
                        UpdateQualifyPosition(qp, sim, sessionResult);
                }
            }

            var resultsInfo = session.GetList("ResultsPositions").Children.OfType<YamlMappingNode>().ToList();

            Debug.WriteLine($">>> Session {sessionResult.Id}: {resultsInfo.Count} results");
            
            // First build results from the SDK results (may exclude drivers without laptimes)
            foreach (var result in resultsInfo)
                UpdateEntity(result, sim, sessionResult);
            
            // Now potentially add missing results if appropriate
            AddMissingEntityResults(root, sessionResult, sim);
        }

        private static void AddMissingEntityResults(YamlMappingNode root, SessionResult sessionResult, Simulation sim)
        {
            // First force the QualifySessionParser to run
            if (sim.Session.GetQualification() == null)
            {
                var parser = new QualiSessionParser();
                parser.Parse(root, sim);
            }

            var entityResults = new List<EntitySessionResult>();

            var entities = sim.Session.Entities.ToDictionary(e => e.CarIdx);
            var existingResults = sessionResult.Results.ToDictionary(r => r.Entity.CarIdx);

            if (sessionResult.HasQualifyPositions)
            {
                // If there are qualify positions, use them to build the grid
                foreach (var qp in sessionResult.QualifyPositions)
                {
                    if (!entities.ContainsKey(qp.CarIdx))
                        continue;

                    if (!(existingResults.TryGet(qp.CarIdx) is EntitySessionResult result))
                    {
                        result = new EntitySessionResult(sessionResult, entities[qp.CarIdx])
                        {
                            Position = qp.Position,
                            ClassPosition = qp.ClassPosition
                        };
                        entityResults.Add(result);
                    }
                    result.StartPosition = qp.Position;
                    result.ClassStartPosition = qp.ClassPosition;
                }
            }
            else
            {
                // No qualify positions means:
                // (1) P or Q session
                // (2) Non-heat race, or feature heat
                if (sessionResult.Type == SessionType.Race)
                {
                    if (sim.Session.Info.IsHeatRacing)
                    {
                        // This is a heat that has no grid yet, do NOT add any results
                    }
                    else
                    {
                        // This is a normal race, get the qual results
                        var qualResults = sim.Session.GetQualification();
                        if (qualResults?.Results?.Count > 0)
                        {
                            foreach (var qr in qualResults.Results)
                            {
                                if (!(existingResults.TryGet(qr.Entity.CarIdx) is EntitySessionResult result))
                                {
                                    result = new EntitySessionResult(sessionResult, qr.Entity)
                                    {
                                        Position = qr.Position,
                                        ClassPosition = qr.ClassPosition
                                    };
                                    entityResults.Add(result);
                                }
                                result.StartPosition = qr.Position;
                                result.ClassStartPosition = qr.ClassPosition;
                            }
                        }
                    }
                }
                else
                {
                    // Not a race yet, just add everyone
                    if (!(sessionResult.Type == SessionType.Race && sim.Session.Info.IsHeatRacing))
                    {
                        foreach (var entity in entities.Values)
                        {
                            if (sessionResult.Results.FirstOrDefault(r => r.Entity.CarIdx == entity.CarIdx) is EntitySessionResult)
                                continue;

                            var result = new EntitySessionResult(sessionResult, entity)
                            {
                                Position = int.MaxValue,
                                ClassPosition = int.MaxValue
                            };
                            entityResults.Add(result);
                        }
                    }
                }
            }

            if (entityResults.Count > 0)
            {
                lock (sim.SharedCollectionLock)
                {
                    sessionResult.ResultsInt.AddRange(entityResults);
                }
            }
        }

        private static void UpdateQualifyPosition(YamlMappingNode qp, ISimulation sim, SessionResult session)
        {
            var carIdx = qp.GetInt("CarIdx");
            if (carIdx >= int.MaxValue || carIdx < 0)
                return;
            
            var position = new SessionQualifyPosition
            {
                CarIdx = (byte)carIdx,
                Position = qp.GetInt("Position") + 1,
                ClassPosition = qp.GetInt("ClassPosition") + 1,
                FastestLap = qp.GetInt("FastestLap"),
                FastestTime = qp.GetFloat("FastestTime")
            };
            
            lock (sim.SharedCollectionLock)
            {
                session.QualifyPositionsInt.Add(position);
            }
        }

        private static void UpdateEntity(YamlMappingNode resultsInfo, Simulation sim, SessionResult session)
        {
            var carIdx = resultsInfo.GetInt("CarIdx");
            if (carIdx >= int.MaxValue || carIdx < 0)
                return;

            var entity = sim.Session.Entities.FirstOrDefault(e => e.CarIdx == carIdx);
            if (entity == null)
                return;

            if (!(session.ResultsInt.FirstOrDefault(r => r.Entity.CarIdx == carIdx) is EntitySessionResult result))
            {
                // First time update
                result = InitResult(resultsInfo, sim, session, entity, carIdx);
            }

            if (session.Type == SessionType.Race && result.StartPosition == 0 && result.ClassStartPosition == 0)
            {
                if (session.HasQualifyPositions)
                {
                    // Heat racing: heat session that has a separate QualifyPositions list
                    var qp = session.QualifyPositions.FirstOrDefault(q => q.CarIdx == result.Entity.CarIdx);
                    if (qp != null)
                    {
                        result.StartPosition = qp.Position;
                        result.ClassStartPosition = qp.ClassPosition;
                    }
                }
                else
                {
                    // Non-heat racing, or practice/qualy in heat racing (no separate QualifyPositions list)
                    var qualiResult = sim.Session.GetQualification()?
                        .Results
                        .FirstOrDefault(e => e.Entity.CarIdx == result.Entity.CarIdx);
                    if (qualiResult != null)
                    {
                        result.StartPosition = qualiResult.Position;
                        result.ClassStartPosition = qualiResult.ClassPosition;
                    }
                }
            }
            
            result.FastestLapTime = resultsInfo.GetFloat("FastestTime");
            result.LapsLed = resultsInfo.GetInt("LapsLed");
            result.ReasonOutString = resultsInfo.GetString("ReasonOutStr");
            if (session.Type == SessionType.Race)
                result.Gap = resultsInfo.GetFloat("Time");
            result.GapLaps = resultsInfo.GetInt("Lap");
            
            // Check for new lap
            var completed = resultsInfo.GetInt("LapsComplete");
            if (sim.Session.Current != null && session.Type == sim.Session.Current.Type
                && result.PreviousLap != null 
                && result.PreviousLap.Number == completed
                && !result.PreviousLap.HasExactLaptime)
            {
                // New lap, set time and add to list
                result.PreviousLap.SetExactLaptime(resultsInfo.GetFloat("LastTime"));
                result.LastLapTime = result.PreviousLap.Time;

                // Set positions etc to make sure we have the latest info from sdk
                result.PreviousLap.Number = completed;
                result.PreviousLap.Position = resultsInfo.GetInt("Position");
                result.PreviousLap.ClassPosition = resultsInfo.GetInt("ClassPosition") + 1;
                result.PreviousLap.Gap = resultsInfo.GetFloat("Time");
                result.PreviousLap.GapLaps = resultsInfo.GetInt("Lap");

                // Update fastest lap etc
                if (result.FastestLap == null || LapIsNewFastestLap(result.Laps, result.PreviousLap))
                    result.FastestLap = result.PreviousLap;

                if (result.Position < result.HighestPosition || result.HighestPosition == 0)
                    result.HighestPosition = result.Position;

                if (result.Position > result.LowestPosition || result.LowestPosition == 0)
                    result.LowestPosition = result.Position;

                if (result.ClassPosition < result.ClassHighestPosition || result.ClassHighestPosition == 0)
                    result.ClassHighestPosition = result.ClassPosition;

                if (result.ClassPosition > result.ClassLowestPosition || result.ClassLowestPosition == 0)
                    result.ClassLowestPosition = result.ClassPosition;

                if (session.Flags.CheckBits(SessionFlags.Caution, SessionFlags.CautionWaving) && result.Position == 1)
                    session.CautionLaps++;
                
                // ReSharper disable once PossibleNullReferenceException
                var classLeader = session.GetClassLeader(result.Entity.Car.Class.Name);
                if (classLeader != null && classLeader.Entity.CarIdx == result.Entity.CarIdx)
                    result.ClassLapsLed++;
                
                result.Laps.AddLap(result.PreviousLap);
            }

            if (sim.Session.Current != null && sim.Session.Current.State == SessionState.CoolDown)
            {
                result.CurrentLap.IsUnknownLaptime = true;
            }

            var pos = resultsInfo.GetInt("Position");
            result.Position = pos > 0 
                ? pos 
                : result.StartPosition > 0 
                    ? result.StartPosition 
                    : int.MaxValue;

            if (result.Position == 1)
                session.Leader = result.Entity;

            result.ClassPosition = resultsInfo.GetInt("ClassPosition") + 1;
            result.Incidents = resultsInfo.GetInt("Incidents");
            result.JokerLapsCompleted = resultsInfo.GetInt("JokerLapsComplete");
        }

        private static bool LapIsNewFastestLap(IEnumerable<ILap> laps, ILap lap)
        {
            return laps.All(l => !(l.Time <= lap.Time) || !(l.Time > 2F));
        }

        private static EntitySessionResult InitResult(YamlMappingNode resultsInfo, Simulation sim, SessionResult session,
            IEntity entity, int carIdx)
        {
            var result = new EntitySessionResult(session, entity)
            {
                FastestLapTime = resultsInfo.GetFloat("FastestTime"),
                LapsLed = resultsInfo.GetInt("LapsLed"),
                CurrentTrackPct = sim.Telemetry.CarIdxLap[carIdx] + sim.Telemetry.CarIdxLapDistPct[carIdx] - 1
            };
            
            var newLap = new CompletedLap(result)
            {
                Number = resultsInfo.GetInt("LapsComplete"),
                Position = resultsInfo.GetInt("Position"),
                ClassPosition = resultsInfo.GetInt("ClassPosition") + 1,
                Gap = resultsInfo.GetFloat("Time"),
                GapLaps = resultsInfo.GetInt("Lap")
            };
            newLap.SetExactLaptime(resultsInfo.GetFloat("LastTime"));

            result.PreviousLap = newLap;
            result.FastestLap = newLap;
            result.Laps.AddLap(newLap);

            // Current lap is empty here, set all properties
            result.CurrentLap.Number = newLap.Number + 1;
            result.CurrentLap.Position = newLap.Position;
            result.CurrentLap.ClassPosition = newLap.ClassPosition;
            result.CurrentLap.Gap = newLap.Gap;
            result.CurrentLap.GapLaps = newLap.GapLaps;

            if (session.Flags.CheckBits(SessionFlags.Caution, SessionFlags.CautionWaving))
                result.CurrentLap.WasUnderCaution = true;
            
            lock (sim.SharedCollectionLock)
            {
                session.ResultsInt.Add(result);
                ((EntityResults) result.Entity.Results).SessionResultsInt.Add(result);
            }

            return result;
        }

        private void TriggerFastestLap(Simulation sim, SessionResult session)
        {
            if (session == null || Math.Abs(session.FastestLapTime - session.PrevFastestLapTime) < 10E-6 || session.FastestLapTime <= 0)
                return;

            session.PrevFastestLapTime = session.FastestLapTime;

            var ev = new SessionEvent((long)(sim.Telemetry.SessionTime * 60 + _updater.TimeOffset),
                "New session fastest lap (" + session.FastestLapTime.ConvertToTimeString() + ")", session.FastestLapDriver,
                sim.CameraManager.CurrentGroup, session.Type, SessionEventType.FastLap, session.FastestLapNumber);

            if (sim.Session.SessionEvents.Contains(ev))
                return;
            
            lock (sim.SharedCollectionLock)
            {
                ((Session.Session)sim.Session).SessionEventsInt.Add(ev);
                sim.Triggers.Push(EventType.FastestLap);
            }
        }

        private static void UpdatePositions(ISession session, SessionResult results)
        {
            foreach (var entity in session.Entities)
            {
                if (!(results.Results.FirstOrDefault(e => e.Entity.CarIdx == entity.CarIdx) is EntitySessionResult result))
                    continue;
                
                if (result.Position == 1)
                    results.Leader = result.Entity;
            }
        }
    }
}
