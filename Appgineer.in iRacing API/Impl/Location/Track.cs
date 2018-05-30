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
using AiRAPI.Data.Location;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Location
{
    internal sealed class Track : BindableBase, ITrack
    {
        private string _name;
        public string Name
        {
            get => _name;
            internal set => SetProperty(ref _name, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            internal set => SetProperty(ref _id, value);
        }

        private float _length;
        public float Length
        {
            get => _length;
            internal set => SetProperty(ref _length, value);
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            internal set => SetProperty(ref _displayName, value);
        }

        private string _displayShortName;
        public string DisplayShortName
        {
            get => _displayShortName;
            internal set => SetProperty(ref _displayShortName, value);
        }

        private string _configName;
        public string ConfigName
        {
            get => _configName;
            internal set => SetProperty(ref _configName, value);
        }

        private string _city;
        public string City
        {
            get => _city;
            internal set => SetProperty(ref _city, value);
        }

        private string _country;
        public string Country
        {
            get => _country;
            internal set => SetProperty(ref _country, value);
        }

        private string _altitude;
        public string Altitude
        {
            get => _altitude;
            internal set => SetProperty(ref _altitude, value);
        }

        private string _latitude;
        public string Latitude
        {
            get => _latitude;
            internal set => SetProperty(ref _latitude, value);
        }

        private string _longitude;
        public string Longitude
        {
            get => _longitude;
            internal set => SetProperty(ref _longitude, value);
        }

        private float _northOffset;
        public float NorthOffset
        {
            get => _northOffset;
            internal set => SetProperty(ref _northOffset, value);
        }

        private int _turns;
        public int Turns
        {
            get => _turns;
            internal set => SetProperty(ref _turns, value);
        }

        private string _pitSpeedLimit;
        public string PitSpeedLimit
        {
            get => _pitSpeedLimit;
            internal set => SetProperty(ref _pitSpeedLimit, value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            internal set => SetProperty(ref _type, value);
        }

        private bool _hasTrackCleanup;
        public bool HasTrackCleanup
        {
            get => _hasTrackCleanup;
            internal set => SetProperty(ref _hasTrackCleanup, value);
        }

        private bool _isDynamicTrack;
        public bool IsDynamicTrack
        {
            get => _isDynamicTrack;
            internal set => SetProperty(ref _isDynamicTrack, value);
        }

        internal ObservableCollection<ISector> SectorsInt;
        private ReadOnlyObservableCollection<ISector> _sectors;
        public ReadOnlyObservableCollection<ISector> Sectors
        {
            get => _sectors;
            internal set => SetProperty(ref _sectors, value);
        }

        private ObservableCollection<ISector> _selectedSectors;
        public ObservableCollection<ISector> SelectedSectors
        {
            get => _selectedSectors;
            internal set => SetProperty(ref _selectedSectors, value);
        }

        internal Track()
        {
            SectorsInt = new ObservableCollection<ISector>();
            _sectors = new ReadOnlyObservableCollection<ISector>(SectorsInt);
            _selectedSectors = new ObservableCollection<ISector>();
        }
    }
}
