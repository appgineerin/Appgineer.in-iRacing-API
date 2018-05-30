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
using AiRAPI.Data.Entity;

namespace AiRAPI.Data.Camera
{
    public interface ICameraGroup : INotifyPropertyChanged, IEquatable<ICameraGroup>, IComparable<ICameraGroup>
    {
        int Id { get; }
        string Name { get; }

        void Show();
        void Show(int numberPadded);
        void Show(IEntity entity);
    }
}
