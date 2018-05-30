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
    internal abstract class AbstractDataOrder : IDataOrder
    {
        public abstract IEnumerable<IEntitySessionResult> Sort(IEnumerable<IEntitySessionResult> results);

        public IEntitySessionResult FindPosition(IEnumerable<IEntitySessionResult> results, int position)
        {
            return Sort(results).Skip(position - 1).FirstOrDefault();
        }

        public IEntitySessionResult FindPosition(IEnumerable<IEntitySessionResult> results, int position, string className)
        {
            return FindPosition(results.Where(r => r.Entity.Car.Class.Name == className), position);
        }
    }
}
