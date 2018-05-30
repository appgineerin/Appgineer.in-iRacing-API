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

namespace AiRAPI.Data.Results
{
    public interface ISessionQualifyPosition
    {
        int Position { get; } // ZERO BASED!
        int ClassPosition { get; } // ZERO BASED!
        byte CarIdx { get; }
        int FastestLap { get; }
        float FastestTime { get; }
    }
}
