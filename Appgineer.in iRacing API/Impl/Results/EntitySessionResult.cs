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
using System.Diagnostics;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Lap;
using AiRAPI.Data.Results;
using AiRAPI.Impl.Entity;
using AiRAPI.Impl.Lap;
using AiRAPI.MVVM;

// ReSharper disable ExplicitCallerInfoArgument

namespace AiRAPI.Impl.Results
{
    internal sealed class EntitySessionResult : BindableBase, IEntitySessionResult
    {
        public IEntity Entity { get; }
        public ISessionResult Session { get; }

        private int _position = int.MaxValue;
        public int Position
        {
            get => _position;
            internal set => SetProperty(ref _position, value);
        }

        private int _livePosition = int.MaxValue;
        public int LivePosition
        {
            get => _livePosition;
            internal set => SetProperty(ref _livePosition, value);
        }

        private int _highestPosition;
        public int HighestPosition
        {
            get => _highestPosition;
            internal set => SetProperty(ref _highestPosition, value);
        }

        private int _lowestPosition;
        public int LowestPosition
        {
            get => _lowestPosition;
            internal set => SetProperty(ref _lowestPosition, value);
        }

        private int _startPosition;
        public int StartPosition
        {
            get => _startPosition;
            internal set => SetProperty(ref _startPosition, value);
        }

        private int _classPosition;
        public int ClassPosition
        {
            get => _classPosition;
            internal set => SetProperty(ref _classPosition, value);
        }

        private int _liveClassPosition;
        public int LiveClassPosition
        {
            get => _liveClassPosition;
            internal set => SetProperty(ref _liveClassPosition, value);
        }

        private int _classHighestPosition;
        public int ClassHighestPosition
        {
            get => _classHighestPosition;
            internal set => SetProperty(ref _classHighestPosition, value);
        }

        private int _classLowestPosition;
        public int ClassLowestPosition
        {
            get => _classLowestPosition;
            internal set => SetProperty(ref _classLowestPosition, value);
        }

        private int _classStartPosition;
        public int ClassStartPosition
        {
            get => _classStartPosition;
            internal set => SetProperty(ref _classStartPosition, value);
        }

        private int _lapsLed;
        public int LapsLed
        {
            get => _lapsLed;
            internal set => SetProperty(ref _lapsLed, value);
        }

        private int _classLapsLed;
        public int ClassLapsLed
        {
            get => _classLapsLed;
            internal set => SetProperty(ref _classLapsLed, value);
        }

        private float _gap;
        public float Gap
        {
            get => _gap;
            internal set => SetProperty(ref _gap, value);
        }

        private float _liveGap;
        public float LiveGap
        {
            get => _liveGap;
            internal set => SetProperty(ref _liveGap, value);
        }

        private int _gapLaps;
        public int GapLaps
        {
            get => _gapLaps;
            internal set => SetProperty(ref _gapLaps, value);
        }

        private int _liveGapLaps;
        public int LiveGapLaps
        {
            get => _liveGapLaps;
            internal set => SetProperty(ref _liveGapLaps, value);
        }

        private float _classGap;
        public float ClassGap
        {
            get => _classGap;
            internal set => SetProperty(ref _classGap, value);
        }

        private float _liveClassGap;
        public float LiveClassGap
        {
            get => _liveClassGap;
            internal set => SetProperty(ref _liveClassGap, value);
        }

        private int _classGapLaps;
        public int ClassGapLaps
        {
            get => _classGapLaps;
            internal set => SetProperty(ref _classGapLaps, value);
        }

        private int _liveClassGapLaps;
        public int LiveClassGapLaps
        {
            get => _liveClassGapLaps;
            internal set => SetProperty(ref _liveClassGapLaps, value);
        }

        private float _interval;
        public float Interval
        {
            get => _interval;
            internal set => SetProperty(ref _interval, value);
        }

        private float _liveInternal;
        public float LiveInterval
        {
            get => _liveInternal;
            internal set => SetProperty(ref _liveInternal, value);
        }

