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

using System.Windows.Media;
using AiRAPI.Data.Entity;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Entity
{
    internal sealed class Class : BindableBase, IClass
    {
        private byte _id;
        public byte Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _order = -1;
        public int Order
        {
            get => _order;
            set => SetProperty(ref _order, value);
        }

        private int _relativeSpeed;
        public int RelativeSpeed
        {
            get => _relativeSpeed;
            set => SetProperty(ref _relativeSpeed, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private Color _color;
        public Color Color
        {
            get => _color;
            set => SetProperty(ref _color, value);
        }

        private int _strengthOfClass;
        public int StrengthOfClass
        {
            get => _strengthOfClass;
            set => SetProperty(ref _strengthOfClass, value);
        }

        private bool _isCustomClass;
        public bool IsCustomClass
        {
            get => _isCustomClass;
            set => SetProperty(ref _isCustomClass, value);
        }

        public bool Equals(IClass other)
        {
            if (other == null)
                return false;

            return _id == other.Id && _order == other.Order && _relativeSpeed == other.RelativeSpeed && _name == other.Name;
        }

        public int CompareTo(IClass other)
        {
            return _order.CompareTo(other.Order);
        }
    }
}
