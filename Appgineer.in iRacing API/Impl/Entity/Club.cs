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

using System.Windows.Media.Imaging;
using AiRAPI.Data.Entity;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Entity
{
    internal sealed class Club : BindableBase, IClub
    {
        public string Name { get; }
        public BitmapSource LogoLarge { get; }
        public BitmapSource LogoSmall { get; }

        internal Club(string name, BitmapSource logoLarge, BitmapSource logoSmall)
        {
            Name = name;
            LogoLarge = logoLarge;
            LogoSmall = logoSmall;
        }
        
        public bool Equals(IClub other)
        {
            return other != null && Name == other.Name;
        }
    }
}
