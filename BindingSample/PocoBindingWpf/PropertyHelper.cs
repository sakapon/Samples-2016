using System;
using System.ComponentModel;

namespace PocoBindingWpf
{
    public static class PropertyHelper
    {
        public static PropertyDescriptor GetDescriptor(this object obj, string propertyName)
        {
            var properties = TypeDescriptor.GetProperties(obj);
            return properties[propertyName];
        }

        public static void SetValue(this object obj, string propertyName, object value)
        {
            var descriptor = obj.GetDescriptor(propertyName);
            descriptor.SetValue(obj, value);
        }

        public static void AddValueChanged(this object obj, string propertyName, Action action)
        {
            var descriptor = obj.GetDescriptor(propertyName);
            descriptor.AddValueChanged(obj, (o, e) => action());
        }
    }
}
