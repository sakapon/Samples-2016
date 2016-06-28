using System;

namespace DynamicBindingWpf
{
    public class AppModel
    {
        public dynamic TextModel { get; } = new TextModel().ToDynamicNotifiable(300);
    }

    public class TextModel
    {
        string _Input;

        public string Input
        {
            get { return _Input; }
            set
            {
                _Input = value;
                Output = _Input?.ToUpper();
            }
        }

        public string Output { get; private set; }
    }
}
