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
using System.ComponentModel;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Location;
using AiRAPI.Data.Results;

namespace AiRAPI.Data.Session
{
    public interface ISession : INotifyPropertyChanged
    {
        ISessionInfo Info { get; }
        ISessionOptions Options { get; }
        ITrack Track { get; }
        IWeather Weather { get; }
        IClassManager ClassManager { get; }
        ISessionResult Current { get; }
        ISessionResult Selected { get; set; }
        ReadOnlyObservableCollection<ISessionResult> SessionResults { get; }
        ReadOnlyObservableCollection<IEntity> Entities { get; }
        ReadOnlyObservableCollection<ISessionEvent> SessionEvents { get; }
        int StrengthOfField { get; }

        ISessionResult GetPractice();
        ISessionResult GetQualification();
        ISessionResult GetLatestRace();
        ReadOnlyObservableCollection<ISessionResult> GetRaceSessions();

//        ISessionResult GetSession(SessionType type);
//        int GetSessionIndex(SessionType type);
    }
}
