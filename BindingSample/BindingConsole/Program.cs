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
            Bind_TwoWay();
            Bind_Indexer_TwoWay();
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

        static void Bind_TwoWay()
        {
            // Binding Source (Any object).
            var person = new Person1 { Id = 123, Name = "Taro" };

            // Binding Target must be FrameworkElement.
            var textBox = new TextBox { Text = "Default" };
            Console.WriteLine(textBox.Text);

            // Binds target to source.
            var binding = new Binding(nameof(person.Name)) { Source = person, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            textBox.SetBinding(TextBox.TextProperty, binding);
            Console.WriteLine(textBox.Text);

            // Changes source value.
            person.Name = "Jiro";
            Console.WriteLine(textBox.Text);

            // Changes target value.
            textBox.Text = "Saburo";
            Console.WriteLine(person.Name);
        }

        static void Bind_Indexer_TwoWay()
        {
            // Binding Source with indexer.
            var map = new PersonMap { [123] = "Taro" };

            // Binding Target must be FrameworkElement.
            var textBox = new TextBox { Text = "Default" };
            Console.WriteLine(textBox.Text);

            // Binds target to source.
            var binding = new Binding("[123]") { Source = map, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
            textBox.SetBinding(TextBox.TextProperty, binding);
            Console.WriteLine(textBox.Text);

            // Changes source value.
            map[123] = "Jiro";
            Console.WriteLine(textBox.Text);

            // Changes target value.
            textBox.Text = "Saburo";
            Console.WriteLine(map[123]);
        }
    }
}
