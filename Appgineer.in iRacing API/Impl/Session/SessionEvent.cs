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
using AiRAPI.Data.Camera;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Session;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Session
{
    internal class SessionEvent : BindableBase, ISessionEvent
    {
        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get => _timestamp;
            internal set => SetProperty(ref _timestamp, value);
        }

        private long _replayPos;
        public long ReplayPos
        {
            get => _replayPos;
            internal set => SetProperty(ref _replayPos, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            internal set => SetProperty(ref _description, value);
        }

        private IEntity _entity;
        public IEntity Entity
        {
            get => _entity;
            internal set => SetProperty(ref _entity, value);
        }

        private ICameraGroup _cameraGroup;

        public ICameraGroup CameraGroup
        {
            get => _cameraGroup;
            set => SetProperty(ref _cameraGroup, value);
        }

        private SessionType _sessionType;
        public SessionType SessionType
        {
            get => _sessionType;
            internal set => SetProperty(ref _sessionType, value);
        }

        private SessionEventType _eventType;
        public SessionEventType EventType
        {
            get => _eventType;
            internal set => SetProperty(ref _eventType, value);
        }

        private int _lapNumber;
        public int LapNumber
        {
            get => _lapNumber;
            internal set => SetProperty(ref _lapNumber, value);
        }

        private int _rewind;
        public int Rewind
        {
            get => _rewind;
            internal set => SetProperty(ref _rewind, value);
        }

        internal SessionEvent(long replayPos, string description, IEntity entity, ICameraGroup cameraGroup, SessionType sessionType, SessionEventType eventType, int lapNumber)
        {
            _timestamp = DateTime.Now;
            _replayPos = replayPos;
            _description = description;
            _entity = entity;
            _cameraGroup = cameraGroup;
            _sessionType = sessionType;
            _eventType = eventType;
            _lapNumber = lapNumber;
            _rewind = 0;
        }

        public int CompareTo(ISessionEvent other)
        {
            return _timestamp.CompareTo(other.Timestamp);
        }
    }
}
