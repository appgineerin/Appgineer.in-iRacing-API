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

using AiRAPI.Data.Enums;
using AiRAPI.Impl.Results;
using AiRAPI.Impl.Session;

namespace AiRAPI.Impl.Updater.Updater
{
    internal sealed class SessionFlagUpdater : UpdaterModule
    {
        internal SessionFlagUpdater(DataUpdater updater) : base(updater) { }

        internal void Update(SessionResult result, Simulation sim)
        {
            var prevFlags = result.Flags;
            result.Flags = sim.Telemetry.SessionFlags;
            if (result.Type == SessionType.Race
                && result.State == SessionState.Racing
                && result.LapsCompleted > 0
                && (result.LapsRemaining == 1 || result.SessionTimeRemaining <= 0))
            {
                result.Flags |= SessionFlags.White;
            }

            if (prevFlags == result.Flags)
                return;

            var ev = new SessionEvent((long)(sim.Telemetry.SessionTime * 60 + Updater.TimeOffset), result.Flags + " Flag", sim.CameraManager.FollowedEntity,
                sim.CameraManager.CurrentGroup, result.Type, SessionEventType.Flag, result.LapsCompleted);
            
            lock (sim.SharedCollectionLock)
            {
                ((Session.Session)sim.Session).SessionEventsInt.Add(ev);
            }

            if (result.Flags.CheckBits(SessionFlags.Caution, SessionFlags.CautionWaving))
            {
                foreach (var r in result.Results)
                {
                    if (r.CurrentLap != null) r.CurrentLap.WasUnderCaution = true;
                }
            }

            if (result.Flags.CheckBit(SessionFlags.OneLapToGreen) && !prevFlags.CheckBit(SessionFlags.OneLapToGreen))
            {
                sim.Triggers.Push(EventType.OneToGreen);
            }

            if (result.State == SessionState.Racing)
            {
                if (result.Flags.CheckBits(SessionFlags.Caution, SessionFlags.CautionWaving))
                    sim.Triggers.Push(EventType.Caution);
                else if (result.Flags.CheckBits(SessionFlags.Yellow, SessionFlags.YellowWaving))
                    sim.Triggers.Push(EventType.FlagYellow);
                else if (result.Flags.CheckBit(SessionFlags.Checkered))
                    sim.Triggers.Push(EventType.FlagCheckered);
                else if (result.Flags.CheckBit(SessionFlags.White))
                    sim.Triggers.Push(EventType.FlagWhite);
                else
                    sim.Triggers.Push(EventType.FlagGreen);
            }

            if (result.State == SessionState.Racing && !prevFlags.CheckBits(SessionFlags.Caution, SessionFlags.CautionWaving) 
                && result.Flags.CheckBits(SessionFlags.Caution, SessionFlags.CautionWaving))
                result.Cautions++;

            var startLightsChanged = false;
            if (result.Flags.CheckBit(SessionFlags.StartHidden) && !prevFlags.CheckBit(SessionFlags.StartHidden))
            {
                sim.Triggers.Push(EventType.LightsOff);
                startLightsChanged = true;
            }
            else if (result.Flags.CheckBit(SessionFlags.StartReady) && !prevFlags.CheckBit(SessionFlags.StartReady))
            {
                sim.Triggers.Push(EventType.LightsReady);
                startLightsChanged = true;
            }
            else if (result.Flags.CheckBit(SessionFlags.StartSet) && !prevFlags.CheckBit(SessionFlags.StartSet))
            {
                sim.Triggers.Push(EventType.LightsSet);
                startLightsChanged = true;
            }
            else if (result.Flags.CheckBit(SessionFlags.StartGo) && !prevFlags.CheckBit(SessionFlags.StartGo))
            {
                sim.Triggers.Push(EventType.LightsGo);
                startLightsChanged = true;
            }

            if (startLightsChanged)
            {
                var ev1 = new SessionEvent((long)(sim.Telemetry.SessionTime * 60 + Updater.TimeOffset), "Startlights changed to " + result.Flags,
                    sim.CameraManager.FollowedEntity, sim.CameraManager.CurrentGroup, result.Type, SessionEventType.StartLights, result.LapsCompleted);
                
                lock (sim.SharedCollectionLock)
                {
                    ((Session.Session)sim.Session).SessionEventsInt.Add(ev1);
                }
            }
        }
    }
}
