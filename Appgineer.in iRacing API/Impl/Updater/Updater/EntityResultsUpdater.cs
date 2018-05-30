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
using AiRAPI.Data.Enums;
using AiRAPI.Impl.Results;

namespace AiRAPI.Impl.Updater.Updater
{
    internal sealed class EntityResultsUpdater : UpdaterModule
    {
        internal EntityResultsUpdater(DataUpdater updater) : base(updater) { }

        internal void Update(SessionResult session, Simulation sim)
        {
            var leader = session.Leader;
            if (leader == null)
                return;

            foreach (var result in session.Results.OfType<EntitySessionResult>())
            {
                if (result.LivePosition > 1 && result.Entity.Car.Movement.Speed > 1)
                    result.LiveGap = result.Finished || session.Type != SessionType.Race ? result.Gap : (float)sim.TimeDelta.GetDelta(result.Entity.CarIdx, leader.CarIdx).TotalSeconds;
                else
                    result.LiveGap = result.Finished || session.Type != SessionType.Race ? result.Gap : 0;

                result.LiveGapLaps = result.Finished || session.Type != SessionType.Race ? result.GapLaps : (int)(leader.Results.CurrentResult.CurrentTrackPct - result.CurrentTrackPct);

                var classLeader = session.Results.OfType<EntitySessionResult>().FirstOrDefault(r => r.Entity.Car.Class.Name == result.Entity.Car.Class.Name && r.LiveClassPosition == 1);
                if (classLeader != null)
                {
                    result.ClassGap = result.Gap - classLeader.Gap;
                    result.LiveClassGap = result.Finished || session.Type != SessionType.Race ? result.ClassGap : (float)sim.TimeDelta.GetDelta(result.Entity.CarIdx, classLeader.Entity.CarIdx).TotalSeconds;
                    result.ClassGapLaps = result.GapLaps - classLeader.GapLaps;
                    result.LiveClassGapLaps = result.Finished || session.Type != SessionType.Race ? result.ClassGapLaps : (int)(classLeader.CurrentTrackPct - result.CurrentTrackPct);
                }
                else
                {
                    result.ClassGap = 0;
                    result.LiveClassGap = 0;
                    result.ClassGapLaps = 0;
                    result.LiveClassGapLaps = 0;
                }
            }

            var results = session.Results.OfType<EntitySessionResult>().OrderBy(r => r.LivePosition).ToList();
            foreach (var result in results)
            {
                var inFront = result.LivePosition > 1 ? session.Results.FirstOrDefault(r => r.LivePosition == result.LivePosition - 1) as EntitySessionResult : null;
                if (inFront != null)
                {
                    if (session.Type == SessionType.Race)
                    {
                        result.Interval = result.Gap - inFront.Gap;
                        result.LiveInterval = result.Finished ? result.Interval : (float)sim.TimeDelta.GetDelta(result.Entity.CarIdx, inFront.Entity.CarIdx).TotalSeconds;
                        result.IntervalLaps = result.GapLaps - inFront.GapLaps;
                        result.LiveIntervalLaps = result.Finished ? result.LiveIntervalLaps : (int)Math.Floor(inFront.CurrentTrackPct - result.CurrentTrackPct);

                        inFront.IntervalBehind = -result.Interval;
                        inFront.LiveIntervalBehind = -result.LiveInterval;
                        inFront.IntervalLapsBehind = -result.IntervalLaps;
                        inFront.LiveIntervalLapsBehind = -result.LiveIntervalLapsBehind;
                    }
                    else
                    {
                        result.Gap = result.FastestLapTime - (session.Results.FirstOrDefault(e => e.LivePosition == 1)?.FastestLapTime ?? 0);
                        result.Interval = result.LiveInterval = result.FastestLapTime - inFront.FastestLapTime;
                        result.IntervalLaps = 0;
                        result.LiveIntervalLaps = 0;

                        inFront.IntervalBehind = inFront.LiveIntervalBehind = -result.Interval;
                        inFront.IntervalLapsBehind = 0;
                        inFront.LiveIntervalLapsBehind = 0;
                    }
                }
                else
                {
                    // Leader
                    result.Interval = 0;
                    result.LiveInterval = 0;
                    result.IntervalLaps = 0;
                    result.LiveIntervalLaps = 0;
                }
                
                result.DidNotStart = session.Type == SessionType.Race && Math.Abs(result.LastLapTime) < 10E-6 && Math.Abs(result.FastestLapTime) < 10E-6
                    && result.CurrentTrackPct < 1 && leader.Results.CurrentResult.CurrentTrackPct > 1;

                if (session.Type != SessionType.Race || session.State == SessionState.ParadeLaps || session.State == SessionState.GetInCar)
                {
                    result.Out = false;
                }
                else
                {
                    var isOut = false;
                    isOut |= result.ReasonOutString != "Running";

                    var classLeader = session.GetClassLeader(result.Entity.Car.Class.Name);
                    if (classLeader != null)
                        isOut |= (result.CurrentTrackPct < classLeader.CurrentTrackPct * 0.75);

                    result.Out = isOut;
                }
            }

            var lastResult = results.LastOrDefault();
            if (lastResult != null)
            {
                lastResult.IntervalBehind = 0;
                lastResult.IntervalLapsBehind = 0;
                lastResult.LiveIntervalBehind = 0;
                lastResult.LiveIntervalLapsBehind = 0;
            }
        }
    }
}
