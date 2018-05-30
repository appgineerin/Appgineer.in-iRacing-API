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

using AiRAPI.Data.Lap;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Lap
{
    internal abstract class Sector : BindableBase, ISector
    {
        public abstract bool IsCompleted { get; }
        public abstract float Time { get; protected set; }

        internal bool IsUnknownSectorTime { get; set; }

        private int _index;
        public int Index
        {
            get => _index;
            internal set => SetProperty(ref _index, value);
        }

        private int _replayPosition;
        public int ReplayPosition
        {
            get => _replayPosition;
            internal set => SetProperty(ref _replayPosition, value);
        }
    }
}
