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
using AiRAPI.Data.Lap;
using AiRAPI.Data.Results;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Lap
{
    internal abstract class Lap : BindableBase, ILap
    {
        public IEntitySessionResult Result { get; }

        public abstract bool IsCompleted { get; }

        private int _number;
        public int Number
        {
            get => _number;
            internal set => SetProperty(ref _number, value);
        }

        private int _position;
        public int Position
        {
            get => _position;
            internal set => SetProperty(ref _position, value);
        }

        private int _classPosition;
        public int ClassPosition
        {
            get => _classPosition;
            internal set => SetProperty(ref _classPosition, value);
        }
        
        public abstract float Time { get; protected set; }

        private float _gap;
        public float Gap
        {
            get => _gap;
            internal set => SetProperty(ref _gap, value);
        }

        private int _gapLaps;
        public int GapLaps
        {
            get => _gapLaps;
            internal set => SetProperty(ref _gapLaps, value);
        }

        private int _replayPosition;
        public int ReplayPosition
        {
            get => _replayPosition;
            internal set => SetProperty(ref _replayPosition, value);
        }

        private bool _wasOnPitRoad;
        public bool WasOnPitRoad
        {
            get => _wasOnPitRoad;
            set => SetProperty(ref _wasOnPitRoad, value);
        }

        private bool _wasUnderCaution;
        public bool WasUnderCaution
        {
            get => _wasUnderCaution;
            set => SetProperty(ref _wasUnderCaution, value);
        }

        private bool _isJokerLap;
        public bool IsJokerLap
        {
            get => _isJokerLap;
            internal set => SetProperty(ref _isJokerLap, value);
        }

        internal readonly ObservableCollection<ISector> SectorsInt;
        public ReadOnlyObservableCollection<ISector> Sectors { get; }

        internal Lap(IEntitySessionResult result)
        {
            Result = result;
            SectorsInt = new ObservableCollection<ISector>();
            Sectors = new ReadOnlyObservableCollection<ISector>(SectorsInt);
        }
    }
}
