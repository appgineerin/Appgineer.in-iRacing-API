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

using AiRAPI.Data.Location;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Location
{
    internal sealed class Weather : BindableBase, IWeather
    {
        private string _weatherType;
        public string WeatherType
        {
            get => _weatherType;
            internal set => SetProperty(ref _weatherType, value);
        }

        private string _skies;
        public string Skies
        {
            get => _skies;
            internal set => SetProperty(ref _skies, value);
        }

        private string _trackTemp;
        public string TrackTemp
        {
            get => _trackTemp;
            internal set => SetProperty(ref _trackTemp, value);
        }

        private string _airTemp;
        public string AirTemp
        {
            get => _airTemp;
            internal set => SetProperty(ref _airTemp, value);
        }

        private string _airPressure;
        public string AirPressure
        {
            get => _airPressure;
            internal set => SetProperty(ref _airPressure, value);
        }

        private string _windSpeed;
        public string WindSpeed
        {
            get => _windSpeed;
            internal set => SetProperty(ref _windSpeed, value);
        }

        private string _windDirection;
        public string WindDirection
        {
            get => _windDirection;
            internal set => SetProperty(ref _windDirection, value);
        }

        private string _relativeHumidity;
        public string RelativeHumidity
        {
            get => _relativeHumidity;
            internal set => SetProperty(ref _relativeHumidity, value);
        }

        private string _fogLevel;
        public string FogLevel
        {
            get => _fogLevel;
            internal set => SetProperty(ref _fogLevel, value);
        }
    }
}
