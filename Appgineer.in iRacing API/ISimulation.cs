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
using System.ComponentModel;
using System.Threading;
using AiRAPI.Calculators;
using AiRAPI.Data;
using AiRAPI.Data.Camera;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Results;
using AiRAPI.Data.Session;
using AiRAPI.Event;

// ReSharper disable InconsistentNaming

namespace AiRAPI
{
    public interface ISimulation : INotifyPropertyChanged
    {
        Mutex DataMutex { get; }
        object SharedCollectionLock { get; }
        
        byte UpdateRate { get; set; }
        bool HideSimUI { get; set; }
        bool IsConnected { get; }
        
        bool UseMetricUnits { get; set; }
        bool UseLiveData { get; set; }
        
        ITelemetry Telemetry { get; }
        ICameraManager CameraManager { get; }
        ITimeDelta TimeDelta { get; }
        ISession Session { get; }
        ISimEventManager EventManager { get; }
        int CurrentRadioCarIdx { get; }
        
        event EventHandler<ISessionResult> CurrentSessionChangedEvent;
        event EventHandler<ISimulation> DataUpdatedEvent;
        
        void Start();
        void Stop();
        object GetData(string key);
        T GetData<T>(string key, T def = default(T));
        void BroadcastMessage(BroadcastMessageType type, int var1, int var2);
        void BroadcastMessage(BroadcastMessageType type, int var1, int var2, int var3);
        void AddEvent(EventType type);
    }
}
