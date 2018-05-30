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

using System.Windows.Media;
using System.Windows.Media.Imaging;
using AiRAPI.Data.Entity;

namespace AiRAPI.Impl.Entity
{
    internal sealed class Driver : Entity, IDriver
    {
        private string _initials;
        public string Initials
        {
            get => _initials;
            internal set => SetProperty(ref _initials, value);
        }

        private string _shortName;
        public string ShortName
        {
            get => _shortName;
            internal set => SetProperty(ref _shortName, value);
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            internal set => SetProperty(ref _lastName, value);
        }

        private string _threeLetterCode;
        public string ThreeLetterCode
        {
            get => _threeLetterCode;
            internal set => SetProperty(ref _threeLetterCode, value);
        }

        private bool _isDriving;
        public bool IsDriving
        {
            get => _isDriving;
            internal set => SetProperty(ref _isDriving, value);
        }

        private IClub _club;
        public IClub Club
        {
            get => _club;
            internal set => SetProperty(ref _club, value);
        }

        private ILicense _license;
        public ILicense License
        {
            get => _license;
            internal set => SetProperty(ref _license, value);
        }

        private string _division;
        public string Division
        {
            get => _division;
            internal set => SetProperty(ref _division, value);
        }

        private BitmapSource _helmet;
        public BitmapSource Helmet
        {
            get => _helmet;
            internal set => SetProperty(ref _helmet, value);
        }

        private int _helmetDesignNumber;
        public int HelmetDesignNumber
        {
            get => _helmetDesignNumber;
            internal set => SetProperty(ref _helmetDesignNumber, value);
        }

        private Color _helmetColor1;
        public Color HelmetColor1
        {
            get => _helmetColor1;
            internal set => SetProperty(ref _helmetColor1, value);
        }

        private Color _helmetColor2;
        public Color HelmetColor2
        {
            get => _helmetColor2;
            internal set => SetProperty(ref _helmetColor2, value);
        }

        private Color _helmetColor3;
        public Color HelmetColor3
        {
            get => _helmetColor3;
            internal set => SetProperty(ref _helmetColor3, value);
        }

        public bool Equals(IDriver other)
        {
            return base.Equals(other);
        }
    }
}
