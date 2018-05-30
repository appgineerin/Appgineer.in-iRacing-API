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

namespace AiRAPI.Impl.Updater.Updater
{
    internal sealed class RadioUpdater : UpdaterModule
    {
        internal RadioUpdater(DataUpdater updater) : base(updater) { }

        internal void Update(Simulation sim)
        {
            var newRadioTransmitCarIdx = sim.Telemetry.RadioTransmitCarIdx;
            if (newRadioTransmitCarIdx == sim.CurrentRadioCarIdx)
                return;

            sim.CurrentRadioCarIdx = newRadioTransmitCarIdx;
            if (newRadioTransmitCarIdx == -1)
                sim.Triggers.Push(EventType.RadioOff);
            else
                sim.Triggers.Push(EventType.RadioOn);
        }
    }
}
