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
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Windows.Threading;
using AiRAPI.Calculators;
using AiRAPI.Data;
using AiRAPI.Data.Camera;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Results;
using AiRAPI.Data.Session;
using AiRAPI.Event;
using AiRAPI.Impl.Calculators;
using AiRAPI.Impl.Camera;
using AiRAPI.Impl.Event;
using AiRAPI.Impl.Updater;
using AiRAPI.MVVM;
using AiRAPI.SDK;

namespace AiRAPI.Impl
{
    internal sealed class Simulation : BindableBase, ISimulation
    {
        public Mutex DataMutex { get; }
        public object SharedCollectionLock { get; }
        public byte UpdateRate { get; set; }

        private double _dataDelay;
        public double DataDelay
        {
            get => _dataDelay;
            set => SetProperty(ref _dataDelay, value);
        }

        private double _dataFps;
        public double DataFps
        {
            get => _dataFps;
            set => SetProperty(ref _dataFps, value);
        }

        private bool _hideSimUi;
        public bool HideSimUI
        {
            get => _hideSimUi;
            set => SetProperty(ref _hideSimUi, value);
        }

        private bool _isConntected;
        public bool IsConnected
        {
            get => _isConntected;
            private set
            {
                if (!SetProperty(ref _isConntected, value))
                    return;

                if (value)
                {
                    AddEvent(EventType.Connected);
                }
                else
                {
                    TimeDelta = new TimeDelta(1000, 300, 64);
                    Session = new Session.Session();
                    Sdk = new iRacingSDK();
                }
            }
        }

        private bool _useMetricUnits = true;
        public bool UseMetricUnits
        {
            get => _useMetricUnits;
            set => SetProperty(ref _useMetricUnits, value);
        }

        private bool _useLiveData = true;
        public bool UseLiveData
        {
            get => _useLiveData;
            set => SetProperty(ref _useLiveData, value);
        }

        public ITelemetry Telemetry { get; }
        public ICameraManager CameraManager { get; }
        public ITimeDelta TimeDelta { get; internal set; }
        
        private ISession _session;
        public ISession Session
        {
            get => _session;
            private set => SetProperty(ref _session, value);
        }

        public ISimEventManager EventManager { get; }

        private int _currentRadioCarIdx = -1;
        public int CurrentRadioCarIdx
        {
            get => _currentRadioCarIdx;
            internal set => SetProperty(ref _currentRadioCarIdx, value);
        }
        
        public event EventHandler<ISessionResult> CurrentSessionChangedEvent;
        public event EventHandler<ISimulation> DataUpdatedEvent;

        internal iRacingSDK Sdk;
        internal readonly Stack Triggers;

        private int _nextConnectTry;
        private Thread _simThread;
        private readonly DispatcherTimer _updateTimer;
        private bool _runSdk;
        private readonly DataUpdater _updater;

        public Simulation()
        {
            DataMutex = new Mutex();
            SharedCollectionLock = new object();
            UpdateRate = 30;

            Telemetry = new Telemetry();
            CameraManager = new CameraManager();
            TimeDelta = new TimeDelta(1000, 300, 64);
            _session = new Session.Session();
            EventManager = new SimEventManager(this);

            Sdk = new iRacingSDK();
            Triggers = new Stack();

            _nextConnectTry = Environment.TickCount;
            _runSdk = false;
            _updater = new DataUpdater();

            _updateTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _updateTimer.Tick += Connect;
        }
        
        public void Start()
        {
            _runSdk = true;
            _updateTimer.Start();
        }

        public void Stop()
        {
            _updateTimer.Stop();
            _runSdk = false;
        }

        public object GetData(string key)
        {
            return Sdk.GetData(key);
        }

        public T GetData<T>(string key, T def = default(T))
        {
            try
            {
                return (T)GetData(key);
            }
            catch
            {
                return def;
            }
        }

        internal void HideUi()
        {
            if ((Telemetry.CamCameraState & CameraState.UIHidden) != CameraState.UIHidden)
                iRacingSDK.BroadcastMessage(BroadcastMessageType.CamSetState, (int)(Telemetry.CamCameraState | CameraState.UIHidden), 0);
        }

        internal void RaiseSessionChangedEvent(ISessionResult session)
        {
            CurrentSessionChangedEvent?.Invoke(this, session);
        }
        
        private void Connect(object sender, EventArgs e)
        {
            if (!Sdk.IsInitialized || !Sdk.IsConnected())
            {
                if (Environment.TickCount <= _nextConnectTry)
                    return;

                Connect();
                _nextConnectTry = Environment.TickCount + 5000;
            }
            else
            {
                if (_simThread != null && _simThread.IsAlive)
                    return;

                _simThread = new Thread(UpdateData) { IsBackground = true };
                _simThread.Start();
            }
        }

        private void Connect()
        {
            try
            {
                Sdk.Startup();
            }
            catch
            {
                // ignored
            }
        }

        private long _lastTick;

        private void UpdateData()
        {
            _updater.LastSessionInfoUpdate = -1;

            while (_runSdk)
            {
#if !DEBUG
                try
                {
#endif
                    var tickStart = DateTime.UtcNow.Ticks;
                    IsConnected = Sdk.IsInitialized && Sdk.IsConnected();
                    if (!_isConntected || !_updater.UpdateData(ref Sdk, this))
                        return;

                    DataUpdatedEvent?.Invoke(this, this);

                    var tickEnd = DateTime.UtcNow.Ticks;
                    var duration = (tickEnd - tickStart) / 10000d;

                    DataDelay = duration;
                    DataFps = 1000d / (tickEnd - _lastTick) * 10000d;

                    _lastTick = tickEnd;

                    var maxDuration = 1000d / UpdateRate;
                    if (duration < maxDuration)
                    {
                        Thread.Sleep((int) (maxDuration - duration));
                    }
                    else
                    {
                        Debug.WriteLine($"System overloaded by {duration - maxDuration}ms.");
                    }
#if !DEBUG
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
#endif
            }
        }

        public void BroadcastMessage(BroadcastMessageType type, int var1, int var2)
        {
            iRacingSDK.BroadcastMessage(type, var1, var2);
        }

        public void BroadcastMessage(BroadcastMessageType type, int var1, int var2, int var3)
        {
            iRacingSDK.BroadcastMessage(type, var1, var2, var3);
        }

        public void AddEvent(EventType type)
        {
            Triggers.Push(type);
        }
    }
}
