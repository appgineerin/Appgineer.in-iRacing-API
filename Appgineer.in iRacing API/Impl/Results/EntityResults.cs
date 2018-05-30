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
using AiRAPI.Data.Results;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Results
{
    internal sealed class EntityResults : BindableBase, IEntityResults
    {
        internal readonly ObservableCollection<IEntitySessionResult> SessionResultsInt;
        public ReadOnlyObservableCollection<IEntitySessionResult> SessionResults { get; }

        private IEntitySessionResult _currentResult;
        public IEntitySessionResult CurrentResult
        {
            get => _currentResult;
            internal set => SetProperty(ref _currentResult, value);
        }

        private IEntity _entity;
        public IEntity Entity
        {
            get => _entity;
            internal set => SetProperty(ref _entity, value);
        }

        internal EntityResults()
        {
            SessionResultsInt = new ObservableCollection<IEntitySessionResult>();
            SessionResults = new ReadOnlyObservableCollection<IEntitySessionResult>(SessionResultsInt);
        }

        public bool HasResult(int sessionNumber)
        {
            return SessionResultsInt.Any(r => r.Session.Id == sessionNumber);
        }

        public bool HasResult(SessionType sessionType)
        {
            return SessionResultsInt.Any(r => r.Session.Type == sessionType);
        }

        public IEntitySessionResult GetResult(int sessionNumber)
        {
            return SessionResultsInt.FirstOrDefault(r => r.Session.Id == sessionNumber);
        }
        
        public IEntitySessionResult GetResult(SessionType sessionType)
        {
            return SessionResultsInt.FirstOrDefault(r => r.Session.Type == sessionType);
        }

        public IEntitySessionResult this[int sessionNumber] => GetResult(sessionNumber);

        public IEntitySessionResult this[SessionType sessionType] => GetResult(sessionType);
    }
}
