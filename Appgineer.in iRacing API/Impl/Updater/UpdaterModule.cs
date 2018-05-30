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

namespace AiRAPI.Impl.Updater
{
    internal abstract class UpdaterModule
    {
        protected DataUpdater Updater;

        internal UpdaterModule(DataUpdater updater)
        {
            Updater = updater;
        }
    }
}
