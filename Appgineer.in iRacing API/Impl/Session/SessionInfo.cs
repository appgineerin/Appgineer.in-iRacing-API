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

using AiRAPI.Data.Session;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Session
{
    internal sealed class SessionInfo : BindableBase, ISessionInfo
    {
        private int _seriesId;
        public int SeriesId
        {
            get => _seriesId;
            internal set => SetProperty(ref _seriesId, value);
        }

        private int _seasonId;
        public int SeasonId
        {
            get => _seasonId;
            internal set => SetProperty(ref _seasonId, value);
        }

        private int _sessionId;
        public int SessionId
        {
            get => _sessionId;
            internal set => SetProperty(ref _sessionId, value);
        }

        private int _subSessionId;
        public int SubSessionId
        {
            get => _subSessionId;
            internal set => SetProperty(ref _subSessionId, value);
        }

        private int _leagueId;
        public int LeagueId
        {
            get => _leagueId;
            internal set => SetProperty(ref _leagueId, value);
        }

        private bool _isOfficial;
        public bool IsOfficial
        {
            get => _isOfficial;
            internal set => SetProperty(ref _isOfficial, value);
        }

        private int _raceWeek;
        public int RaceWeek
        {
            get => _raceWeek;
            internal set => SetProperty(ref _raceWeek, value);
        }

        private string _eventType;
        public string EventType
        {
            get => _eventType;
            internal set => SetProperty(ref _eventType, value);
        }

        private string _category;
        public string Category
        {
            get => _category;
            internal set => SetProperty(ref _category, value);
        }

        private string _simMode;
        public string SimMode
        {
            get => _simMode;
            internal set => SetProperty(ref _simMode, value);
        }

        private bool _isTeamRacing;
        public bool IsTeamRacing
        {
            get => _isTeamRacing;
            internal set => SetProperty(ref _isTeamRacing, value);
        }

        private int _minDrivers;
        public int MinDrivers
        {
            get => _minDrivers;
            internal set => SetProperty(ref _minDrivers, value);
        }

        private int _maxDrivers;
        public int MaxDrivers
        {
            get => _maxDrivers;
            internal set => SetProperty(ref _maxDrivers, value);
        }

        private string _dcRuleSet;
        public string DCRuleSet
        {
            get => _dcRuleSet;
            internal set => SetProperty(ref _dcRuleSet, value);
        }

        private bool _qualifierMustStartRace;
        public bool QualifierMustStartRace
        {
            get => _qualifierMustStartRace;
            internal set => SetProperty(ref _qualifierMustStartRace, value);
        }

        private int _numCarClasses;
        public int NumCarClasses
        {
            get => _numCarClasses;
            internal set => SetProperty(ref _numCarClasses, value);
        }

        private int _numCarTypes;
        public int NumCarTyps
        {
            get => _numCarTypes;
            internal set => SetProperty(ref _numCarTypes, value);
        }

        private bool _isHeatRacing;
        public bool IsHeatRacing
        {
            get { return _isHeatRacing; }
            internal set { SetProperty(ref _isHeatRacing, value); }
        }

        private bool _isConsolationStacked;
        public bool IsConsolationStacked
        {
            get { return _isConsolationStacked; }
            internal set { SetProperty(ref _isConsolationStacked, value); }
        }

        private int _numAdvanceHeat;
        public int NumAdvanceHeat
        {
            get { return _numAdvanceHeat; }
            internal set { SetProperty(ref _numAdvanceHeat, value); }
        }

        private int _numAdvanceConsolation;
        public int NumAdvanceConsolation
        {
            get { return _numAdvanceConsolation; }
            internal set { SetProperty(ref _numAdvanceConsolation, value); }
        }

        private int _numJokerLaps;
        public int NumJokerLaps
        {
            get { return _numJokerLaps; }
            internal set { SetProperty(ref _numJokerLaps, value); }
        }
    }
}
