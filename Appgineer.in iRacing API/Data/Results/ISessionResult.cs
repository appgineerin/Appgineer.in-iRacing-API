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
using AiRAPI.Calculators;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.Ordering;

namespace AiRAPI.Data.Results
{
    public interface ISessionResult : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<IEntitySessionResult> Results { get; }
        ReadOnlyObservableCollection<ISessionQualifyPosition> QualifyPositions { get; }

        IEntity Leader { get; }
        IEntity LiveLeader { get; }
        string Name { get; }
        SessionType Type { get; }
        SessionSubType SubType { get; }
        bool Skipped { get; }
        bool HasStarted { get; }
        bool HasFinished { get; }
        bool IsCurrent { get; }
        bool IsSelected { get; }
        bool RunGroupsUsed { get; }
        SessionState State { get; }
        SessionFlags Flags { get; }
        IDriver FastestLapDriver { get; }
        int FastestLapNumber { get; }
        float FastestLapTime { get; }
        int Id { get; }
        int LeadChanges { get; }
        int Cautions { get; }
        int CautionLaps { get; }
        double SessionLength { get; }
        double SessionTime { get; }
        double SessionTimeRemaining { get; }
        double SessionStartTime { get; }
        int LapsTotal { get; }
        int LapsCompleted { get; }
        int LapsRemaining { get; }
        int FinishLine { get; }
        int CurrentReplayPosition { get; }
        bool PitOccupied { get; }
        bool HasQualifyPositions { get; }

        bool HasEstimatedSessionLength { get; }
        bool HasEstimatedLapsTotal { get; }
        double EstimatedSessionLength { get; }
        double EstimatedSessionTimeRemaining { get; }
        int EstimatedLapsTotal { get; }
        int EstimatedLapsRemaining { get; }
        bool SessionLengthDecidedByLaps { get; }
        double AverageLaptime { get; }
        bool IsFinalLap { get; }

        IAverageLapCalculator AverageLapCalculator { get; }

        IEntitySessionResult GetClassLeader(string className);
        IEntitySessionResult GetLiveClassLeader(string className);
        IEntitySessionResult GetResult(int position, IDataOrder order);
        IEntitySessionResult GetResult(int position, IDataOrder order, string className);
            
        void EstimateSessionLength(double averageLeaderLaptimeSeconds);
        void EstimateLapsTotal(double averageLeaderLaptimeSeconds);
        double GetAverageLaptimePrediction(ISimulation sim);
    }
}