        private int _intervalLaps;
        public int IntervalLaps
        {
            get => _intervalLaps;
            internal set => SetProperty(ref _intervalLaps, value);
        }

        private int _liveIntervalLaps;
        public int LiveIntervalLaps
        {
            get => _liveIntervalLaps;
            internal set => SetProperty(ref _liveIntervalLaps, value);
        }

        private float _intervalBehind;
        public float IntervalBehind
        {
            get => _intervalBehind;
            internal set => SetProperty(ref _intervalBehind, value);
        }

        private float _liveIntervalBehind;
        public float LiveIntervalBehind
        {
            get => _liveIntervalBehind;
            internal set => SetProperty(ref _liveIntervalBehind, value);
        }

        private int _intervalLapsBehind;
        public int IntervalLapsBehind
        {
            get => _intervalLapsBehind;
            internal set => SetProperty(ref _intervalLapsBehind, value);
        }

        private int _liveIntervalLapsBehind;
        public int LiveIntervalLapsBehind
        {
            get => _liveIntervalLapsBehind;
            internal set => SetProperty(ref _liveIntervalLapsBehind, value);
        }

        private IEntitySessionResult _ahead;
        public IEntitySessionResult Ahead
        {
            get => _ahead;
            internal set => SetProperty(ref _ahead, value);
        }

        private IEntitySessionResult _liveAhead;
        public IEntitySessionResult LiveAhead
        {
            get => _liveAhead;
            internal set => SetProperty(ref _liveAhead, value);
        }

        private IEntitySessionResult _behind;
        public IEntitySessionResult Behind
        {
            get => _behind;
            internal set => SetProperty(ref _behind, value);
        }

        private IEntitySessionResult _liveBehind;
        public IEntitySessionResult LiveBehind
        {
            get => _liveBehind;
            internal set => SetProperty(ref _liveBehind, value);
        }

        private float _fastestLapTime;
        public float FastestLapTime
        {
            get => _fastestLapTime;
            internal set
            {
                if (SetProperty(ref _fastestLapTime, value))
                    OnPropertyChanged(nameof(CurrentLapTime));
            }
        }

        private float _lastLapTime;
        public float LastLapTime
        {
            get => _lastLapTime;
            internal set
            {
                if (SetProperty(ref _lastLapTime, value))
                    OnPropertyChanged(nameof(CurrentLapTime));
            }
        }

        public double CurrentLapTime
        {
            get
            {
                var laptime = CurrentLap.Time;
                if (laptime / 60 > 60)
                    return -1;

                if (laptime >= 5)
                    return Entity.Car.Movement.TrackLocation == TrackLocation.NotInWorld ? _fastestLapTime : laptime;

                if (_lastLapTime < 5)
                    return -1;

                return _lastLapTime;
            }
        }

        private IncompleteLap _currentLap;
        public IncompleteLap CurrentLap 
        {
            get => _currentLap;
            internal set => SetProperty(ref _currentLap, value);
        }

        ILap IEntitySessionResult.CurrentLap => CurrentLap;

        private CompletedLap _previousLap;
        public CompletedLap PreviousLap
        {
            get => _previousLap;
            internal set => SetProperty(ref _previousLap, value);
        }

        ILap IEntitySessionResult.PreviousLap => PreviousLap;

        private CompletedLap _fastestLap;
        public CompletedLap FastestLap
        {
            get => _fastestLap;
            internal set => SetProperty(ref _fastestLap, value);
        }

        ILap IEntitySessionResult.FastestLap => FastestLap;
        
        public LapCollection Laps { get; }
        ReadOnlyObservableCollection<ILap> IEntitySessionResult.Laps => Laps;

        public ReadOnlyDictionary<int, ILap> LapsMap { get; }
        
        private double _currentTrackPct;
        public double CurrentTrackPct
        {
            get => _currentTrackPct;
            internal set
            {
                if (SetProperty(ref _currentTrackPct, value))
                    OnPropertyChanged(nameof(CurrentLapTime));
            }
        }

        private int _stint;
        public int Stint
        {
            get => _stint;
            internal set => SetProperty(ref _stint, value);
        }

