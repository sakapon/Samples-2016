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

namespace FaceWpf
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Drop += MainWindow_Drop;
        }

        void MainWindow_Drop(object sender, DragEventArgs e)
        {
            var imageUrl = GetImageUrl(e.Data);
            if (string.IsNullOrWhiteSpace(imageUrl)) return;

            var appModel = (AppModel)DataContext;
            appModel.ImageUrl.Value = imageUrl;
        }

        static string GetImageUrl(IDataObject data)
        {
            var fileDrops = (string[])data.GetData(DataFormats.FileDrop);
            if (fileDrops?.Length >= 1) return fileDrops[0];

            var text = (string)data.GetData(DataFormats.UnicodeText);
            return text;
        }
    }
}
