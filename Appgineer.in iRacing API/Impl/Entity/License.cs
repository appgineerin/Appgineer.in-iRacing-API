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

using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using AiRAPI.Data.Entity;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Entity
{
    internal sealed class License : BindableBase, ILicense
    {
        private readonly Color _licenseColor;
        public Color LicenseColor => Level.BackgroundOverride ?? _licenseColor;
        public Color TextColor => Level.TextColor;
        public float SafetyRating { get; }
        public int IRating { get; }
        public int Order { get; }
        
        private LicenseLevel Level { get; }

        internal License(int level, int subLevel, Color color, int iRating)
        {
            SafetyRating = subLevel / 100F;
            Level = GetLevel(level);
            Order = (int)Level.Level * 1000 + subLevel;
            _licenseColor = color;
            IRating = iRating;
        }

        public string Display
        {
            get
            {
                var level = Level.Level == LicenseLevel.Licenses.Unknown ? "?" : Level.Display;
                return $"{level} {SafetyRating:0.00}";
            }
        }

        public override string ToString()
        {
            return Display;
        }

        private static LicenseLevel GetLevel(int level)
        {
            return LicenseLevel.FromLevel(level);
        }

        internal abstract class LicenseLevel
        {
            protected LicenseLevel(Licenses level, int lowRange, int highRange, Color? textColor = null, string display = null)
            {
                Level = level;
                Name = level.ToString();
                Display = string.IsNullOrWhiteSpace(display) ? Name.Substring(0, 1) : display;
                LowRange = lowRange;
                HighRange = highRange;
                TextColor = textColor ?? Colors.White;
                BackgroundOverride = null;
            }

            internal Licenses Level { get; }
            private string Name { get; }
            internal string Display { get; }
            private int LowRange { get; }
            private int HighRange { get; }
            internal Color TextColor { get; }
            internal Color? BackgroundOverride { get; set; }

            internal enum Licenses
            {
                R = 0,
                D,
                C,
                B,
                A,
                P,
                Wc,
                Unknown
            }

            private static readonly List<LicenseLevel> LicenseLevels;

            static LicenseLevel()
            {
                LicenseLevels = new List<LicenseLevel>(new LicenseLevel[]
                {
                    new LicenseRookie(),
                    new LicenseD(),
                    new LicenseC(),
                    new LicenseB(),
                    new LicenseA(),
                    new LicensePro(),
                    new LicenseProWc(),
                    new LicenseUnknown()
                });
            }

            internal static LicenseLevel FromLevel(int level)
            {
                var license = LicenseLevels.SingleOrDefault(l => l.LowRange <= level && l.HighRange >= level);
                return license ?? LicenseLevels.Last();
            }
        }

        private class LicenseRookie : LicenseLevel
        {
            internal LicenseRookie() : base(Licenses.R, 0, 4) { }
        }

        private class LicenseD : LicenseLevel
        {
            internal LicenseD() : base(Licenses.D, 5, 8, Colors.Black) { }
        }

        private class LicenseC : LicenseLevel
        {
            internal LicenseC() : base(Licenses.C, 9, 12, Colors.Black) { }
        }

        private class LicenseB : LicenseLevel
        {
            internal LicenseB() : base(Licenses.B, 13, 16) { }
        }

        private class LicenseA : LicenseLevel
        {
            internal LicenseA() : base(Licenses.A, 17, 20) { }
        }

        private class LicensePro : LicenseLevel
        {
            internal LicensePro() : base(Licenses.P, 21, 24, Colors.White)
            {
                BackgroundOverride = Colors.Black;
            }
        }

        private class LicenseProWc : LicenseLevel
        {
            internal LicenseProWc() : base(Licenses.Wc, 25, 28, Colors.White, "WC") { }
        }

        private class LicenseUnknown : LicenseLevel
        {
            internal LicenseUnknown() : base(Licenses.Unknown, -1, -1, Colors.Black, "?")
            {
                BackgroundOverride = Colors.DarkGray;
            }
        }
    }
}
