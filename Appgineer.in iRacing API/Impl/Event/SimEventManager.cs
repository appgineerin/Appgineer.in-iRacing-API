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
using System.Collections.Generic;
using System.Linq;
using AiRAPI.Data.Enums;
using AiRAPI.Event;

namespace AiRAPI.Impl.Event
{
    internal sealed class SimEventManager : ISimEventManager
    {
        private readonly Dictionary<EventType, List<object>> _eventHandlers;
        private readonly Simulation _sim;

        internal SimEventManager(Simulation sim)
        {
            _eventHandlers = new Dictionary<EventType, List<object>>();
            _sim = sim;
        }

        ~SimEventManager()
        {
            Dispose();
        }

        private void Dispose()
        {
            _eventHandlers.Clear();
        }

        public void RegisterEventHandler(EventType eventType, Action handler) => Add(eventType, handler);
        public void RegisterEventHandler(EventType eventType, Action<int> handler) => Add(eventType, handler);
        public bool RemoveHandler(Action handler) => Remove(handler);
        public bool RemoveHandler(Action<int> handler) => Remove(handler);
        public bool RemoveHandler(EventType eventType, Action handler) => Remove(eventType, handler);
        public bool RemoveHandler(EventType eventType, Action<int> handler) => Remove(eventType, handler);

        internal void ExecuteEvents()
        {
            while (_sim.Triggers.Count > 0)
            {
                var idx = -1;

                var obj = _sim.Triggers.Pop();
                EventType type;
                if (obj is TriggerInfo info)
                {
                    idx = info.CarIdx;
                    type = info.Type;
                }
                else
                {
                    type = (EventType)obj;
                }

                if (!_eventHandlers.ContainsKey(type))
                    continue;

                foreach (var handler in _eventHandlers[type])
                {
                    switch (handler)
                    {
                        case Action action:
                            action();
                            break;
                        case Action<int> intAction:
                            intAction(idx);
                            break;
                    }
                }
            }
        }

        private void Add(EventType eventType, object handler)
        {
            if (!_eventHandlers.ContainsKey(eventType))
                _eventHandlers.Add(eventType, new List<object>());

            _eventHandlers[eventType].Add(handler);
        }

        private bool Remove(object handler)
        {
            var eventTypes = Enum.GetValues(typeof(EventType)).Cast<EventType>();
            return eventTypes.All(t => Remove(t, handler));
        }

        private bool Remove(EventType eventType, object handler)
        {
            return _eventHandlers.ContainsKey(eventType) && _eventHandlers[eventType].Remove(handler);
        }
    }
}
