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

using System.Linq;
using AiRAPI.Data.Enums;
using AiRAPI.Impl.Results;
using AiRAPI.Impl.Utils;
using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Updater.Parsers
{
    internal sealed class QualiSessionParser : Parser
    {
        internal override void Parse(YamlMappingNode root, Simulation sim)
        {
            if (sim.Session.GetQualification() is SessionResult quali && quali.Results.Count > 0)
                return;

            var results = root.GetList("QualifyResultsInfo.Results");
            if (results?.Children == null || results.Children.Count == 0)
                return;
            
            var session = new SessionResult
            {
                Type = SessionType.Qualify,
                Name = "QUALIFY",
                FastestLapTime = float.MaxValue
            };

            foreach (var result in results.Children.OfType<YamlMappingNode>())
                CreateResult(ref session, result, sim);
            
            lock (sim.SharedCollectionLock)
            {
                ((Session.Session)sim.Session).SessionResultsInt.Add(session);
            }
        }

        private static void CreateResult(ref SessionResult session, YamlMappingNode result, ISimulation sim)
        {
            var carIdx = result.GetByte("CarIdx");
            var entity = sim.Session.Entities.FirstOrDefault(e => e.CarIdx == carIdx);
            if (entity == null)
                return;

            var sessionResult = new EntitySessionResult(session, entity);

            var pos = result.GetInt("Position") + 1;
            sessionResult.Position = pos > 0 ? pos : int.MaxValue;
            if (sessionResult.Position == 1)
                session.Leader = entity;

            sessionResult.ClassPosition = result.GetInt("ClassPosition") + 1;
            sessionResult.FastestLapTime = result.GetFloat("FastestTime");

            session.ResultsInt.Add(sessionResult);
            if (sessionResult.FastestLapTime < session.FastestLapTime && sessionResult.FastestLapTime > 0)
                session.FastestLapTime = sessionResult.FastestLapTime;
        }
    }
}
