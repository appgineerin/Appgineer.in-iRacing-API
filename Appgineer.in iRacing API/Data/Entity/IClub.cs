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
using System.Windows.Media.Imaging;

namespace AiRAPI.Data.Entity
{
    public interface IClub : INotifyPropertyChanged, IEquatable<IClub>
    {
        string Name { get; }
        BitmapSource LogoLarge { get; }
        BitmapSource LogoSmall { get; }

    }
}
