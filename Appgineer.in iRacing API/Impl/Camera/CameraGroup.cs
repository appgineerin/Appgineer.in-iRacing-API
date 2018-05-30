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

using AiRAPI.Data.Camera;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.MVVM;
using AiRAPI.SDK;

namespace AiRAPI.Impl.Camera
{
    internal sealed class CameraGroup : BindableBase, ICameraGroup
    {
        private int _id;
        public int Id
        {
            get => _id;
            internal set => SetProperty(ref _id, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            internal set => SetProperty(ref _name, value);
        }

        public int CompareTo(ICameraGroup other)
        {
            return _id.CompareTo(other.Id);
        }

        public bool Equals(ICameraGroup other)
        {
            return _id == other?.Id && _name == other.Name;
        }

        public void Show()
        {
            iRacingSDK.BroadcastMessage(BroadcastMessageType.CamSwitchNum, ApiProvider.Sim.CameraManager.FollowedEntity?.Car.NumberPadded ?? -1, _id);
        }

        public void Show(IEntity entity)
        {
            if (entity == null)
                Show(0);
            else
                iRacingSDK.BroadcastMessage(BroadcastMessageType.CamSwitchNum, entity.Car.NumberPadded, _id);
        }

        public void Show(int numberPadded)
        {
            iRacingSDK.BroadcastMessage(BroadcastMessageType.CamSwitchNum, numberPadded, _id);
        }
    }
}
