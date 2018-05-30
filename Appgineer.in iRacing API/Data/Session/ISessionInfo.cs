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

using System.ComponentModel;

namespace AiRAPI.Data.Session
{
    public interface ISessionInfo : INotifyPropertyChanged
    {
        int SeriesId { get; }
        int SeasonId { get; }
        int SessionId { get; }
        int SubSessionId { get; }
        int LeagueId { get; }
        bool IsOfficial { get; }
        int RaceWeek { get; }
        string EventType { get; }
        string Category { get; }
        string SimMode { get; }
        bool IsTeamRacing { get; }
        int MinDrivers { get; }
        int MaxDrivers { get; }
        string DCRuleSet { get; }
        bool QualifierMustStartRace { get; }
        int NumCarClasses { get; }
        int NumCarTyps { get; }
        bool IsHeatRacing { get; }
        bool IsConsolationStacked { get; }
        int NumAdvanceHeat { get; }
        int NumAdvanceConsolation { get; }
        int NumJokerLaps { get; }
    }
}
