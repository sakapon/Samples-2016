using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DynamicBindingWpf
{
    public static class DynamicNotifiable
    {
        public static DynamicNotifiable<T> ToDynamicNotifiable<T>(this T target) => new DynamicNotifiable<T>(target);
    }

    public class DynamicNotifiable<T> : DynamicObject, INotifyPropertyChanged
    {
        public const string IndexerName = "Item[]";

        T Target { get; }

        Dictionary<string, PropertyInfo> Properties;
        Dictionary<string, object> PropertyValuesCache;

        public DynamicNotifiable(T target)
        {
            Target = target;

            Properties = Target.GetType().GetProperties()
                .ToDictionary(p => p.Name);
            PropertyValuesCache = Properties.Values
                .ToDictionary(p => p.Name, p => p.GetValue(Target));
        }

        public T GetTargetObject() => Target;

        #region DynamicObject

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = Properties[binder.Name].GetValue(Target);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            Properties[binder.Name].SetValue(Target, value);
            return true;
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion
    }
}
