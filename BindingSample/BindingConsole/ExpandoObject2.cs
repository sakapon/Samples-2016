using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;

namespace BindingConsole
{
    /// <summary>
    /// A class such as the <see cref="ExpandoObject"/> class.
    /// </summary>
    public class ExpandoObject2 : DynamicObject, INotifyPropertyChanged
    {
        Dictionary<string, object> Properties = new Dictionary<string, object>();

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return Properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetValue(binder.Name);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetValue(binder.Name, value);
            return true;
        }

        object GetValue(string propertyName)
        {
            return Properties.ContainsKey(propertyName) ? Properties[propertyName] : null;
        }

        void SetValue(string propertyName, object value)
        {
            if (Properties.ContainsKey(propertyName) && Equals(Properties[propertyName], value)) return;
            Properties[propertyName] = value;
            NotifyPropertyChanged(propertyName);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
