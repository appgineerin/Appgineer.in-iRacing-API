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
using System.Windows.Media.Imaging;
using AiRAPI.Data.Entity;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Entity
{
    internal sealed class Car : BindableBase, ICar
    {
        private string _number;
        public string Number
        {
            get => _number;
            internal set => SetProperty(ref _number, value);
        }

        private string _path;
        public string Path
        {
            get => _path;
            internal set => SetProperty(ref _path, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            internal set => SetProperty(ref _name, value);
        }

        private int _numberPadded;
        public int NumberPadded
        {
            get => _numberPadded;
            internal set => SetProperty(ref _numberPadded, value);
        }

        private int _id;
        public int Id
        {
            get => _id;
            internal set => SetProperty(ref _id, value);
        }

        private IClass _clazz;
        public IClass Class
        {
            get => _clazz;
            internal set => SetProperty(ref _clazz, value);
        }

        private ICarMovement _movement = new CarMovement();
        public ICarMovement Movement
        {
            get => _movement;
            internal set => SetProperty(ref _movement, value);
        }

        private BitmapSource _image;
        public BitmapSource Image
        {
            get => _image;
            internal set => SetProperty(ref _image, value);
        }

        private int _designNumber;
        public int DesignNumber
        {
            get => _designNumber;
            internal set => SetProperty(ref _designNumber, value);
        }

        private Color _color1;
        public Color Color1
        {
            get => _color1;
            internal set => SetProperty(ref _color1, value);
        }

        private Color _color2;
        public Color Color2
        {
            get => _color2;
            internal set => SetProperty(ref _color2, value);
        }

        private Color _color3;
        public Color Color3
        {
            get => _color3;
            internal set => SetProperty(ref _color3, value);
        }

        private Color _color4;
        public Color Color4
        {
            get => _color4;
            internal set => SetProperty(ref _color4, value);
        }

        private int _numberDesignNumber1;
        public int NumberDesignNumber1
        {
            get => _numberDesignNumber1;
            internal set => SetProperty(ref _numberDesignNumber1, value);
        }

        private int _numberDesignNumber2;
        public int NumberDesignNumber2
        {
            get => _numberDesignNumber2;
            internal set => SetProperty(ref _numberDesignNumber2, value);
        }

        private Color _numberColor1;
        public Color NumberColor1
        {
            get => _numberColor1;
            internal set => SetProperty(ref _numberColor1, value);
        }

        private Color _numberColor2;
        public Color NumberColor2
        {
            get => _numberColor2;
            internal set => SetProperty(ref _numberColor2, value);
        }

        private Color _numberColor3;
        public Color NumberColor3
        {
            get => _numberColor3;
            internal set => SetProperty(ref _numberColor3, value);
        }

        public bool Equals(ICar other)
        {
            if (other == null)
                return false;

            return _numberPadded == other.NumberPadded && _id == other.Id;
        }
    }
}
