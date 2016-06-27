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
            Bind_OneWay();
        }

        static void Bind_OneWay()
        {
            // Binding Source (Any object).
            var person = new Person1 { Id = 123, Name = "Taro" };

            // Binding Target must be FrameworkElement.
            var textBlock = new TextBlock { Text = "Default" };
            Console.WriteLine(textBlock.Text);

            // Binds target to source.
            var binding = new Binding(nameof(person.Name)) { Source = person };
            textBlock.SetBinding(TextBlock.TextProperty, binding);
            Console.WriteLine(textBlock.Text);

            // Changes source value.
            person.Name = "Jiro";
            Console.WriteLine(textBlock.Text);
        }
    }
}
