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

namespace AiRAPI.Data.Location
{
    public interface ISector : IEquatable<ISector>, IComparable<ISector>
    {
        byte Index { get; }
        float Location { get; }
    }
}
