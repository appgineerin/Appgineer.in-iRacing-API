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

using System.ComponentModel;

namespace AiRAPI.Data.Entity
{
    public interface IPitStop : INotifyPropertyChanged
    {
        int LapNumber { get; }
        double PitStopTime { get; }
        double PitLaneTime { get; }
    }
}
