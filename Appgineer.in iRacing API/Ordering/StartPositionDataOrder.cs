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
using AiRAPI.Data.Results;

namespace AiRAPI.Ordering
{
    internal sealed class StartPositionDataOrder : AbstractDataOrder
    {
        public override IEnumerable<IEntitySessionResult> Sort(IEnumerable<IEntitySessionResult> results)
        {
            return results.OrderBy(r => r.StartPosition);
        }
    }
}
