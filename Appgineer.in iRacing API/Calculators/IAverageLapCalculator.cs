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

namespace AiRAPI.Calculators
{
    public interface IAverageLapCalculator
    {
        int TrackId { get; }
        double TrackLength { get; }
        void Update(ISessionResult session, ISimulation sim);
        double Calculate();
    }
}
