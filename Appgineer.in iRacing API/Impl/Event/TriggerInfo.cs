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

using AiRAPI.Data.Enums;

namespace AiRAPI.Impl.Event
{
    internal class TriggerInfo
    {
        internal int CarIdx { get; set; }
        internal EventType Type { get; set; }
    }
}
