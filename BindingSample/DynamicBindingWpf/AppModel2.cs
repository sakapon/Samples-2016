using System;
using System.ComponentModel;
using System.Dynamic;

namespace DynamicBindingWpf
{
    public class AppModel2
    {
        public dynamic TextModel { get; } = new ExpandoObject();

        public AppModel2()
        {
            ((INotifyPropertyChanged)TextModel).PropertyChanged += (o, e) =>
            {
                if (e.PropertyName == "Input")
                    TextModel.Output = TextModel.Input?.ToUpper();
            };
        }
    }
}
