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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HandRotationLeap
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public Model3D CubeModel
        {
            get { return (Model3D)GetValue(CubeModelProperty); }
            set { SetValue(CubeModelProperty, value); }
        }

        public static readonly DependencyProperty CubeModelProperty =
            DependencyProperty.Register(nameof(CubeModel), typeof(Model3D), typeof(MainWindow), new PropertyMetadata(null));

        public MainWindow()
        {
            InitializeComponent();

            InitializeCube();
        }

        void InitializeCube()
        {
            var faces = new Dictionary<string, string>
            {
                { "-1,1,1 -1,-1,1 1,-1,1 1,1,1", nameof(Face1) },
                { "-1,1,1 -1,1,-1 -1,-1,-1 -1,-1,1", nameof(Face2) },
                { "-1,-1,1 -1,-1,-1 1,-1,-1 1,-1,1", nameof(Face3) },
                { "1,1,1 1,1,-1 -1,1,-1 -1,1,1", nameof(Face4) },
                { "1,-1,1 1,-1,-1 1,1,-1 1,1,1", nameof(Face5) },
                { "-1,-1,-1 -1,1,-1 1,1,-1 1,-1,-1", nameof(Face6) },
            };
            CubeModel = CubeUtility.CreateCubeModel(faces);
        }
    }
}
