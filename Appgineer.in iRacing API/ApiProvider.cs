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

using AiRAPI.Impl;

namespace AiRAPI
{
    public static class ApiProvider
    {
        public static ISimulation Simulation => Sim;

        internal static Simulation Sim { get; } = new Simulation();
    }
}
