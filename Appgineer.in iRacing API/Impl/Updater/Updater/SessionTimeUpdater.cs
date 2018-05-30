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
using AiRAPI.Data.Enums;
using AiRAPI.Impl.Calculators;
using AiRAPI.Impl.Results;

namespace AiRAPI.Impl.Updater.Updater
{
    internal sealed class SessionTimeUpdater : UpdaterModule
    {
        internal SessionTimeUpdater(DataUpdater updater) : base(updater) { }

        internal void Update(SessionResult result, Simulation sim)
        {
            if (result.SessionStartTime < 0)
            {
                result.SessionStartTime = result.SessionTime;
                result.CurrentReplayPosition = sim.Telemetry.ReplayFrameNum;
            }

            if (result.SessionTime < 0.5)
                sim.TimeDelta = new TimeDelta(sim.Session.Track.Length, 300, 64);

            result.SessionTimeRemaining = sim.Telemetry.SessionTimeRemain;

            UpdateRemainingEstimations(result, sim);
        }

        private void UpdateRemainingEstimations(SessionResult result, Simulation sim)
        {
            // Default to non-estimated values
            result.IsFinalLap = false;
            //result.SessionLengthDecidedByLaps = false;
            result.EstimatedSessionLength = result.SessionLength;
            result.EstimatedSessionTimeRemaining = result.SessionTimeRemaining;
            result.EstimatedLapsTotal = result.LapsTotal;
            result.EstimatedLapsRemaining = result.LapsRemaining;

            if (result.Type != SessionType.Race)
                return;
            
            // Get overall average predicted pace
            var averageLaptime = result.GetAverageLaptimePrediction(sim);
            
            // Are laps dictated?
            var hasDictatedLaps = result.LapsTotal >= 1 && result.LapsTotal != int.MaxValue;
            if (hasDictatedLaps)
            {
                // Make sure session length is not "unlimited"
                var isTimeUnlimited = result.SessionLength > TimeSpan.FromHours(100).TotalSeconds;

                if (isTimeUnlimited)
                {
                    // "Unlimited" session time > 100 hours (official race probably?)
                    // Race is determined by laps,
                    // Estimate actual session length

                    result.SessionLengthDecidedByLaps = true;
                    result.EstimateSessionLength(averageLaptime);
                }
                else
                {
                    // Both laps and time dictated, determine which will be limiting

                    // Estimate laps from dictated session length
                    result.EstimateLapsTotal(averageLaptime);
                    
                    // Check if estimated laps > dictated laps
                    if (result.EstimatedLapsTotal > result.LapsTotal)
                    {
                        // Dictated total laps are limiting,
                        // Race decided by LAPS,
                        // Estimate session time
                        result.SessionLengthDecidedByLaps = true;
                        result.EstimateSessionLength(averageLaptime);

                        // Now that we know our estimated lap total will never be reached, 
                        // we should reset it to the prescribed value
                        result.EstimatedLapsTotal = result.LapsTotal;
                        result.EstimatedLapsRemaining = result.LapsRemaining;
                        
                    }
                    else
                    {
                        // Dictated session time is limiting,
                        // Race decided by TIME
                        // Estimate laps (already done)
                        result.SessionLengthDecidedByLaps = false;
                    }
                }
            }
            else
            {
                // No laps set, limited by time only

                // Estimate laps from session length and leader laptime
                result.SessionLengthDecidedByLaps = false;
                result.EstimateLapsTotal(averageLaptime);
                
            }

            // Determine if this is the final lap
            // Use the estimated laps, which are the actual dictated laps if limited by laps
            // Also use white flag or timeremaining = 0 as backup
            result.IsFinalLap = result.State == SessionState.Racing 
                                && (result.EstimatedLapsRemaining == 1
                                || result.Flags.CheckBit(SessionFlags.White)
                                || (result.LapsCompleted > 0 && result.SessionTimeRemaining <= 0.01));
        }
    }
}
