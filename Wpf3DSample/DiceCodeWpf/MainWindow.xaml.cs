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

namespace DiceCodeWpf
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TheViewport.Children.Add(CreateCubeVisual());
        }

        ModelVisual3D CreateCubeVisual()
        {
            var visual = new ModelVisual3D();

            var faces = new Dictionary<string, Visual>
            {
                { "0,1,1 0,0,1 1,0,1 1,1,1", Face1 },
                { "0,1,1 0,1,0 0,0,0 0,0,1", Face2 },
                { "0,0,1 0,0,0 1,0,0 1,0,1", Face3 },
                { "1,1,1 1,1,0 0,1,0 0,1,1", Face4 },
                { "1,0,1 1,0,0 1,1,0 1,1,1", Face5 },
                { "0,0,0 0,1,0 1,1,0 1,0,0", Face6 },
            };
            visual.Content = CubeUtility.CreateCubeModel(faces);

            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-0.5, -0.5, -0.5)));
            transform.Children.Add(new ScaleTransform3D(new Vector3D(2, 2, 2)));
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0.6, 0.3, 0.7), -70)));
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-2, 1, 0)));
            visual.Transform = transform;

            return visual;
        }
    }
}
