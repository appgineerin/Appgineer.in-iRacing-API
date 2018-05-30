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
using System.Globalization;
using AiRAPI.Data.Enums;
using AiRAPI.Impl.Location;
using AiRAPI.Impl.Session;
using AiRAPI.Impl.Utils;
using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Updater.Parsers
{
    internal sealed class WeekendInfoParser : Parser
    {
        internal override void Parse(YamlMappingNode root, Simulation sim)
        {
            var weekendInfo = root.GetMap("WeekendInfo");
            var session = (Session.Session)sim.Session;
            
            // Nick: session info first, required for numjokerlaps in weekend options
            if (session.Info == null)
                ParseSessionInfo(weekendInfo, session);

            if (session.Options == null)
                ParseWeekendOptions(weekendInfo.GetMap("WeekendOptions"), session);

            ParseWeather(weekendInfo, (Weather)sim.Session.Weather);

            var trackLengthStr = weekendInfo.GetString("TrackLength");
            var trackLength = float.Parse(trackLengthStr.Substring(0, trackLengthStr.IndexOf(' ')), CultureInfo.InvariantCulture) * 1000;
            if (Math.Abs(trackLength - sim.Session.Track.Length) > 10E-6)
                ParseTrack(weekendInfo, session, trackLength);
        }

        private static void ParseWeekendOptions(YamlMappingNode weekendOptions, Session.Session session)
        {
            session.Options = new SessionOptions
            {
                NumStarters = weekendOptions.GetInt("NumStarters"),
                StartingGrid = weekendOptions.GetString("StartingGrid"),
                QualifyingScoring = weekendOptions.GetString("QualifyScoring"),
                CourseCautions = weekendOptions.GetString("CourseCautions"),
                StartingType = (RaceStartingType)weekendOptions.GetInt("StandingStart"),
                Restarts = weekendOptions.GetString("Restarts"),
                WeatherType = weekendOptions.GetString("WeatherType"),
                Skies = weekendOptions.GetString("Skies"),
                WindDirection = weekendOptions.GetString("WindDirection"),
                WindSpeed = weekendOptions.GetString("WindSpeed"),
                WeatherTemp = weekendOptions.GetString("WeatherTemp"),
                RelativeHumidity = weekendOptions.GetString("RelativeHumidity"),
                FogLevel = weekendOptions.GetString("FogLevel"),
                IsOfficial = !weekendOptions.GetBool("Unofficial"),
                CommercialMode = weekendOptions.GetString("CommercialMode"),
                IsNightSession = weekendOptions.GetBool("NightMode"),
                IsFixedSetup = weekendOptions.GetBool("IsFixedSetup"),
                StrictLapsChecking = weekendOptions.GetString("StrictLapsChecking"),
                HasOpenRegistration = weekendOptions.GetBool("HasOpenRegistration"),
                HardcoreLevel = weekendOptions.GetInt("HardcoreLevel")
            };

            ((SessionInfo)session.Info).NumJokerLaps = weekendOptions.GetInt("NumJokerLaps");
        }

        private static void ParseWeather(YamlMappingNode weekendInfo, Weather weather)
        {
            weather.WeatherType = weekendInfo.GetString("TrackWeatherType");
            weather.Skies = weekendInfo.GetString("TrackSkies");
            weather.TrackTemp = weekendInfo.GetString("TrackSurfaceTemp");
            weather.AirTemp = weekendInfo.GetString("TrackAirTemp");
            weather.AirPressure = weekendInfo.GetString("TrackAirPressure");
            weather.WindSpeed = weekendInfo.GetString("TrackWindVel");
            weather.WindDirection = weekendInfo.GetString("TrackWindDir");
            weather.RelativeHumidity = weekendInfo.GetString("TrackRelativeHumidity");
            weather.FogLevel = weekendInfo.GetString("TrackFogLevel");
        }

        private static void ParseTrack(YamlMappingNode weekendInfo, Session.Session session, float trackLength)
        {
            session.Track = new Track
            {
                Name = weekendInfo.GetString("TrackName"),
                Id = weekendInfo.GetInt("TrackID"),
                Length = trackLength,
                DisplayName = weekendInfo.GetString("TrackDisplayName"),
                DisplayShortName = weekendInfo.GetString("TrackDisplayShortName"),
                ConfigName = weekendInfo.GetString("TrackConfigName"),
                City = weekendInfo.GetString("TrackCity"),
                Country = weekendInfo.GetString("TrackCountry"),
                Altitude = weekendInfo.GetString("TrackAltitude"),
                Latitude = weekendInfo.GetString("TrackLatitude"),
                Longitude = weekendInfo.GetString("TrackLongitude"),
                Turns = weekendInfo.GetInt("TrackNumTurns"),
                PitSpeedLimit = weekendInfo.GetString("TrackPitSpeedLimit"),
                Type = weekendInfo.GetString("TrackType"),
                HasTrackCleanup = weekendInfo.GetBool("TrackCleanup"),
                IsDynamicTrack = weekendInfo.GetBool("TrackDynamicTrack"),
                NorthOffset = ParseFloat(weekendInfo.GetString("TrackNorthOffset"))
            };
        }

        private static void ParseSessionInfo(YamlMappingNode weekendInfo, Session.Session session)
        {
            session.Info = new SessionInfo
            {
                SeriesId = weekendInfo.GetInt("SeriesID"),
                SeasonId = weekendInfo.GetInt("SeasonID"),
                SessionId = weekendInfo.GetInt("SessionID"),
                SubSessionId = weekendInfo.GetInt("SubSessionID"),
                LeagueId = weekendInfo.GetInt("LeagueID"),
                IsOfficial = weekendInfo.GetBool("Official"),
                RaceWeek = weekendInfo.GetInt("RaceWeek"),
                EventType = weekendInfo.GetString("EventType"),
                Category = weekendInfo.GetString("Category"),
                SimMode = weekendInfo.GetString("SimMode"),
                IsTeamRacing = weekendInfo.GetBool("TeamRacing"),
                MinDrivers = weekendInfo.GetInt("MinDrivers"),
                MaxDrivers = weekendInfo.GetInt("MaxDrivers"),
                DCRuleSet = weekendInfo.GetString("DCRuleSet"),
                QualifierMustStartRace = weekendInfo.GetBool("QualifierMustStartRace"),
                NumCarClasses = weekendInfo.GetInt("NumCarClasses"),
                NumCarTyps = weekendInfo.GetInt("NumCarTypes"),
                IsHeatRacing = weekendInfo.GetBool("HeatRacing"),
                //NumAdvanceHeat = weekendInfo.GetInt("XXXX"), TODO: Enable once this setting is included in session info
                //NumAdvanceConsolation = weekendInfo.GetInt("XXXX"), TODO: Enable once this setting is included in session info
                //IsConsolationStacked = weekendInfo.GetBool("XXXX") TODO: Enable once this setting is included in session info
            };
        }

        private static float ParseFloat(string s)
        {
            return float.Parse(s.Substring(0, s.IndexOf(' ')), CultureInfo.InvariantCulture);
        }
    }
}
