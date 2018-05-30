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
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AiRAPI.Data.Entity
{
    public interface ICar : INotifyPropertyChanged, IEquatable<ICar>
    {
        string Number { get; }
        string Path { get; }
        string Name { get; }
        int NumberPadded { get; }
        int Id { get; }
        IClass Class { get; }
        ICarMovement Movement { get; }
        BitmapSource Image { get; }
        int DesignNumber { get; }
        Color Color1 { get; }
        Color Color2 { get; }
        Color Color3 { get; }
        Color Color4 { get; }
        int NumberDesignNumber1 { get; }
        int NumberDesignNumber2 { get; }
        Color NumberColor1 { get; }
        Color NumberColor2 { get; }
        Color NumberColor3 { get; }
    }
}
