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
using System.Collections.ObjectModel;
using System.Linq;
using AiRAPI.Data.Location;
using AiRAPI.Impl.Location;
using AiRAPI.Impl.Utils;
using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Updater.Parsers
{
    internal sealed class SectorParser : Parser
    {
        internal override void Parse(YamlMappingNode root, Simulation sim)
        {
            var sectors = root.GetList("SplitTimeInfo.Sectors");
            var sectorList = new ObservableCollection<ISector>();

            foreach (var sector in sectors.Children.OfType<YamlMappingNode>())
                sectorList.Add(new Sector { Index = sector.GetByte("SectorNum"), Location = sector.GetFloat("SectorStartPct") });

            ((Track)sim.Session.Track).SectorsInt = sectorList;
            ((Track)sim.Session.Track).Sectors = new ReadOnlyObservableCollection<ISector>(((Track)sim.Session.Track).SectorsInt);

            sim.Session.Track.SelectedSectors.Clear();
            InitSelectedSectors(sim);
        }

        private static void InitSelectedSectors(ISimulation sim)
        {
            if (sim.Session.Track.Sectors.Count <= 3)
            {
                foreach (var sector in sim.Session.Track.Sectors)
                    sim.Session.Track.SelectedSectors.Add(sector);

                return;
            }

            ISector prevSector = new Sector();
            foreach (var sector in sim.Session.Track.Sectors)
            {
                if (Math.Abs(sector.Location) < 10E-6 && sim.Session.Track.SelectedSectors.Count == 0)
                {
                    sim.Session.Track.SelectedSectors.Add(sector);
                }
                else if (sector.Location >= 0.333 && sim.Session.Track.SelectedSectors.Count == 1)
                {
                    sim.Session.Track.SelectedSectors.Add(sector.Location - 0.333 < Math.Abs(prevSector.Location - 0.333) ? sector : prevSector);
                }
                else if (sector.Location >= 0.666 && sim.Session.Track.SelectedSectors.Count == 2)
                {
                    sim.Session.Track.SelectedSectors.Add(sector.Location - 0.666 < Math.Abs(prevSector.Location - 0.666) ? sector : prevSector);
                }

                prevSector = sector;
            }
        }
    }
}
