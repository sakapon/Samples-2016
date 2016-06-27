using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamicBindingWpf
{
    public static class DynamicNotifiable
    {
        public static DynamicNotifiable<T> ToDynamicNotifiable<T>(this T target) => new DynamicNotifiable<T>(target);
    }

    public class DynamicNotifiable<T> : DynamicObject, INotifyPropertyChanged
    {
        T Target { get; }

        public DynamicNotifiable(T target)
        {
            Target = target;
        }

        public T GetTargetObject() => Target;

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
