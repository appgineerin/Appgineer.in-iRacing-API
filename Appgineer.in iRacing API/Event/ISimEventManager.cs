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
using AiRAPI.Data.Enums;

namespace AiRAPI.Event
{
    public interface ISimEventManager
    {
        void RegisterEventHandler(EventType eventType, Action handler);
        void RegisterEventHandler(EventType eventType, Action<int> handler);
        bool RemoveHandler(Action handler);
        bool RemoveHandler(Action<int> handler);
        bool RemoveHandler(EventType eventType, Action handler);
        bool RemoveHandler(EventType eventType, Action<int> handler);
    }
}
