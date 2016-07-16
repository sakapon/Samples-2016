using System;

namespace PocoBindingWpf
{
    public class AppModel
    {
        public TextModel TextModel { get; } = new TextModel();
    }

    public class TextModel
    {
        public string Input { get; set; }
    }
}
