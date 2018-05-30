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
using AiRAPI.Data.Entity;

namespace AiRAPI.Data.Camera
{
    public sealed class FollowerChangedEventArgs : EventArgs
    {
        public IEntity OldEntity { get; }
        public IEntity NewEntity { get; }

        public FollowerChangedEventArgs(IEntity oldEntity, IEntity newEntity)
        {
            OldEntity = oldEntity;
            NewEntity = newEntity;
        }
    }
}
