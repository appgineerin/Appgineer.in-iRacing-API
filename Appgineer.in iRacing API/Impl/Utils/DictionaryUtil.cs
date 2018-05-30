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
    public static class DictionaryUtil
    {
        public static TValue TryGet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            if (dict.TryGetValue(key, out var val))
                return val;
            return default(TValue);
        }
    }
}