        private int _pitStopCount;
        public int PitStopCount
        {
            get => _pitStopCount;
            internal set => SetProperty(ref _pitStopCount, value);
        }

        private IPitStop _currentPitStop;
        public IPitStop CurrentPitStop
        {
            get => _currentPitStop;
            internal set => SetProperty(ref _currentPitStop, value);
        }

        internal ObservableCollection<IPitStop> PitStopsInt;
        public ReadOnlyObservableCollection<IPitStop> PitStops { get; }

        private bool _finished;
        public bool Finished
        {
            get => _finished;
            internal set => SetProperty(ref _finished, value);
        }

        private bool _out;
        public bool Out
        {
            get => _out;
            internal set => SetProperty(ref _out, value);
        }

        private bool _ditNotStart;
        public bool DidNotStart
        {
            get => _ditNotStart;
            internal set => SetProperty(ref _ditNotStart, value);
        }

        private int _incidents;
        public int Incidents
        {
            get => _incidents;
            internal set => SetProperty(ref _incidents, value);
        }

        private TimeSpan _airTime;
        public TimeSpan AirTime
        {
            get => _airTime;
            set => SetProperty(ref _airTime, value);
        }

        private string _reasonOutString;
        public string ReasonOutString
        {
            get => _reasonOutString;
            internal set => SetProperty(ref _reasonOutString, value);
        }

        private double _invisibleSince;
        public double InvisibleSince
        {
            get => _invisibleSince;
            set => SetProperty(ref _invisibleSince, value);
        }

        private double? _pitLaneEntryTime;
        public double? PitLaneEntryTime
        {
            get => _pitLaneEntryTime;
            set => SetProperty(ref _pitLaneEntryTime, value);
        }

        private double? _pitStopStartTime;
        public double? PitStopStartTime
        {
            get => _pitStopStartTime;
            set => SetProperty(ref _pitStopStartTime, value);
        }

        private double? _pitStopEndTime;
        public double? PitStopEndTime
        {
            get => _pitStopEndTime;
            set => SetProperty(ref _pitStopEndTime, value);
        }

        private double? _pitLaneExitTime;
        public double? PitLaneExitTime
        {
            get => _pitLaneExitTime;
            set => SetProperty(ref _pitLaneExitTime, value);
        }

        private bool _isAdvancing;
        public bool IsAdvancing
        {
            get { return _isAdvancing; }
            internal set { SetProperty(ref _isAdvancing, value); }
        }

        private int _jokerLapsCompleted;
        public int JokerLapsCompleted
        {
            get { return _jokerLapsCompleted; }
            internal set { SetProperty(ref _jokerLapsCompleted, value); }
        }

        private bool _isParticipating;
        public bool IsParticipating
        {
            get { return _isParticipating; }
            internal set { _isParticipating = value; }
        }

        internal double PrevLapNumber { get; set; }
        internal double PrevTrackPct { get; set; }
        internal double PrevTrackPctUpdate { get; set; }
        internal double PrevTrackSpeedPct { get; set; }
        internal double PrevTrackSpeedPctUpdate { get; set; }

        internal bool HasIncrementedCounter { get; set; }
        
        internal EntitySessionResult(ISessionResult session, IEntity entity)
        {
            Debug.Assert(entity != null);
            Debug.Assert(session != null);

            Entity = entity;
            Session = session;

            _currentLap = new IncompleteLap(this);
            _previousLap = new CompletedLap(this);
            _fastestLap = new CompletedLap(this);
            _currentPitStop = new PitStop();

            Laps = new LapCollection();
            LapsMap = new ReadOnlyDictionary<int, ILap>(Laps.Map);

            PitStopsInt = new ObservableCollection<IPitStop>();
            PitStops = new ReadOnlyObservableCollection<IPitStop>(PitStopsInt);
        }

        public int CompareTo(IEntitySessionResult other)
        {
            return _position.CompareTo(other.Position);
        }

        internal void AddAirTime(double time)
        {
            if (time > 0)
                AirTime = AirTime.Add(TimeSpan.FromSeconds(time));
        }
    }
}
