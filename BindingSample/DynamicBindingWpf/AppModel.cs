using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicBindingWpf
{
    public class AppModel
    {
        public dynamic TextModel { get; } = new TextModel().ToDynamicNotifiable();
    }

    public class TextModel
    {
        string input;
        public string Input
        {
            get { return input; }
            set
            {
                input = value;
                Output = input?.ToUpper();
            }
        }

        public string Output { get; private set; }
    }
}
