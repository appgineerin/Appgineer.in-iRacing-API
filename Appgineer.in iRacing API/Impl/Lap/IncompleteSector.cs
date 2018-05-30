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

namespace AiRAPI.Impl.Lap
{
    internal sealed class IncompleteSector : Sector
    {
        public IncompleteSector()
        {
            IsUnknownSectorTime = true;
        }

        public override bool IsCompleted => false;

        private double _beginTime;
        public double BeginTime
        {
            get => _beginTime;
            internal set
            {
                SetProperty(ref _beginTime, value);
                IsUnknownSectorTime = BeginTime > 0;
            }
        }

        public override float Time
        {
            get
            {
                if (IsUnknownSectorTime || BeginTime <= 0)
                    return -1;

                return (float)(ApiProvider.Sim.Telemetry.SessionTime - BeginTime);
            }
            protected set => throw new InvalidOperationException();
        }
    }
}
