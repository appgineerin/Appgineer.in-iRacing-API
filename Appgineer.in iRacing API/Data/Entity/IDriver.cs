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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AiRAPI.Data.Entity
{
    public interface IDriver : IEntity, IEquatable<IDriver>
    {
        string Initials { get; }
        string ShortName { get; }
        string LastName { get; }
        string ThreeLetterCode { get; }
        bool IsDriving { get; }
        IClub Club { get; }
        ILicense License { get; }
        string Division { get; }
        BitmapSource Helmet { get; }
        int HelmetDesignNumber { get; }
        Color HelmetColor1 { get; }
        Color HelmetColor2 { get; }
        Color HelmetColor3 { get; }
    }
}
