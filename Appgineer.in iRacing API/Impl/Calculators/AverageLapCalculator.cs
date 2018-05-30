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
using System.Linq;
using AiRAPI.Calculators;
using AiRAPI.Data.Lap;
using AiRAPI.Data.Location;
using AiRAPI.Data.Results;

namespace AiRAPI.Impl.Calculators
{
    internal class AverageLapCalculator : IAverageLapCalculator
    {
        private const int LapsToAverage = 5;

        private static List<Weights> _weights;
        private static Dictionary<Weights, double> _weightValues;
        
        public AverageLapCalculator(ITrack track)
        {

            TrackId = track.Id;
            TrackLength = track.Length;
        }

        static AverageLapCalculator()
        {
            OrderPriorities();
            SetWeightValues();
        }

        public int TrackId { get; }
        public double TrackLength { get; }

        public Dictionary<Weights, Queue<double>> _buckets;

        private static void OrderPriorities()
        {
            _weights = new List<Weights>
            {
                // Add weights in order of importance

                // Leader race laps first
                Weights.LeaderRaceLapsExceptOpeningLap,

                // Qually laps
                Weights.QualLap,

                // Other drivers behind leader may be much slower
                Weights.NonLeaderRaceLapsExceptOpeningLap,

                // Start including opening lap (usually much slower than average)
                Weights.LeaderOpeningLap,
                Weights.NonLeaderOpeningLap,

                // Include pit laps
                Weights.LeaderPittedLap,
                Weights.NonLeaderPittedLap,

                // Finally caution laps
                Weights.CautionLap
            };
        }

        private static void SetWeightValues()
        {
            _weightValues = new Dictionary<Weights, double>();

            // Give weights a value between 1 to 0.
            var max = _weights.Count;
            for (var i = 0; i < max; i++)
            {
                _weightValues.Add(_weights[i], (max - i)/(double)max);
            }
        }
        
        public void Update(ISessionResult session, ISimulation sim)
        {
            // Completely rebuild the queue
            // TODO: Add laps to the queue as they come in, making sure not to add duplicates
            // Or: only update when new information available (new laps)
            //Queue.Clear();

            _buckets = new Dictionary<Weights, Queue<double>>();

            var leader = session.LiveLeader;
            var raceSession = sim.Session.GetLatestRace();
            var qualSession = sim.Session.GetQualification();

            if (leader != null && raceSession != null)
            {
                // Get last LapsToAverage laps of top 5
                var top5 = raceSession.Results.OrderBy(r => r.Position).Take(5);
                foreach (var result in top5)
                {
                    // Make sure we only take into account the leading class
                    if (result.Entity.Car.Class.Id != leader.Car.Class.Id)
                        continue;

                    // Get the valid laps, most recent first
                    var laps = result.Laps.Where(l => l.Time > 1).OrderByDescending(l => l.Number).ToArray();

//                    Debug.WriteLine(
//                        $" > Found {laps?.Length} laps for car P{result.Position}, #{result.Entity.Car.Number} {result.Entity.CurrentDriver.ShortName}");

                    if (laps.Length > 0)
                    {
                        var isLeader = result.Position == 1;

                        // Add as many laps as possible until LapsToAverage is reached
                        // Laps receive the appropriate weight
                        foreach (var lap in laps)
                        {
                            Weights weight;
                            if (lap.WasUnderCaution)
                            {
                                weight = Weights.CautionLap;
                            }
                            else
                            {
                                if (lap.WasOnPitRoad)
                                {
                                    weight = isLeader ? Weights.LeaderPittedLap : Weights.NonLeaderPittedLap;
                                }
                                else
                                {
                                    if (lap.Number <= 1)
                                        weight = isLeader ? Weights.LeaderOpeningLap : Weights.NonLeaderOpeningLap;
                                    else
                                        weight = isLeader
                                            ? Weights.LeaderRaceLapsExceptOpeningLap
                                            : Weights.NonLeaderRaceLapsExceptOpeningLap;
                                }
                            }

                            EnqueueLap(lap, weight);

                            // Stop if we have enough laps for a good average
                            if (HasEnoughLaps())
                                return;
                        }
                    }

                    // Stop if we have enough laps for a good average
                    if (HasEnoughLaps())
                        return;
                }
            }

            // Best qual laps
            if (qualSession?.Results?.Count > 0)
            {
                var qualResults = qualSession.Results.OrderBy(r => r.Position);
                foreach (var result in qualResults)
                {
                    if (result.FastestLapTime > 1)
                    {
                        EnqueueLap(result.FastestLapTime, Weights.QualLap);
                    }
                }
            }
        }

        private bool HasEnoughLaps()
        {
            // If we already have at least LapsToAverage laps of the highest weight, we can stop
            return _buckets.ContainsKey(Weights.LeaderRaceLapsExceptOpeningLap)
                   && _buckets[Weights.LeaderRaceLapsExceptOpeningLap].Count >= LapsToAverage;

            //var laps = Queue.Take(LapsToAverage).ToArray();
            //return laps.Length == LapsToAverage && laps.All(l => l.WeightValue >= 0.99);
        }

        private void EnqueueLap(ILap lap, Weights weight)
        {
            EnqueueLap(lap.Time, weight);
        }

        private void EnqueueLap(double laptime, Weights weight)
        {
            if (laptime > 1)
            {
                if (!_buckets.ContainsKey(weight))
                    _buckets.Add(weight, new Queue<double>());
                _buckets[weight].Enqueue(laptime);
            }
        }

        public double Calculate()
        {
            // Just average the LapsToAverage laps with the highest weight
            // PriorityQueue automatically returns the 5 highest weighted laps (in order that they were added in case the weight is the same).
            //var laps = Queue.Take(LapsToAverage).ToList();

            var laps = new List<double>();
            foreach (Weights weight in Enum.GetValues(typeof(Weights)))
            {
                if (_buckets.ContainsKey(weight))
                {
                    laps.AddRange(_buckets[weight]);
                }

                if (laps.Count >= LapsToAverage)
                    break;
            }
            
            if (laps.Count > 0)
                return laps.Take(LapsToAverage).Average();
            return 0;
        }
        
        public enum Weights
        {
            LeaderRaceLapsExceptOpeningLap,
            NonLeaderRaceLapsExceptOpeningLap,
            LeaderOpeningLap,
            NonLeaderOpeningLap,
            QualLap,
            LeaderPittedLap,
            NonLeaderPittedLap,
            CautionLap
        }
    }
}
