using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace DynamicBindingWpf
{
    public static class DynamicNotifiable
    {
        public static DynamicNotifiable<T> ToDynamicNotifiable<T>(this T target, int intervalInMilliseconds = 1000) =>
            new DynamicNotifiable<T>(target, intervalInMilliseconds);

        public static bool IsIndexer(this PropertyInfo property) =>
            property.Name == "Item" && property.GetIndexParameters().Length > 0;
    }

    public class DynamicNotifiable<T> : DynamicObject, INotifyPropertyChanged
    {
        T Target { get; }

        Dictionary<string, PropertyInfo> Properties;
        Dictionary<string, object> PropertyValuesCache;

        Timer SyncTimer;

        public DynamicNotifiable(T target, int intervalInMilliseconds = 1000)
        {
            Target = target;

            Properties = Target.GetType().GetProperties()
                .Where(p => !p.IsIndexer())
                .ToDictionary(p => p.Name);
            PropertyValuesCache = Properties.Values
                .ToDictionary(p => p.Name, p => p.GetValue(Target));

            SyncTimer = new Timer(o => SyncPropertyValues(), null, intervalInMilliseconds, intervalInMilliseconds);
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

        #region Sync

        void SyncPropertyValues()
        {
            foreach (var name in Properties.Keys)
                SyncPropertyValue(name);
        }

        void SyncPropertyValue(string propertyName)
        {
            var oldValue = PropertyValuesCache[propertyName];
            var newValue = Properties[propertyName].GetValue(Target);

            if (Equals(oldValue, newValue)) return;
            PropertyValuesCache[propertyName] = newValue;
            NotifyPropertyChanged(propertyName);
        }

        #endregion
    }
}
