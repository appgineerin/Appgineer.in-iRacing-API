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
using System.ComponentModel;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Lap;

namespace AiRAPI.Data.Results
{
    public interface IEntitySessionResult : INotifyPropertyChanged, IComparable<IEntitySessionResult>
    {
        IEntity Entity { get; }
        ISessionResult Session { get; }
        int Position { get; }
        int LivePosition { get; }
        int HighestPosition { get; }
        int LowestPosition { get; }
        int StartPosition { get; }
        int ClassPosition { get; }
        int LiveClassPosition { get; }
        int ClassHighestPosition { get; }
        int ClassLowestPosition { get; }
        int ClassStartPosition { get; }
        int LapsLed { get; }
        int ClassLapsLed { get; }
        float Gap { get; }
        float LiveGap { get; }
        int GapLaps { get; }
        int LiveGapLaps { get; }
        float ClassGap { get; }
        float LiveClassGap { get; }
        int ClassGapLaps { get; }
        int LiveClassGapLaps { get; }
        float Interval { get; }
        float LiveInterval { get; }
        int IntervalLaps { get; }
        int LiveIntervalLaps { get; }
        float IntervalBehind { get; }
        float LiveIntervalBehind { get; }
        int IntervalLapsBehind { get; }
        int LiveIntervalLapsBehind { get; }
        IEntitySessionResult Ahead { get; }
        IEntitySessionResult LiveAhead { get; }
        IEntitySessionResult Behind { get; }
        IEntitySessionResult LiveBehind { get; }
        float FastestLapTime { get; }
        float LastLapTime { get; }
        double CurrentLapTime { get; }
        ILap CurrentLap { get; }
        ILap PreviousLap { get; }
        ILap FastestLap { get; }
        ReadOnlyObservableCollection<ILap> Laps { get; }
        ReadOnlyDictionary<int, ILap> LapsMap { get; }
        double CurrentTrackPct { get; }
        int Stint { get; }
        int PitStopCount { get; }
        IPitStop CurrentPitStop { get; }
        ReadOnlyObservableCollection<IPitStop> PitStops { get; }
        bool Finished { get; }
        bool Out { get; }
        bool DidNotStart { get; }
        int Incidents { get; }
        TimeSpan AirTime { get; set; }
        string ReasonOutString { get; }
        double InvisibleSince { get; }
        double? PitLaneEntryTime { get; }
        double? PitStopStartTime { get; }
        double? PitStopEndTime { get; }
        double? PitLaneExitTime { get; }
        bool IsAdvancing { get; }
        int JokerLapsCompleted { get; }
    }
}
