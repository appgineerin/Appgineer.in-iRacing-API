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

using System.Collections.ObjectModel;
using System.ComponentModel;
using AiRAPI.Data.Results;

namespace AiRAPI.Data.Lap
{
    public interface ILap : INotifyPropertyChanged
    {
        IEntitySessionResult Result { get; }
        int Number { get; }
        int Position { get; }
        int ClassPosition { get; }
        float Time { get; }
        float Gap { get; }
        int GapLaps { get; }
        int ReplayPosition { get; }
        bool WasOnPitRoad { get; set; }
        bool WasUnderCaution { get; set; }
        bool IsJokerLap { get; }
        ReadOnlyObservableCollection<ISector> Sectors { get; }
    }
}
