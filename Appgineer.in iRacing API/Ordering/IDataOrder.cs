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
using AiRAPI.Data.Results;

namespace AiRAPI.Ordering
{
    public interface IDataOrder
    {
        IEnumerable<IEntitySessionResult> Sort(IEnumerable<IEntitySessionResult> results);
        IEntitySessionResult FindPosition(IEnumerable<IEntitySessionResult> results, int position);
        IEntitySessionResult FindPosition(IEnumerable<IEntitySessionResult> results, int position, string className);
    }
}
