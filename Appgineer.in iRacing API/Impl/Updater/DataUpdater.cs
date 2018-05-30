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
using System.IO;
using System.Linq;
using AiRAPI.Impl.Calculators;
using AiRAPI.Impl.Camera;
using AiRAPI.Impl.Event;
using AiRAPI.Impl.Results;
using AiRAPI.Impl.Updater.Parsers;
using AiRAPI.Impl.Updater.Updater;
using AiRAPI.SDK;
using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Updater
{
    internal sealed class DataUpdater
    {
        internal double TimeOffset { get; set; }
        internal double CurrentTime { get; set; }
        internal double PrevTime { get; set; }
        internal int LastSessionInfoUpdate { get; set; }

        private readonly List<Parser> _parsers;
        private readonly SectorParser _sectorParser = new SectorParser();

        private readonly SessionTimeUpdater _sessionTimeUpdater;
        private readonly SessionStateUpdater _sessionStateUpdater;
        private readonly SessionFlagUpdater _sessionFlagUpdater;
        private readonly CameraUpdater _cameraUpdater;
        private readonly RadioUpdater _radioUpdater;
        private readonly EntityUpdater _entityUpdater;
        private readonly EntityResultsUpdater _entityResultsUpdater;

        internal DataUpdater()
        {
            _parsers = new List<Parser>
            {
                new WeekendInfoParser(),
                new DriverInfoParser(this),
                new SessionInfoParser(this),
                new QualiSessionParser(),
                new CameraParser()
            };

            _sessionTimeUpdater = new SessionTimeUpdater(this);
            _sessionStateUpdater = new SessionStateUpdater(this);
            _sessionFlagUpdater = new SessionFlagUpdater(this);
            _cameraUpdater = new CameraUpdater(this);
            _radioUpdater = new RadioUpdater(this);
            _entityUpdater = new EntityUpdater(this);
            _entityResultsUpdater = new EntityResultsUpdater(this);
        }

        internal bool UpdateData(ref iRacingSDK sdk, Simulation sim)
        {
            if (!sdk.IsInitialized)
                return false;

            if (!sdk.IsConnected())
            {
                sdk.Shutdown();
                return false;
            }

            if (sdk.GetData("SessionNum") == null)
            {
                sdk = new iRacingSDK();
                return true;
            }

            Update(sdk, sim);
            return true;
        }

        private void Update(iRacingSDK sdk, Simulation sim)
        {
            sim.DataMutex.WaitOne();

            var sessionInfoUpdate = sdk.Header.SessionInfoUpdate;
            var updateSessionInfo = false;
            if (sessionInfoUpdate != LastSessionInfoUpdate)
            {
                LastSessionInfoUpdate = sessionInfoUpdate;
                updateSessionInfo = true;
            }

            string sessionInfo = null;
            if (updateSessionInfo)
                sessionInfo = sdk.GetSessionInfo();

            try
            {
                Update(sim, sessionInfo, updateSessionInfo);
                ((SimEventManager) sim.EventManager).ExecuteEvents();
            }
            finally
            {
                sim.DataMutex.ReleaseMutex();
            }
        }

        private void Update(Simulation sim, string sessionInfo, bool updateSessionInfo)
        {
            ((Telemetry)sim.Telemetry).UpdateTelemetryData(sim.Sdk);

            var trackLength = sim.Session.Track.Length;

            if (updateSessionInfo)
            {
                var root = ParseYaml(sessionInfo);

                foreach (var parser in _parsers)
                    parser.Parse(root, sim);
            
                if (Math.Abs(trackLength - sim.Session.Track.Length) > 10E-6)
                {
                    lock (sim.SharedCollectionLock)
                    {
                        _sectorParser.Parse(root, sim);
                        sim.TimeDelta = new TimeDelta(sim.Session.Track.Length, 300, 64);
                    }
                }
            }

            if (CurrentTime >= PrevTime)
                CurrentTime = sim.Telemetry.SessionTime;
            else
                PrevTime = 0;

            ((Session.Session)sim.Session).Current = sim.Session.SessionResults.FirstOrDefault(r => r.Id == sim.Telemetry.SessionNum);
            ((CameraManager)sim.CameraManager).SetFollowedEntity(sim.Telemetry.CamCarIdx);
            (sim.CameraManager.FollowedEntity?.Results.CurrentResult as EntitySessionResult)?.AddAirTime(CurrentTime - PrevTime);

            if (CurrentTime <= PrevTime)
                return;

            var result = (SessionResult)sim.Session.Current;
            result.SessionTime = sim.Telemetry.SessionTime;
            if (sim.HideSimUI)
                sim.HideUi();

            _sessionTimeUpdater.Update(result, sim);
            if (sim.Telemetry.SessionTime - sim.Telemetry.ReplaySessionTime < 2)
                TimeOffset = sim.Telemetry.ReplayFrameNum - sim.Telemetry.SessionTime * 60;

            _sessionStateUpdater.Update(result, sim);
            _sessionFlagUpdater.Update(result, sim);
            _cameraUpdater.Update(sim);
            _radioUpdater.Update(sim);
            _entityUpdater.Update(result, sim);

            ((TimeDelta)sim.TimeDelta).Update(CurrentTime, sim.Telemetry.CarIdxLapDistPct);
            result.UpdatePosition(sim);
            result.CheckPitStatus();
            result.UpdateEntityLinks(sim);
            _entityResultsUpdater.Update(result, sim);
            PrevTime = CurrentTime;
        }

        private static YamlMappingNode ParseYaml(string yaml)
        {
            using (var reader = new StringReader(yaml))
            {
                var stream = new YamlStream();
                stream.Load(reader);
                return (YamlMappingNode) stream.Documents[0].RootNode;
            }
        }
    }
}
