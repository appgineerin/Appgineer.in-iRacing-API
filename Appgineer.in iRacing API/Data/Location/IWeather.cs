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

using System.ComponentModel;

namespace AiRAPI.Data.Location
{
    public interface IWeather : INotifyPropertyChanged
    {
        string WeatherType { get; }
        string Skies { get; }
        string TrackTemp { get; }
        string AirTemp { get; }
        string AirPressure { get; }
        string WindSpeed { get; }
        string WindDirection { get; }
        string RelativeHumidity { get; }
        string FogLevel { get; }
    }
}
