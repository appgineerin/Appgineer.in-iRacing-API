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
using AiRAPI.Data.Results;

namespace AiRAPI.Impl.Lap
{
    internal sealed class IncompleteLap : Lap
    {
        public override bool IsCompleted => false;

        internal bool IsUnknownLaptime { get; set; }

        private double _beginTime;
        public double BeginTime
        {
            get => _beginTime;
            internal set
            {
                if (SetProperty(ref _beginTime, value))
                {
                    IsUnknownLaptime = false;
                }
            }
        }

        public IncompleteSector CurrentSector { get; }

        public override float Time
        {
            get
            {
                if (IsUnknownLaptime)
                    return -1;

                return (float)(ApiProvider.Sim.Telemetry.SessionTime - BeginTime);
            }
            protected set => throw new InvalidOperationException();
        }

        internal IncompleteLap(IEntitySessionResult result) : base(result)
        {
            IsUnknownLaptime = true;
            CurrentSector = new IncompleteSector();
        }
    }
}
