using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace BindingConsole
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Binding Source (Any object).
            var person = new Person0 { Id = 0, Name = "Taro" };

            // Binding Target must be FrameworkElement.
            var textBlock = new TextBlock { Text = "Default" };
            Console.WriteLine(textBlock.Text);

            // Binds target to source.
            var binding = new Binding(nameof(person.Name)) { Source = person };
            textBlock.SetBinding(TextBlock.TextProperty, binding);
            Console.WriteLine(textBlock.Text);

            // Changes value.
            person.Name = "Jiro";
            Console.WriteLine(textBlock.Text);
        }
    }
}
