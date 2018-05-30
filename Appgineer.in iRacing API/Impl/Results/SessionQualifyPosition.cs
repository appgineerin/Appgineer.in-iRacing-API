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

using AiRAPI.Data.Results;

namespace AiRAPI.Impl.Results
{
    internal class SessionQualifyPosition : ISessionQualifyPosition
    {
        private int _position;
        public int Position
        {
            get { return _position; }
            internal set { _position = value; }
        }

        private int _classPosition;
        public int ClassPosition
        {
            get { return _classPosition; }
            internal set { _classPosition = value; }
        }

        private byte _carIdx;
        public byte CarIdx
        {
            get { return _carIdx; }
            internal set { _carIdx = value; }
        }

        private int _fastestLap;
        public int FastestLap
        {
            get { return _fastestLap; }
            internal set { _fastestLap = value; }
        }

        private float _fastestTime;
        public float FastestTime
        { 
            get { return _fastestTime; }
            internal set { _fastestTime = value; }
        }
    }
}
