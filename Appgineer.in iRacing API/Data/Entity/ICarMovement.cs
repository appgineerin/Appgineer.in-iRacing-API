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
using AiRAPI.Data.Enums;

namespace AiRAPI.Data.Entity
{
    public interface ICarMovement : INotifyPropertyChanged
    {
        float Speed { get; }
        bool IsAccelerating { get; }
        bool IsBraking { get; }
        double TrackPct { get; }
        int Gear { get; }
        int Rpm { get; }
        TrackLocation TrackLocation { get; }
        bool IsInPits { get; }
        bool IsOnJokerLap { get; }
        bool IsOnJokerSection { get; }
    }
}
