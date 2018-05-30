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
using AiRAPI.Data.Session;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Session
{
    internal sealed class SessionOptions : BindableBase, ISessionOptions
    {
        private int _numStarters;
        public int NumStarters
        {
            get => _numStarters;
            set => SetProperty(ref _numStarters, value);
        }

        private string _startingGrid;
        public string StartingGrid
        {
            get => _startingGrid;
            set => SetProperty(ref _startingGrid, value);
        }

        private string _qualifyingScoring;
        public string QualifyingScoring
        {
            get => _qualifyingScoring;
            set => SetProperty(ref _qualifyingScoring, value);
        }

        private string _courseCautions;
        public string CourseCautions
        {
            get => _courseCautions;
            set => SetProperty(ref _courseCautions, value);
        }

        private RaceStartingType _startingType;
        public RaceStartingType StartingType
        {
            get => _startingType;
            set => SetProperty(ref _startingType, value);
        }

        private string _restarts;
        public string Restarts
        {
            get => _restarts;
            set => SetProperty(ref _restarts, value);
        }

        private string _weatherType;
        public string WeatherType
        {
            get => _weatherType;
            set => SetProperty(ref _weatherType, value);
        }

        private string _skies;
        public string Skies
        {
            get => _skies;
            set => SetProperty(ref _skies, value);
        }

        private string _windDirection;
        public string WindDirection
        {
            get => _windDirection;
            set => SetProperty(ref _windDirection, value);
        }

        private string _windSpeed;
        public string WindSpeed
        {
            get => _windSpeed;
            set => SetProperty(ref _windSpeed, value);
        }

        private string _weatherTemp;
        public string WeatherTemp
        {
            get => _weatherTemp;
            set => SetProperty(ref _weatherTemp, value);
        }

        private string _relativeHumidity;
        public string RelativeHumidity
        {
            get => _relativeHumidity;
            set => SetProperty(ref _relativeHumidity, value);
        }

        private string _fogLevel;
        public string FogLevel
        {
            get => _fogLevel;
            set => SetProperty(ref _fogLevel, value);
        }

        private bool _isOfficial;
        public bool IsOfficial
        {
            get => _isOfficial;
            set => SetProperty(ref _isOfficial, value);
        }

        private string _commercialMode;
        public string CommercialMode
        {
            get => _commercialMode;
            set => SetProperty(ref _commercialMode, value);
        }

        private bool _isNightSession;
        public bool IsNightSession
        {
            get => _isNightSession;
            set => SetProperty(ref _isNightSession, value);
        }

        private bool _isFixedSetup;
        public bool IsFixedSetup
        {
            get => _isFixedSetup;
            set => SetProperty(ref _isFixedSetup, value);
        }

        private string _strictLapsChecking;
        public string StrictLapsChecking
        {
            get => _strictLapsChecking;
            set => SetProperty(ref _strictLapsChecking, value);
        }

        private bool _hasOpenRegistration;
        public bool HasOpenRegistration
        {
            get => _hasOpenRegistration;
            set => SetProperty(ref _hasOpenRegistration, value);
        }

        private int _hardcoreLevel;
        public int HardcoreLevel
        {
            get => _hardcoreLevel;
            set => SetProperty(ref _hardcoreLevel, value);
        }

    }
}
