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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AiRAPI.Data.Results;

namespace AiRAPI.Data.Entity
{
    public interface IEntity : INotifyPropertyChanged, INotifyCollectionChanged, IReadOnlyCollection<IDriver>, IEquatable<IEntity>, IComparable<IEntity>
    {
        string Name { get; }
        int Id { get; }
        byte CarIdx { get; }
        bool IsFollowedEntity { get; }
        ICar Car { get; }
        ReadOnlyObservableCollection<IDriver> Drivers { get; }
        IDriver CurrentDriver { get; }
        IEntityResults Results { get; }
        BitmapSource Suit { get; }
        int SuitDesignNumber { get; }
        Color SuitColor1 { get; }
        Color SuitColor2 { get; }
        Color SuitColor3 { get; }
    }
}
