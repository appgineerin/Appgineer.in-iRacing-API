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

namespace AiRAPI.Impl.Utils
{
    internal static class EnumerableUtil
    {
        internal static int IndexOf<T>(this IEnumerable<T> enumerable, T obj)
        {
            var i = 0;
            foreach (var element in enumerable)
            {
                if (Equals(element, obj))
                    return i;

                i++;
            }

            return -1;
        }
    }
}
