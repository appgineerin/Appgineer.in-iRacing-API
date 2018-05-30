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
using AiRAPI.Calculators;

namespace AiRAPI.Impl.Calculators
{
    internal sealed class TimeDelta : ITimeDelta
    {
        private readonly float _splitdistance;
        private readonly int _maxcars;
        private readonly double[][] _splits;
        private readonly int[] _splitPointer;
        private readonly float _splitLength;
        private double _prevTimestamp;
        private readonly int _followed;
        private readonly double[] _bestlap;
        private readonly double[] _currentlap;
        private readonly int _arraySize;

        internal TimeDelta(float length, float splitdist, int drivers)
        {
            _splitdistance = splitdist;
            _maxcars = drivers;
            _arraySize = (int)Math.Round(length / _splitdistance);
            _splitLength = (float)(1.0 / _arraySize);
            _followed = -1;
            _bestlap = new double[_arraySize];
            _currentlap = new double[_arraySize];
            _splits = new double[_maxcars][];
            _splitPointer = new int[_maxcars];

            for (var i = 0; i < _maxcars; i++)
                _splits[i] = new double[_arraySize];
        }

        internal void Update(double timestamp, float[] trackPosition)
        {
            var temp = Array.ConvertAll(trackPosition, item => (double)item);
            Update(timestamp, temp);
        }

        private void Update(double timestamp, double[] trackPosition)
        {
            if (!(timestamp > _prevTimestamp))
                return;

            for (var i = 0; i < trackPosition.Length; i++)
            {
                if (!(trackPosition[i] > 0))
                    continue;

                var currentSplitPointer = (int)Math.Floor(trackPosition[i] % 1 / _splitLength);
                if (currentSplitPointer == _splitPointer[i])
                    continue;

                var distance = trackPosition[i] - currentSplitPointer * _splitLength;
                var correction = distance / _splitLength;
                var currentSplitTime = timestamp - (timestamp - _prevTimestamp) * correction;
                var newlap = currentSplitPointer < 100 / _splitdistance && _splitPointer[i] > _arraySize - 100 / _splitdistance;

                var splithop = currentSplitPointer - _splitPointer[i];
                var splitcumulator = (currentSplitTime - _prevTimestamp) / splithop;
                var k = 1;

                if (splithop < 0 && newlap)
                {
                    splithop = _arraySize - _splitPointer[i] + currentSplitPointer;
                    if (_followed >= 0 && i == _followed)
                    {
                        for (var j = _splitPointer[i] + 1; j < _arraySize; j++)
                        {
                            _splits[i][j % _arraySize] = _splits[i][_splitPointer[i]] + k++ * splitcumulator;
                        }
                    }
                }

                if (_followed >= 0 && i == _followed)
                {
                    if (newlap)
                    {
                        if (currentSplitTime - _splits[i][0] < _bestlap[_bestlap.Length - 1] || Math.Abs(_bestlap[_bestlap.Length - 1]) < 10E-6)
                        {
                            for (var j = 0; j < _bestlap.Length - 1; j++)
                            {
                                _bestlap[j] = _splits[i][j + 1] - _splits[i][0];
                            }

                            _bestlap[_bestlap.Length - 1] = currentSplitTime - _splits[i][0];
                        }
                    }
                    
                    _currentlap[currentSplitPointer] = currentSplitTime;
                }

                if (splithop > 1)
                {
                    k = 1;
                    for (int j = _splitPointer[i] + 1; j % _arraySize != currentSplitPointer; j++)
                    {
                        _splits[i][j % _arraySize] = _splits[i][_splitPointer[i]] + (k++ * splitcumulator);
                    }
                }

                _splits[i][currentSplitPointer] = currentSplitTime;
                _splitPointer[i] = currentSplitPointer;
            }
            _prevTimestamp = timestamp;
        }

        public TimeSpan GetDelta(int caridx1, int caridx2)
        {
            if (caridx1 >= _maxcars || caridx2 >= _maxcars || caridx1 < 0 || caridx2 < 0)
                return new TimeSpan();

            var comparedSplit = _splitPointer[caridx1];
            if (comparedSplit < 0)
                comparedSplit = _splits[caridx1].Length - 1;

            var delta = _splits[caridx1][comparedSplit] - _splits[caridx2][comparedSplit];
            if (Math.Abs(_splits[caridx1][comparedSplit]) < 10E-6 || Math.Abs(_splits[caridx2][comparedSplit]) < 10E-6)
                return new TimeSpan();

            return new TimeSpan(0, 0, 0, (int)Math.Floor(delta), (int)Math.Abs(delta % 1 * 1000));
        }
    }
}
