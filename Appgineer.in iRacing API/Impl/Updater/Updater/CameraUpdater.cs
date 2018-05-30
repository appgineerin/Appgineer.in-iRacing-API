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
using AiRAPI.Impl.Camera;

namespace AiRAPI.Impl.Updater.Updater
{
    internal sealed class CameraUpdater : UpdaterModule
    {
        internal CameraUpdater(DataUpdater updater) : base(updater) { }

        internal void Update(Simulation sim)
        {
            var cameraManager = (CameraManager)sim.CameraManager;
            cameraManager.CurrentGroupId = sim.Telemetry.CamGroupNumber;

            var isInReplay = sim.Telemetry.SessionTime - sim.Telemetry.ReplaySessionTime > 1.1;
            if (isInReplay == cameraManager.IsInReplay)
                return;

            cameraManager.IsInReplay = isInReplay;
            sim.Triggers.Push(isInReplay ? EventType.Replay : EventType.Live);
        }
    }
}
