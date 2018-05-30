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

using System.Collections.ObjectModel;
using System.Linq;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Location;
using AiRAPI.Data.Results;
using AiRAPI.Data.Session;
using AiRAPI.Impl.Entity;
using AiRAPI.Impl.Location;
using AiRAPI.Impl.Results;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Session
{
    internal sealed class Session : BindableBase, ISession
    {
        private ISessionInfo _info;
        public ISessionInfo Info
        {
            get => _info;
            internal set => SetProperty(ref _info, value);
        }

        private ISessionOptions _options;
        public ISessionOptions Options
        {
            get => _options;
            internal set => SetProperty(ref _options, value);
        }

        private ITrack _track;
        public ITrack Track
        {
            get => _track;
            internal set => SetProperty(ref _track, value);
        }

        private IWeather _weather;
        public IWeather Weather
        {
            get => _weather;
            internal set => SetProperty(ref _weather, value);
        }

        private IClassManager _classManager;
        public IClassManager ClassManager
        {
            get => _classManager;
            internal set => SetProperty(ref _classManager, value);
        }

        private ISessionResult _current;
        public ISessionResult Current
        {
            get => _current;
            internal set
            {
                if (_current is SessionResult prev) prev.IsCurrent = false;

                if (SetProperty(ref _current, value))
                    ApiProvider.Sim.RaiseSessionChangedEvent(value);

                if (_current is SessionResult cur) cur.IsCurrent = true;
            }
        }

        private ISessionResult _selected;
        public ISessionResult Selected
        {
            get => _selected;
            set
            {
                if (_selected is SessionResult prev) prev.IsSelected = false;

                SetProperty(ref _selected, value);

                if (_selected is SessionResult cur) cur.IsSelected = true;
            }
        }

        internal ObservableCollection<ISessionResult> SessionResultsInt;
        public ReadOnlyObservableCollection<ISessionResult> SessionResults { get; }

        internal ObservableCollection<IEntity> EntitiesInt;
        public ReadOnlyObservableCollection<IEntity> Entities { get; }

        internal ObservableCollection<ISessionEvent> SessionEventsInt;
        public ReadOnlyObservableCollection<ISessionEvent> SessionEvents { get; }

        private int _strengthOfField;
        public int StrengthOfField
        {
            get => _strengthOfField;
            internal set => SetProperty(ref _strengthOfField, value);
        }
        
        public ISessionResult GetPractice()
        {
            var p = SessionResults.FirstOrDefault(r => r.Type == SessionType.Practice);
            return p ?? SessionResults.FirstOrDefault(r => r.Type == SessionType.Warmup);
        }

        public ISessionResult GetQualification()
        {
            return SessionResults.FirstOrDefault(r => r.Type == SessionType.Qualify);
        }

        public ISessionResult GetLatestRace()
        {
            return SessionResults.FirstOrDefault(r => r.Type == SessionType.Race && r.HasStarted && !r.HasFinished);
        }

        public ReadOnlyObservableCollection<ISessionResult> GetRaceSessions()
        {
            return new ReadOnlyObservableCollection<ISessionResult>(
                new ObservableCollection<ISessionResult>(SessionResults.Where(r => r.Type == SessionType.Race)));
        }

        internal Session()
        {
            _track = new Track();
            _weather = new Weather();

            SessionResultsInt = new ObservableCollection<ISessionResult>();
            SessionResults = new ReadOnlyObservableCollection<ISessionResult>(SessionResultsInt);

            EntitiesInt = new ObservableCollection<IEntity>();
            Entities = new ReadOnlyObservableCollection<IEntity>(EntitiesInt);

            SessionEventsInt = new ObservableCollection<ISessionEvent>();
            SessionEvents = new ReadOnlyObservableCollection<ISessionEvent>(SessionEventsInt);
            
            _classManager = new ClassManager();
        }

//        public ISessionResult GetSession(SessionType type)
//        {
//            return SessionResults.FirstOrDefault(r => r.Type == type);
//        }
//
//        public int GetSessionIndex(SessionType type)
//        {
//            return SessionResults.IndexOf(GetSession(type));
//        }
    }
}
