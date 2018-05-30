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
using AiRAPI.Impl.Utils;

namespace AiRAPI.Impl.Lap
{
    internal sealed class CompletedLap : Lap
    {
        public override bool IsCompleted => true;

        private float _time;
        public override float Time
        {
            get => _time;
            protected set => SetProperty(ref _time, value);
        }

        internal bool HasExactLaptime { get; private set; }

        internal CompletedLap(IEntitySessionResult result) : base(result)
        {
        }

        internal CompletedLap(IncompleteLap lap, double finishTime) : this(lap.Result)
        {
            Time = (float) (finishTime - lap.BeginTime);
            Number = lap.Number;
            Position = lap.Position;
            ClassPosition = lap.ClassPosition;
            Gap = lap.Gap;
            GapLaps = lap.GapLaps;
            ReplayPosition = lap.ReplayPosition;
            WasOnPitRoad = lap.WasOnPitRoad;
            WasUnderCaution = lap.WasUnderCaution;

            SectorsInt.AddRange(lap.SectorsInt);
        }

        internal void SetExactLaptime(float laptime)
        {
            if (HasExactLaptime)
                throw new InvalidOperationException("Exact laptime has already been set.");
            Time = laptime;
            HasExactLaptime = true;
        }

        public override string ToString()
        {
            return $"L{Number}, {Time.ConvertToTimeString()}, Entity {Result.Entity.Name}";
        }
    }
}
