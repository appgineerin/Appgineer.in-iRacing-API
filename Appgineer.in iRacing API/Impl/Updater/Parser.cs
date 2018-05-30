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

using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Updater
{
    internal abstract class Parser
    {
        internal abstract void Parse(YamlMappingNode root, Simulation sim);
    }
}
