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

namespace AiRAPI.Data.Camera
{
    public sealed class CameraChangedEventArgs : EventArgs
    {
        public ICameraGroup OldCameraGroup { get; }
        public ICameraGroup NewCameraGroup { get; }

        public CameraChangedEventArgs(ICameraGroup oldCameraGroup, ICameraGroup newCameraGroup)
        {
            OldCameraGroup = oldCameraGroup;
            NewCameraGroup = newCameraGroup;
        }
    }
}
