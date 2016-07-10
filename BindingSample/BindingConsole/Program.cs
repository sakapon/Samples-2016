using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
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
            Bind_Dependency_TwoWay();
            Bind_Indexer_TwoWay();
            Bind_Expando_TwoWay();
            Bind_Collection();
            Bind_Collection_View();
            Bind_Collection_CurrentItem();
        }

        static void Bind_OneWay()
        {
            // Binding Source (Any object).
            var person = new Person1 { Id = 123, Name = "Taro" };

            // Binding Target (DependencyObject).
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

            // Binding Target (DependencyObject).
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

        static void Bind_Dependency_TwoWay()
        {
            // [STAThread] is unnecessary.
            // Binding Source (Any object).
            var person = new Person2 { Id = 123, Name = "Taro" };

            // Binding Target (DependencyObject).
            var target = new Person2 { Id = 999, Name = "Default" };
            Console.WriteLine(target.Name);

            // Binds target to source.
            // Default mode is OneWay.
            var binding = new Binding(nameof(person.Name)) { Source = person, Mode = BindingMode.TwoWay };
            BindingOperations.SetBinding(target, Person2.NameProperty, binding);
            Console.WriteLine(target.Name);

            // Changes source value.
            person.Name = "Jiro";
            Console.WriteLine(target.Name);

            // Changes target value.
            target.Name = "Saburo";
            Console.WriteLine(person.Name);
        }

        static void Bind_Indexer_TwoWay()
        {
            // Binding Source with indexer.
            var map = new PersonMap { [123] = "Taro" };

            // Binding Target (DependencyObject).
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

        static void Bind_Expando_TwoWay()
        {
            // Binding Source (Any object).
            dynamic person = new ExpandoObject();
            person.Id = 123;
            person.Name = "Taro";

            // Binding Target (DependencyObject).
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

        static void Bind_Collection()
        {
            var taro = new Person1 { Id = 123, Name = "Taro" };
            var jiro = new Person1 { Id = 234, Name = "Jiro" };

            // Binding Source (collection).
            var people = new ObservableCollection<Person1> { taro };

            // Binding Target.
            var itemsControl = new ItemsControl();
            Console.WriteLine(itemsControl.Items.Count);

            // Binds target to source.
            // MEMO: Binding Source のオブジェクト自体が変更されないのであれば、
            // ItemsSource プロパティのデータ バインディングは必須ではありません。
            itemsControl.ItemsSource = people;
            Console.WriteLine(itemsControl.Items.Count);

            // Changes source collection.
            people.Add(jiro);
            Console.WriteLine(itemsControl.Items.Count);
            people.RemoveAt(0);
            Console.WriteLine(itemsControl.Items.Count);

            // MEMO: ItemsSource に値を設定している場合、Items を直接変更しようとすると例外が発生します。
            //itemsControl.Items.Add(jiro);
        }

        static void Bind_Collection_View()
        {
            var people = new ObservableCollection<Person1>
            {
                new Person1 { Id = 123, Name = "Taro" },
                new Person1 { Id = 234, Name = "Jiro" },
                new Person1 { Id = 678, Name = "Mana" },
                new Person1 { Id = 789, Name = "Kana" },
            };

            var itemsControl = new ItemsControl { ItemsSource = people };
            Console.WriteLine(string.Join(", ", itemsControl.Items.Cast<Person1>().Select(p => p.Name)));

            itemsControl.Items.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            Console.WriteLine(string.Join(", ", itemsControl.Items.Cast<Person1>().Select(p => p.Name)));

            itemsControl.Items.Filter = o => ((Person1)o).Name.Contains("ana");
            Console.WriteLine(string.Join(", ", itemsControl.Items.Cast<Person1>().Select(p => p.Name)));

            people.Add(new Person1 { Id = 567, Name = "Hana" });
            Console.WriteLine(string.Join(", ", itemsControl.Items.Cast<Person1>().Select(p => p.Name)));
        }

        static void Bind_Collection_CurrentItem()
        {
            var taro = new Person1 { Id = 123, Name = "Taro" };
            var jiro = new Person1 { Id = 234, Name = "Jiro" };
            var people = new[] { taro, jiro };

            var grid = new Grid { DataContext = people };
            var listBox = new ListBox { IsSynchronizedWithCurrentItem = true };
            grid.Children.Add(listBox);
            var textBlock = new TextBlock { Text = "Default" };
            grid.Children.Add(textBlock);

            textBlock.SetBinding(TextBlock.TextProperty, new Binding("/Name"));
            Console.WriteLine((listBox.SelectedValue as Person1)?.Name ?? "null");
            Console.WriteLine(textBlock.Text);

            // MEMO: In case that IsSynchronizedWithCurrentItem is false, SelectedValue is null.
            listBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding());
            Console.WriteLine((listBox.SelectedValue as Person1)?.Name ?? "null");
            Console.WriteLine(textBlock.Text);

            listBox.SelectedValue = jiro;
            Console.WriteLine((listBox.SelectedValue as Person1)?.Name ?? "null");
            Console.WriteLine(textBlock.Text);
        }
    }
}
