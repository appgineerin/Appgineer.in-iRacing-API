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

namespace AiRAPI.Data.Lap
{
    public interface ISector : INotifyPropertyChanged
    {
        int Index { get; }
        float Time { get; }
        int ReplayPosition { get; }
    }
}
