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
using System.Windows.Media;

namespace AiRAPI.Data.Entity
{
    public interface ILicense : INotifyPropertyChanged
    {
        float SafetyRating { get; }
        int IRating { get; }
        int Order { get; }
        string Display { get; }
        Color LicenseColor { get; }
        Color TextColor { get; }
    }
}
