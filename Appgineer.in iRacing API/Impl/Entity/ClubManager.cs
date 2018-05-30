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
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using AiRAPI.Data.Entity;

namespace AiRAPI.Impl.Entity
{
    internal static class ClubManager
    {
        private static readonly Dictionary<string, Club> Clubs = new Dictionary<string, Club>
        {
            { "-none-", new Club("-none-", null, null) },
            { "Asia", new Club("Asia", null, null) },
            { "Atlantic", new Club("Atlantic", Load(Properties.Resources.atlanticL), Load(Properties.Resources.atlanticS)) },
            { "Australia/NZ", new Club("Australia/NZ", Load(Properties.Resources.AusNzL), Load(Properties.Resources.AusNzS)) },
            { "Benelux", new Club("Benelux", Load(Properties.Resources.BeneluxL), Load(Properties.Resources.BeneluxS)) },
            { "Brazil", new Club("Brazil", Load(Properties.Resources.BrasilL), Load(Properties.Resources.BrasilS)) },
            { "California", new Club("California", Load(Properties.Resources.CaliforniaL), Load(Properties.Resources.CaliforniaS)) },
            { "Canada", new Club("Canada", null, null) },
            { "Carolina", new Club("Carolina", Load(Properties.Resources.CarolinaL), Load(Properties.Resources.CarolinaS)) },
            { "Central-Eastern Europe", new Club("Central-Eastern Europe", Load(Properties.Resources.CenEastEuropeL), Load(Properties.Resources.CenEastEuropeS)) },
            { "DE-AT-CH", new Club("DE-AT-CH", Load(Properties.Resources.DeAtChL), Load(Properties.Resources.DeAtChS)) },
            { "Finland", new Club("Finland", Load(Properties.Resources.FinnlandL), Load(Properties.Resources.FinnlandS)) },
            { "Florida", new Club("Florida", Load(Properties.Resources.FloridaL), Load(Properties.Resources.FloridaS)) },
            { "France", new Club("France", Load(Properties.Resources.FranceL), Load(Properties.Resources.FranceS)) },
            { "Georgia", new Club("Georgia", Load(Properties.Resources.GeorgiaL), Load(Properties.Resources.GeorgiaS)) },
            { "Hispanoamérica", new Club("Hispanoamérica", null, null) },
            { "Iberia", new Club("Iberia", Load(Properties.Resources.IberiaL), Load(Properties.Resources.IberiaS)) },
            { "Illinois", new Club("Illinois", Load(Properties.Resources.IllinoisL), Load(Properties.Resources.IllinoisS)) },
            { "Indiana", new Club("Indiana", Load(Properties.Resources.IndianaL), Load(Properties.Resources.IndianaS)) },
            { "International", new Club("International", Load(Properties.Resources.InternationalL), Load(Properties.Resources.InternationalS)) },
            { "Italy", new Club("Italy", Load(Properties.Resources.ItalyL), Load(Properties.Resources.ItalyS)) },
            { "Michigan", new Club("Michigan", Load(Properties.Resources.MichiganL), Load(Properties.Resources.MichiganS)) },
            { "Mid-South", new Club("Mid-South", Load(Properties.Resources.MidsouthL), Load(Properties.Resources.MidsouthS)) },
            { "Midwest", new Club("Midwest", Load(Properties.Resources.MidwestL), Load(Properties.Resources.MidwestS)) },
            { "New England", new Club("New England", Load(Properties.Resources.NewEnglandL), Load(Properties.Resources.NewEnglandS)) },
            { "New Jersey", new Club("New Jersey", Load(Properties.Resources.NewJerseyL), Load(Properties.Resources.NewJerseyS)) },
            { "New York", new Club("New York", Load(Properties.Resources.NewYorkL), Load(Properties.Resources.NewYorkS)) },
            { "Northwest", new Club("Northwest", Load(Properties.Resources.NorthwestL), Load(Properties.Resources.NorthwestS)) },
            { "Ohio", new Club("Ohio", Load(Properties.Resources.OhioL), Load(Properties.Resources.OhioS)) },
            { "Pennsylvania", new Club("Pennsylvania", Load(Properties.Resources.PennsylvaniaL), Load(Properties.Resources.PennsylvaniaS)) },
            { "Plains", new Club("Plains", Load(Properties.Resources.PlainsL), Load(Properties.Resources.PlainsS)) },
            { "Scandinavia", new Club("Scandinavia", Load(Properties.Resources.ScandinaviaL), Load(Properties.Resources.ScandinaviaS)) },
            { "South America", new Club("South America", Load(Properties.Resources.SouthAmericaL), Load(Properties.Resources.SouthAmericaS)) },
            { "Texas", new Club("Texas", Load(Properties.Resources.TexasL), Load(Properties.Resources.TexasS)) },
            { "UK and I", new Club("UK and I", null, null) },
            { "Virginias", new Club("Virginias", Load(Properties.Resources.VirginiasL), Load(Properties.Resources.VirginiasS)) },
            { "West", new Club("West", Load(Properties.Resources.WestL), Load(Properties.Resources.WestS)) },
        };

        internal static IClub GetClub(string name)
        {
            return string.IsNullOrWhiteSpace(name) || !Clubs.ContainsKey(name) 
                ? Clubs.First().Value 
                : Clubs[name];
        }

        private static BitmapSource Load(Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
