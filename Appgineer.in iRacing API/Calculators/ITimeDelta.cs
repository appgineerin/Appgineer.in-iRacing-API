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

namespace AiRAPI.Calculators
{
    public interface ITimeDelta
    {
        TimeSpan GetDelta(int caridx1, int caridx2);
    }
}
