using System;
using System.Configuration;
using System.Windows;
using System.Windows.Markup;

namespace MarkupExWpf
{
    [MarkupExtensionReturnType(typeof(object))]
    public class AppSettingsExtension : MarkupExtension
    {
        public string Key { get; set; }

        public AppSettingsExtension()
        {
        }

        public AppSettingsExtension(string key)
        {
            Key = key;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var type = GetTargetPropertyType(serviceProvider);
            if (type == null) return DependencyProperty.UnsetValue;

            var value = ConfigurationManager.AppSettings[Key];
            if (type.IsValueType && value == null) return DependencyProperty.UnsetValue;

            return type.IsEnum ?
                Enum.Parse(type, value) :
                Convert.ChangeType(value, type);
        }

        static Type GetTargetPropertyType(IServiceProvider serviceProvider)
        {
            var provider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (provider == null) return null;

            var property = provider.TargetProperty as DependencyProperty;
            if (property == null) return null;

            return property.PropertyType;
        }
    }
}
