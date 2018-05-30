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

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Results;
using AiRAPI.Impl.Results;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Entity
{
    internal abstract class Entity : BindableBase, IEntity
    {
        private string _name;
        public string Name
        {
            get => _name;
            internal set => SetProperty(ref _name, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            internal set => SetProperty(ref _id, value);
        }

        private byte _carIdx;
        public byte CarIdx
        {
            get => _carIdx;
            internal set => SetProperty(ref _carIdx, value);
        }

        private bool _isFollowedEntity;
        public bool IsFollowedEntity
        {
            get => _isFollowedEntity;
            internal set => SetProperty(ref _isFollowedEntity, value);
        }
        
        private ICar _car;
        public ICar Car
        {
            get => _car;
            internal set => SetProperty(ref _car, value);
        }

        internal readonly ObservableCollection<IDriver> DriversInt;
        public ReadOnlyObservableCollection<IDriver> Drivers { get; }

        private IDriver _currentDriver;
        public IDriver CurrentDriver
        {
            get => _currentDriver;
            internal set => SetProperty(ref _currentDriver, value);
        }

        private IEntityResults _results;
        public IEntityResults Results
        {
            get => _results;
            internal set => SetProperty(ref _results, value);
        }

        private BitmapSource _suit;
        public BitmapSource Suit
        {
            get => _suit;
            internal set => SetProperty(ref _suit, value);
        }

        private int _suitDesignNumber;
        public int SuitDesignNumber
        {
            get => _suitDesignNumber;
            internal set => SetProperty(ref _suitDesignNumber, value);
        }

        private Color _suitColor1;
        public Color SuitColor1
        {
            get => _suitColor1;
            internal set => SetProperty(ref _suitColor1, value);
        }

        private Color _suitColor2;
        public Color SuitColor2
        {
            get => _suitColor2;
            internal set => SetProperty(ref _suitColor2, value);
        }

        private Color _suitColor3;
        public Color SuitColor3
        {
            get => _suitColor3;
            internal set => SetProperty(ref _suitColor3, value);
        }

        internal Entity()
        {
            DriversInt = new ObservableCollection<IDriver>();
            Drivers = new ReadOnlyObservableCollection<IDriver>(DriversInt);
            DriversInt.CollectionChanged += OnCollectionChanged;

            _results = new EntityResults();
        }

        public int Count => Drivers.Count;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public IEnumerator<IDriver> GetEnumerator()
        {
            return DriversInt.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Equals(IEntity other)
        {
            if (other == null)
                return false;

            return _name == other.Name && _id == other.Id && _carIdx == other.CarIdx;
        }

        public int CompareTo(IEntity other)
        {
            return _carIdx.CompareTo(other.CarIdx);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // ReSharper disable once ExplicitCallerInfoArgument
            OnPropertyChanged("Count");
            CollectionChanged?.Invoke(this, e);
        }
    }
}
