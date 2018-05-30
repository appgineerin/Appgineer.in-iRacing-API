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

using AiRAPI.Data.Lap;

namespace AiRAPI.Data.Entity
{
    public class EntityComparison
    {
        public ILap Lap1 { get; set; }
        public ILap Lap2 { get; set; }

        public float? LapDelta
        {
            get
            {
                if (Lap1?.Time > 0 && Lap2?.Time > 0)
                    return Lap1.Time - Lap2.Time;
                return null;
            }
        }

        public float? LapDeltaReverse
        {
            get
            {
                if (Lap1?.Time > 0 && Lap2?.Time > 0)
                    return Lap2.Time - Lap1.Time;
                return null;
            }
        }

        public IEntity Entity1 { get; set; }
        public IEntity Entity2 { get; set; }
    }
}
