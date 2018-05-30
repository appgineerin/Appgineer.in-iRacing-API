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
using AiRAPI.Data.Location;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Location
{
    internal sealed class Sector : BindableBase, ISector
    {
        private byte _index;
        public byte Index
        {
            get => _index;
            internal set => SetProperty(ref _index, value);
        }

        private float _location;
        public float Location
        {
            get => _location;
            internal set => SetProperty(ref _location, value);
        }

        public bool Equals(ISector other)
        {
            if (other == null)
                return false;

            return _index == other.Index && Math.Abs(_location - other.Location) < 10E-6;
        }

        public int CompareTo(ISector other)
        {
            return _index.CompareTo(other.Index);
        }
    }
}
