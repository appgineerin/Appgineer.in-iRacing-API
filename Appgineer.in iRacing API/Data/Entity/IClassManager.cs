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

namespace AiRAPI.Data.Entity
{
    public interface IClassManager : INotifyPropertyChanged
    {
        ObservableCollection<IClass> Classes { get; }

        IClass GetClass(int index);
        IClass GetClass(string name);
    }
}
