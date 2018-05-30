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

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AiRAPI.Data.Location
{
    public interface ITrack : INotifyPropertyChanged
    {
        string Name { get; }
        int Id { get; }
        float Length { get; }
        string DisplayName { get; }
        string DisplayShortName { get; }
        string ConfigName { get; }
        string City { get; }
        string Country { get; }
        string Altitude { get; }
        string Latitude { get; }
        string Longitude { get; }
        float NorthOffset { get; }
        int Turns { get; }
        string PitSpeedLimit { get; }
        string Type { get; }
        bool HasTrackCleanup { get; }
        bool IsDynamicTrack { get; }

        ReadOnlyObservableCollection<ISector> Sectors { get; }
        ObservableCollection<ISector> SelectedSectors { get; }
    }
}
