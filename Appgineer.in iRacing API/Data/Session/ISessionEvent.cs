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
using System.ComponentModel;
using AiRAPI.Data.Camera;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;

namespace AiRAPI.Data.Session
{
    public interface ISessionEvent : INotifyPropertyChanged, IComparable<ISessionEvent>
    {
        DateTime Timestamp { get; }
        long ReplayPos { get; }
        string Description { get; }
        IEntity Entity { get; }
        ICameraGroup CameraGroup { get; set; }
        SessionType SessionType { get; }
        SessionEventType EventType { get; }
        int LapNumber { get; }
        int Rewind { get; }
    }
}
