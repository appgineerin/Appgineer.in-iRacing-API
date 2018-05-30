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

using System.Collections.ObjectModel;
using System.ComponentModel;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;

namespace AiRAPI.Data.Results
{
    public interface IEntityResults : INotifyPropertyChanged
    {
        ReadOnlyObservableCollection<IEntitySessionResult> SessionResults { get; }
        IEntitySessionResult CurrentResult { get; }
        IEntity Entity { get; }

        bool HasResult(int sessionNumber);
        bool HasResult(SessionType sessionType);
        IEntitySessionResult GetResult(int sessionNumber);
        IEntitySessionResult GetResult(SessionType sessionType);
        IEntitySessionResult this[int sessionNumber] { get; }
        IEntitySessionResult this[SessionType sessionType] { get; }
    }
}
