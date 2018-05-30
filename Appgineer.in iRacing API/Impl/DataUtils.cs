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
using System.Text;

namespace AiRAPI.Impl
{
    internal static class DataUtils
    {
        internal static int FindIndex<T>(this IEnumerable<T> list, Predicate<T> match)
        {
            var i = 0;
            foreach (var element in list)
            {
                if (match(element))
                    return i;

                i++;
            }

            return -1;
        }

        internal static string ConvertToTimeString(this double seconds, string secFormat = "0.000", bool withMinutes = true, string prefix = "")
        {
            return ((float)seconds).ConvertToTimeString(secFormat, withMinutes, prefix);
        }

        internal static string ConvertToTimeString(this float seconds, string secFormat = "0.000", bool withMinutes = true, string prefix = "")
        {
            if (seconds < 0)
                return string.Empty;

            var min = 0;
            float sectime;
            if (withMinutes)
            {
                min = (int)(seconds / 60);
                sectime = seconds % 60;
            }
            else
            {
                sectime = seconds;
            }

            var sb = new StringBuilder(prefix);
            if (min > 0)
                sb.Append(min).Append(':').Append(sectime.ToString("0" + secFormat));
            else
                sb.Append(sectime.ToString(secFormat));

            return sb.ToString().Replace(',', '.');
        }

        internal static int PadCarNum(this string input)
        {
            if (!int.TryParse(input, out int num))
                return 0;

            var zero = input.Length - num.ToString().Length;

            var retVal = num;
            var numPlace = 1;
            if (num > 99)
                numPlace = 3;
            else if (num > 9)
                numPlace = 2;
            if (zero <= 0)
                return retVal;

            numPlace += zero;
            return num + 1000 * numPlace;
        }

        internal static double Interpolate(double refPct, double prevDist, double currDist, double prevTime, double currTime)
        {
            double result;

            if (Math.Abs(prevDist - currDist) < 10E-6)
                result = currTime;
            else
                result = prevTime + (currTime - prevTime) * (refPct - prevDist) / (currDist - prevDist);

            return result;
        }
    }
}
