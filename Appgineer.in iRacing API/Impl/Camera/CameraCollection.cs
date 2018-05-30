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
using AiRAPI.Data.Camera;

namespace AiRAPI.Impl.Camera
{
    internal sealed class CameraCollection : ReadOnlyObservableCollection<ICameraGroup>
    {
        public CameraCollection() : base(new ObservableCollection<ICameraGroup>()) { }

        internal void Add(CameraGroup group)
        {
            Items.Add(group);
        }

        internal void Clear()
        {
            Items.Clear();
        }
    }
}
