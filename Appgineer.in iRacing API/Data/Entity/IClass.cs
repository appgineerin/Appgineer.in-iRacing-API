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

namespace AiRAPI.Data.Entity
{
    public interface IClass : INotifyPropertyChanged, IEquatable<IClass>, IComparable<IClass>
    {
        byte Id { get; set; }
        int Order { get; set; }
        int RelativeSpeed { get; set; }
        string Name { get; set; }
        Color Color { get; set; }
        int StrengthOfClass { get; set; }
        bool IsCustomClass { get; set; }
    }
}
