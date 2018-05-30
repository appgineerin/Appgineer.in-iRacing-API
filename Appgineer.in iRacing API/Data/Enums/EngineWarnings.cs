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

namespace AiRAPI.Data.Enums
{
    [Flags]
    public enum EngineWarnings
    {
        WaterTempWarning = 0x01,
        FuelPressureWarning = 0x02,
        OilPressureWarning = 0x04,
        EngineStalled = 0x08,
        PitSpeedLimiter = 0x10,
        RevLimitActive = 0x20
    }
}
