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

using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Entity
{
    internal sealed class CarMovement : BindableBase, ICarMovement
    {
        private float _speed;
        public float Speed
        {
            get => _speed;
            internal set => SetProperty(ref _speed, value);
        }

        private bool _isAccelerating;
        public bool IsAccelerating
        {
            get => _isAccelerating;
            internal set => SetProperty(ref _isAccelerating, value);
        }

        private bool _isBraking;
        public bool IsBraking
        {
            get => _isBraking;
            internal set => SetProperty(ref _isBraking, value);
        }

        private double _trackPct;
        public double TrackPct
        {
            get => _trackPct;
            internal set => SetProperty(ref _trackPct, value);
        }

        private int _gear;
        public int Gear
        {
            get => _gear;
            internal set => SetProperty(ref _gear, value);
        }

        private int _rpm;
        public int Rpm
        {
            get => _rpm;
            internal set => SetProperty(ref _rpm, value);
        }

        private TrackLocation _trackLocation;
        public TrackLocation TrackLocation
        {
            get => _trackLocation;
            internal set => SetProperty(ref _trackLocation, value);
        }

        private bool _isInPits;
        public bool IsInPits
        {
            get => _isInPits;
            internal set => SetProperty(ref _isInPits, value);
        }

        private bool _isOnJokerLap;
        public bool IsOnJokerLap
        {
            get { return _isOnJokerLap; }
            internal set { SetProperty(ref _isOnJokerLap, value); }
        }

        private bool _isOnJokerSection;
        public bool IsOnJokerSection
        {
            get { return _isOnJokerSection; }
            internal set { SetProperty(ref _isOnJokerSection, value); }
        }
    }
}
