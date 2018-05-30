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
using System.Linq;
using AiRAPI.Data.Camera;
using AiRAPI.Data.Entity;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Camera
{
    internal sealed class CameraManager : BindableBase, ICameraManager
    {
        internal CameraCollection CameraGroups { get; }
        ReadOnlyObservableCollection<ICameraGroup> ICameraManager.CameraGroups => CameraGroups;

        public event EventHandler<CameraChangedEventArgs> CameraChanged;
        public event EventHandler<FollowerChangedEventArgs> FollowerChanged;

        private int _currentGroupId;
        public int CurrentGroupId
        {
            get => _currentGroupId;
            set
            {
                var oldId = _currentGroupId;
                if (!SetProperty(ref _currentGroupId, value))
                    return;

                CurrentGroup = GetCameraGroup(value);
                RaiseCameraChanged(oldId, _currentGroupId);
            }
        }

        private ICameraGroup _currentGroup;
        public ICameraGroup CurrentGroup
        {
            get => _currentGroup;
            private set => SetProperty(ref _currentGroup, value);
        }

        private IEntity _followedEntity;
        public IEntity FollowedEntity
        {
            get => _followedEntity;
            private set
            {
                var oldEntity = _followedEntity;
                if (!SetProperty(ref _followedEntity, value))
                    return;

                if (oldEntity != null)
                    ((Entity.Entity)oldEntity).IsFollowedEntity = false;
                if (_followedEntity != null)
                    ((Entity.Entity)_followedEntity).IsFollowedEntity = true;

                RaiseFollowerChanged(oldEntity, _followedEntity);
            }
        }

        private bool _isInReplay;
        public bool IsInReplay
        {
            get => _isInReplay;
            set => SetProperty(ref _isInReplay, value);
        }
        
        internal CameraManager()
        {
            CameraGroups = new CameraCollection();
        }

        public ICameraGroup GetCameraGroup(int id)
        {
            return CameraGroups.SingleOrDefault(c => c.Id == id);
        }

        public ICameraGroup GetCameraGroup(string name)
        {
            return CameraGroups.SingleOrDefault(c => c.Name == name);
        }

        public void Show(int id)
        {
            GetCameraGroup(id)?.Show();
        }

        public void Show(string name)
        {
            GetCameraGroup(name)?.Show();
        }

        public void Show(ICameraGroup cameraGroup)
        {
            cameraGroup?.Show();
        }

        public void Show(int id, int numberPadded)
        {
            GetCameraGroup(id)?.Show(numberPadded);
        }

        public void Show(string name, int numberPadded)
        {
            GetCameraGroup(name)?.Show(numberPadded);
        }

        public void Show(ICameraGroup cameraGroup, int numberPadded)
        {
            cameraGroup?.Show(numberPadded);
        }

        public void Show(int id, IEntity entity)
        {
            GetCameraGroup(id)?.Show(entity);
        }

        public void Show(string name, IEntity entity)
        {
            GetCameraGroup(name)?.Show(entity);
        }

        public void Show(ICameraGroup cameraGroup, IEntity entity)
        {
            cameraGroup?.Show(entity);
        }

        public void Show(IEntity entity)
        {
            GetCameraGroup(_currentGroupId)?.Show(entity);
        }

        internal void SetFollowedEntity(int carIdx)
        {
            if (FollowedEntity != null && FollowedEntity.CarIdx == carIdx)
                return;

            FollowedEntity = ApiProvider.Sim.Session.Entities.FirstOrDefault(e => e.CarIdx == carIdx);
        }

        private void RaiseCameraChanged(int oldId, int newId)
        {
            CameraChanged?.Invoke(this, new CameraChangedEventArgs(GetCameraGroup(oldId), GetCameraGroup(newId)));
        }

        private void RaiseFollowerChanged(IEntity oldEntity, IEntity newEntity)
        {
            FollowerChanged?.Invoke(this, new FollowerChangedEventArgs(oldEntity, newEntity));
        }
    }
}
