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

using AiRAPI.Data.Entity;

namespace AiRAPI.Impl.Entity
{
    internal sealed class Team : Entity, ITeam
    {
        public bool Equals(ITeam other)
        {
            return base.Equals(other);
        }
    }
}
