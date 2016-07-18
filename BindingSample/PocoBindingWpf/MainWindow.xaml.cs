using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PocoBindingWpf
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var AppModel = (AppModel)DataContext;

            Loaded += (o, e) =>
            {
                // Notification does not work in usual property setting.
                //AppModel.Input.Number = 1234;
                AppModel.Input.SetValue(nameof(InputModel.Number), 1234);
            };

            AppModel.Input.AddValueChanged(nameof(InputModel.Number), () => Console.WriteLine(AppModel.Input.Number));
        }
    }
}
