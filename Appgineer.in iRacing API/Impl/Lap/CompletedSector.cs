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

namespace AiRAPI.Impl.Lap
{
    internal sealed class CompletedSector : Sector
    {
        public override bool IsCompleted => true;

        private float _time;
        public override float Time
        {
            get => _time;
            protected set => SetProperty(ref _time, value);
        }

        internal CompletedSector(IncompleteSector sector, double finishTime)
        {
            if (sector.BeginTime <= 0)
            {
                IsUnknownSectorTime = true;
                Time = -1;
            }
            else
            {
                Time = (float)(finishTime - sector.BeginTime);
            }
            Index = sector.Index;
            ReplayPosition = sector.ReplayPosition;
        }
    }
}
