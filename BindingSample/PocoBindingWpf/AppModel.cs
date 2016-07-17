using System;

namespace PocoBindingWpf
{
    public class AppModel
    {
        public InputModel Input { get; } = new InputModel();
    }

    public class InputModel
    {
        public long Number { get; set; }
    }
}
