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
using AiRAPI.Impl.Results;
using AiRAPI.Impl.Session;

namespace AiRAPI.Impl.Updater.Updater
{
    internal sealed class SessionStateUpdater : UpdaterModule
    {
        internal SessionStateUpdater(DataUpdater updater) : base(updater) { }

        internal void Update(SessionResult result, Simulation sim)
        {
            var prevState = result.State;
            result.State = sim.Telemetry.SessionState;
            if (prevState == result.State)
                return;

            var ev = new SessionEvent((long)(sim.Telemetry.SessionTime * 60 + Updater.TimeOffset), "Session state changed to " + result.State.ToString(),
                sim.CameraManager.FollowedEntity, sim.CameraManager.CurrentGroup, result.Type, SessionEventType.State, result.LapsCompleted);
            
            lock (sim.SharedCollectionLock)
            {
                ((Session.Session)sim.Session).SessionEventsInt.Add(ev); 
            }

            if (result.Type == SessionType.Race && result.FinishLine == int.MaxValue && prevState == SessionState.Racing && result.State == SessionState.Checkered)
                result.FinishLine = (int)Math.Ceiling(result.Leader.Results.CurrentResult.CurrentTrackPct);

            if (result.State == SessionState.Racing && result.Flags.CheckBit(SessionFlags.Green))
                sim.Triggers.Push(EventType.FlagGreen);
            else if (result.State == SessionState.Checkered || result.State == SessionState.CoolDown)
                sim.Triggers.Push(EventType.FlagCheckered);
            else if (result.State == SessionState.GetInCar || result.State == SessionState.ParadeLaps || result.State == SessionState.Warmup)
                sim.Triggers.Push(EventType.FlagYellow);

            if (result.State == SessionState.Racing && (prevState == SessionState.ParadeLaps || prevState == SessionState.GetInCar))
                Updater.TimeOffset = sim.Telemetry.ReplayFrameNum - sim.Telemetry.SessionTime * 60;
        }
    }
}
