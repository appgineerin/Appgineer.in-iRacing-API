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

using AiRAPI.Data.Entity;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Entity
{
    internal sealed class PitStop : BindableBase, IPitStop
    {
        private int _lapNumber;
        public int LapNumber
        {
            get => _lapNumber;
            internal set => SetProperty(ref _lapNumber, value);
        }

        private double _pitStopTime;
        public double PitStopTime
        {
            get => _pitStopTime;
            internal set => SetProperty(ref _pitStopTime, value);
        }

        private double _pitLaneTime;
        public double PitLaneTime
        {
            get => _pitLaneTime;
            internal set => SetProperty(ref _pitLaneTime, value);
        }
    }
}
