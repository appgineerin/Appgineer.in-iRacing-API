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
using System.Linq;
using System.Windows.Media;
using AiRAPI.Data.Entity;
using AiRAPI.MVVM;

namespace AiRAPI.Impl.Entity
{
    internal sealed class ClassManager : BindableBase, IClassManager
    {
        public ObservableCollection<IClass> Classes { get; }

        internal ClassManager()
        {
            Classes = new ObservableCollection<IClass>();
        }

        public IClass GetClass(int index)
        {
            return Classes.SingleOrDefault(c => c.Order == index);
        }

        public IClass GetClass(string name)
        {
            return Classes.FirstOrDefault(c => c.Name == name);
        }

        public IClass CreateCustomClass(string name, int order, Color color)
        {
            var clazz = Classes.FirstOrDefault(c => c.Name == name);
            if (clazz != null)
                return clazz;

            clazz = new Class
            {
                Name = name,
                RelativeSpeed = int.MaxValue - order,
                Color = color
            };

            AddClass(clazz);
            return clazz;
        }

        internal void AddClass(IClass clazz)
        {
            if (Classes.Any(c => c.Name == clazz.Name))
                return;

            // FIXME Application.Current.Dispatcher.Invoke(() => Classes.Add(clazz));
            Classes.Add(clazz);

            var i = 0;
            foreach (var c in Classes.OrderByDescending(c => c.RelativeSpeed).OfType<Class>())
                c.Order = i++;
        }
    }
}
