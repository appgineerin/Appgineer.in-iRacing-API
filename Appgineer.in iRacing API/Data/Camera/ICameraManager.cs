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
using System.Collections.ObjectModel;
using System.ComponentModel;
using AiRAPI.Data.Entity;

namespace AiRAPI.Data.Camera
{
    public interface ICameraManager : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<ICameraGroup> CameraGroups { get; }
        int CurrentGroupId { get; }
        ICameraGroup CurrentGroup { get; }
        IEntity FollowedEntity { get; }
        bool IsInReplay { get; }

        event EventHandler<CameraChangedEventArgs> CameraChanged;
        event EventHandler<FollowerChangedEventArgs> FollowerChanged;

        ICameraGroup GetCameraGroup(int id);
        ICameraGroup GetCameraGroup(string name);

        void Show(int id);
        void Show(string name);
        void Show(ICameraGroup cameraGroup);
        void Show(int id, int numberPadded);
        void Show(string name, int numberPadded);
        void Show(ICameraGroup cameraGroup, int numberPadded);
        void Show(int id, IEntity entity);
        void Show(string name, IEntity entity);
        void Show(ICameraGroup cameraGroup, IEntity entity);
        void Show(IEntity entity);
    }
}
