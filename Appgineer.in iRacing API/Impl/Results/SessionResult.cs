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
using System.Linq;
using AiRAPI.Calculators;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Results;
using AiRAPI.Impl.Calculators;
using AiRAPI.MVVM;
using AiRAPI.Ordering;

namespace AiRAPI.Impl.Results
{
    internal sealed class SessionResult : BindableBase, ISessionResult
    {
        internal readonly ObservableCollection<IEntitySessionResult> ResultsInt;
        public ReadOnlyObservableCollection<IEntitySessionResult> Results { get; }

        internal readonly ObservableCollection<ISessionQualifyPosition> QualifyPositionsInt;
        public ReadOnlyObservableCollection<ISessionQualifyPosition> QualifyPositions { get; }

        internal readonly ObservableCollection<IEntity> ParticipatingEntitiesInt;
        public ReadOnlyObservableCollection<IEntity> ParticipatingEntities { get; }

        private IEntity _leader;
        public IEntity Leader
        {
            get => _leader;
            internal set => SetProperty(ref _leader, value);
        }

        private IEntity _liveLeader;
        public IEntity LiveLeader
        {
            get => _liveLeader;
            private set => SetProperty(ref _liveLeader, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            internal set => SetProperty(ref _name, value);
        }

        private SessionType _type;
        public SessionType Type
        {
            get => _type;
            internal set => SetProperty(ref _type, value);
        }

        private SessionSubType _subType;
        public SessionSubType SubType
        {
            get => _subType;
            internal set => SetProperty(ref _subType, value);
        }

        private bool _skipped;
        public bool Skipped
        {
            get => _skipped;
            internal set => SetProperty(ref _skipped, value);
        }

        private bool _hasStarted;
        public bool HasStarted
        {
            get  => _hasStarted; 
            internal set => SetProperty(ref _hasStarted, value);
        }

        private bool _hasFinished;
        public bool HasFinished
        {
            get => _hasFinished;
            internal set => SetProperty(ref _hasFinished, value);
        }

        private bool _isCurrent;
        public bool IsCurrent
        {
            get => _isCurrent;
            internal set => SetProperty(ref _isCurrent, value);
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            internal set => SetProperty(ref _isSelected, value);
        }

        private bool _runGroupsUsed;
        public bool RunGroupsUsed
        {
            get => _runGroupsUsed;
            internal set => SetProperty(ref _runGroupsUsed, value);
        }

        private SessionState _state;
        public SessionState State
        {
            get => _state;
            internal set => SetProperty(ref _state, value);
        }

        private SessionFlags _flags;
        public SessionFlags Flags
        {
            get => _flags;
            internal set => SetProperty(ref _flags, value);
        }

        private IDriver _fastestLapDriver;
        public IDriver FastestLapDriver
        {
            get => _fastestLapDriver;
            internal set => SetProperty(ref _fastestLapDriver, value);
        }

        private int _fastestLapNumber;
        public int FastestLapNumber
        {
            get => _fastestLapNumber;
            internal set => SetProperty(ref _fastestLapNumber, value);
        }

        private float _fastestLapTime;
        public float FastestLapTime
        {
            get => _fastestLapTime;
            internal set => SetProperty(ref _fastestLapTime, value);
        }
        
        private int _id;
        public int Id
        {
            get => _id;
            internal set => SetProperty(ref _id, value);
        }

        private int _leadChanges;
        public int LeadChanges
        {
            get => _leadChanges;
            internal set => SetProperty(ref _leadChanges, value);
        }

        private int _cautions;
        public int Cautions
        {
            get => _cautions;
            internal set => SetProperty(ref _cautions, value);
        }

        private int _cautionLaps;
        public int CautionLaps
        {
            get => _cautionLaps;
            internal set => SetProperty(ref _cautionLaps, value);
        }

        private double _sessionLength;
        public double SessionLength
        {
            get => _sessionLength;
            internal set => SetProperty(ref _sessionLength, value);
        }

        private double _sessionTime;
        public double SessionTime
        {
            get => _sessionTime;
            internal set => SetProperty(ref _sessionTime, value);
        }

        private double _sessionTimeRemaining;
        public double SessionTimeRemaining
        {
            get => _sessionTimeRemaining;
            internal set => SetProperty(ref _sessionTimeRemaining, value);
        }

        private double _sessionStartTime = -1;
        public double SessionStartTime
        {
            get => _sessionStartTime;
            internal set => SetProperty(ref _sessionStartTime, value);
        }

        private int _lapsTotal;
        public int LapsTotal
        {
            get => _lapsTotal;
            internal set => SetProperty(ref _lapsTotal, value);
        }

        private int _lapsCompleted;
        public int LapsCompleted
        {
            get => _lapsCompleted;
            internal set => SetProperty(ref _lapsCompleted, value);
        }

        private int _lapsRemaining;
        public int LapsRemaining
        {
            get => _lapsRemaining;
            internal set => SetProperty(ref _lapsRemaining, value);
        }

        private int _finishLine;
        public int FinishLine
        {
            get => _finishLine;
            internal set => SetProperty(ref _finishLine, value);
        }

        private int _currentReplayPosition;
        public int CurrentReplayPosition
        {
            get => _currentReplayPosition;
            internal set => SetProperty(ref _currentReplayPosition, value);
        }

        private bool _pitOccupied;
        public bool PitOccupied
        {
            get => _pitOccupied;
            private set => SetProperty(ref _pitOccupied, value);
        }

        private bool _hasEstimatedSessionLength;
        public bool HasEstimatedSessionLength
        {
            get => _hasEstimatedSessionLength;
            private set => SetProperty(ref _hasEstimatedSessionLength, value);
        }

        private bool _hasEstimatedLapsTotal;
        public bool HasEstimatedLapsTotal
        {
            get => _hasEstimatedLapsTotal;
            private set => SetProperty(ref _hasEstimatedLapsTotal, value);
        }

        private double _estimatedSessionLength;
        public double EstimatedSessionLength
        {
            get => _estimatedSessionLength;
            internal set => SetProperty(ref _estimatedSessionLength, value);
        }
        
        private double _estimatedSessionTimeRemaining;
        public double EstimatedSessionTimeRemaining
        {
            get => _estimatedSessionTimeRemaining;
            internal set => SetProperty(ref _estimatedSessionTimeRemaining, value);
        }

        private int _estimatedLapsTotal;
        public int EstimatedLapsTotal
        {
            get => _estimatedLapsTotal;
            internal set => SetProperty(ref _estimatedLapsTotal, value);
        }

        private int _estimatedLapsRemaining;
        public int EstimatedLapsRemaining
        {
            get => _estimatedLapsRemaining;
            internal set => SetProperty(ref _estimatedLapsRemaining, value);
        }

        private bool _sessionLengthDecidedByLaps;
        public bool SessionLengthDecidedByLaps
        {
            get => _sessionLengthDecidedByLaps;
            internal set => SetProperty(ref _sessionLengthDecidedByLaps, value);
        }

        private double _averageLaptime;
        public double AverageLaptime
        {
            get => _averageLaptime;
            set => SetProperty(ref _averageLaptime, value);
        }

        private bool _isFinalLap;
        public bool IsFinalLap
        {
            get => _isFinalLap;
            internal set => SetProperty(ref _isFinalLap, value);
        }

        private bool _hasQualifyPositions;
        public bool HasQualifyPositions
        {
            get { return _hasQualifyPositions; }
            internal set { _hasQualifyPositions = value; }
        }
        
        internal float PrevFastestLapTime;

        internal SessionResult()
        {
            QualifyPositionsInt = new ObservableCollection<ISessionQualifyPosition>();
            QualifyPositions = new ReadOnlyObservableCollection<ISessionQualifyPosition>(QualifyPositionsInt);
            
            ResultsInt = new ObservableCollection<IEntitySessionResult>();
            Results = new ReadOnlyObservableCollection<IEntitySessionResult>(ResultsInt);
        }

        public IEntitySessionResult GetClassLeader(string className)
        {
            return Results.FirstOrDefault(r => r.ClassPosition == 1 && r.Entity.Car.Class.Name == className);
        }

        public IEntitySessionResult GetLiveClassLeader(string className)
        {
            return Results.FirstOrDefault(r => r.LiveClassPosition == 1 && r.Entity.Car.Class.Name == className);
        }

        public IEntitySessionResult GetResult(int position, IDataOrder order)
        {
            return order.FindPosition(Results, position);
        }

        public IEntitySessionResult GetResult(int position, IDataOrder order, string className)
        {
            return order.FindPosition(Results, position, className);
        }

        public void EstimateSessionLength(double averageLeaderLaptimeSeconds)
        {
            if (averageLeaderLaptimeSeconds > 1)
            {
                HasEstimatedSessionLength = true;

                var sim = ApiProvider.Simulation;
                var currentLapTime = 0d;
                if (sim.IsConnected && sim.Telemetry != null && LiveLeader?.Results?.CurrentResult != null)
                {
                    // If current laptime < 120% of average, use it. If not, ignore current lap, it will fix itself next lap
                    var currTime = LiveLeader.Results.CurrentResult.CurrentLap.Time;
                    if (currTime >= 0 && currTime < 1.2 * averageLeaderLaptimeSeconds)
                    {
                        currentLapTime = currTime;
                    }
                }

                var lapsCompleted = LapsCompleted;
                if (LiveLeader?.Results?.CurrentResult?.CurrentLap != null)
                {
                    lapsCompleted = LiveLeader.Results.CurrentResult.CurrentLap.Number - 1;
                }

                var remainingLaps = LapsTotal - lapsCompleted;
                if (remainingLaps > LapsTotal) remainingLaps = LapsTotal;

                EstimatedSessionTimeRemaining = remainingLaps * averageLeaderLaptimeSeconds - currentLapTime;
                EstimatedSessionLength = SessionTime + EstimatedSessionTimeRemaining;
            }
            else
            {
                // Don't know yet
                HasEstimatedSessionLength = false;
            }
        }

        public void EstimateLapsTotal(double averageLeaderLaptimeSeconds)
        {
            if (averageLeaderLaptimeSeconds > 1)
            {
                HasEstimatedLapsTotal = true;
                EstimatedLapsTotal = (int) Math.Ceiling(SessionLength / averageLeaderLaptimeSeconds);
                EstimatedLapsRemaining = EstimatedLapsTotal - LapsCompleted;

                if (EstimatedLapsRemaining > EstimatedLapsTotal)
                    EstimatedLapsRemaining = EstimatedLapsTotal;
            }
            else
            {
                // Don't know yet
                HasEstimatedLapsTotal = false;
            }
        }
        
        private IAverageLapCalculator _averageLapCalculator;
        public IAverageLapCalculator AverageLapCalculator => _averageLapCalculator;

        public double GetAverageLaptimePrediction(ISimulation sim)
        {
            if (_averageLapCalculator == null || _averageLapCalculator.TrackId != sim.Session.Track.Id)
            {
                _averageLapCalculator = new AverageLapCalculator(sim.Session.Track);
            }

            try
            {
                _averageLapCalculator.Update(this, sim);
                AverageLaptime = _averageLapCalculator.Calculate();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error calculating average laptime: " + e);
            }

            return AverageLaptime;
        }

        internal void UpdatePosition(Simulation sim)
        {
            if (_type == SessionType.Race && 
                (sim.Session.Current.State == SessionState.Racing ||
                sim.Session.Current.State == SessionState.Checkered || 
                sim.Session.Current.State == SessionState.CoolDown))
            {
                var i = 1;
                foreach (var result in ResultsInt.OfType<EntitySessionResult>().OrderByDescending(r => r.CurrentTrackPct))
                {
                    if (i == 1)
                        LiveLeader = result.Entity;

                    result.LivePosition = result.Finished ? result.Position : i;
                    i++;
                }
            }
            else
            {
                foreach (var result in ResultsInt.OfType<EntitySessionResult>().OrderBy(r => r.Position))
                {
                    if (result.Position == 1)
                        LiveLeader = result.Entity;

                    result.LivePosition = result.Position;
                }
            }

            var classes = sim.Session.ClassManager.Classes;
            for (var i = 0; i < classes.Count; i++)
            {
                if (classes[i].Order == -1)
                    continue;

                if (Type == SessionType.Race &&
                    (sim.Session.Current.State == SessionState.Racing ||
                     sim.Session.Current.State == SessionState.Checkered ||
                     sim.Session.Current.State == SessionState.CoolDown))
                {
                    var p = 1;
                    var i1 = i;
                    foreach (var result in ResultsInt.OfType<EntitySessionResult>().Where(r => r.Entity.Car.Class.Order == i1).OrderByDescending(r => r.CurrentTrackPct))
                    {
                        result.LiveClassPosition = result.Finished ? result.ClassPosition : p;
                        p++;
                    }
                }
                else
                {
                    var i1 = i;
                    foreach (var result in ResultsInt.OfType<EntitySessionResult>().Where(r => r.Entity.Car.Class.Order == i1).OrderBy(r => r.ClassPosition))
                        result.LiveClassPosition = result.ClassPosition;
                }
            }
        }

        internal void CheckPitStatus()
        {
            PitOccupied = Type == SessionType.Race && ResultsInt.Any(r => r.Entity.Car.Movement.TrackLocation == TrackLocation.InPitStall);
        }

        internal void UpdateEntityLinks(Simulation sim)
        {
            EntitySessionResult ahead = null;
            foreach (var result in ResultsInt.OfType<EntitySessionResult>().OrderBy(r => r.Position))
            {
                result.Ahead = ahead;
                if (ahead != null)
                    ahead.Behind = result;

                ahead = result;
            }

            EntitySessionResult liveAhead = null;
            foreach (var result in ResultsInt.OfType<EntitySessionResult>().OrderBy(r => r.LivePosition))
            {
                result.LiveAhead = liveAhead;
                if (liveAhead != null)
                    liveAhead.LiveBehind = result;

                liveAhead = result;
            }

            var last = ResultsInt.OfType<EntitySessionResult>().OrderBy(r => r.Position).LastOrDefault();
            if (last != null)
                last.Behind = null;

            last = ResultsInt.OfType<EntitySessionResult>().OrderBy(r => r.LivePosition).LastOrDefault();
            if (last != null)
                last.LiveBehind = null;
        }

    }
}
