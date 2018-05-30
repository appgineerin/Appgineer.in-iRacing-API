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
using System.Collections.ObjectModel;
using System.Diagnostics;
using AiRAPI.Data.Lap;
using AiRAPI.Data.Results;

namespace AiRAPI.Impl.Lap
{
    internal class LapCollection : ReadOnlyObservableCollection<ILap>
    {
        private int _highestLap = -1;

        public Dictionary<int, ILap> Map { get; }

        public LapCollection() : base(new ObservableCollection<ILap>())
        {
            Map = new Dictionary<int, ILap>();
        }

        public void AddLap(CompletedLap lap)
        {
            if (lap.Number > 0)
            {
                if (Map.TryGetValue(lap.Number, out var oldLap))
                {
                    // Duplicate: remove old lap
                    Items.Remove(oldLap);
                }

                Map[lap.Number] = lap;
                Items.Add(lap);

                if (lap.Number > _highestLap) _highestLap = lap.Number;
            }
            else
            {
                Debug.WriteLine($"[LAP NR 0] Attempt to add lap: {lap}");
            }
            
            FillMissingLaps(lap.Result);
        }

        private void FillMissingLaps(IEntitySessionResult result)
        {
            if (Items.Count == 0)
                return;

            // Add initial laps we missed
            if (Items[0].Number > 1)
            {
                for (int i = Items[0].Number - 1; i > 0; i--)
                {
                    var lap = new CompletedLap(result);
                    lap.Number = i;
                    lap.SetExactLaptime(-1);

                    Map[i] = lap;
                    Items.Insert(0, lap);
                }
            }

            if (Items.Count > 2)
            {
                var last = Items[Items.Count - 1].Number;
                var secondToLast = Items[Items.Count - 2].Number;

                for (int i = secondToLast + 1; i < last; i++)
                {
                    var lap = new CompletedLap(result);
                    lap.Number = i;
                    lap.SetExactLaptime(-1);

                    Map[i] = lap;
                    Items.Insert(Items.Count - 1, lap);
                }
            }
        }
    }
}
