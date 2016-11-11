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
        MatrixTransform3D matrixTransform = new MatrixTransform3D();
        TranslateTransform3D translateTransform = new TranslateTransform3D();

        public MainWindow()
        {
            InitializeComponent();

            TheViewport.Children.Add(CreateCubeVisual());
        }

        ModelVisual3D CreateCubeVisual()
        {
            var visual = new ModelVisual3D();

            var cubeModel = CubeUtility.CreateCubeModel();
            CubeUtility.SetMaterialToCube(cubeModel, 0, Face1);
            CubeUtility.SetMaterialToCube(cubeModel, 1, Face2);
            CubeUtility.SetMaterialToCube(cubeModel, 2, Face3);
            CubeUtility.SetMaterialToCube(cubeModel, 3, Face4);
            CubeUtility.SetMaterialToCube(cubeModel, 4, Face5);
            CubeUtility.SetMaterialToCube(cubeModel, 5, Face6);
            visual.Content = cubeModel;

            var transform = new Transform3DGroup();
            transform.Children.Add(new TranslateTransform3D(new Vector3D(-0.5, -0.5, -0.5)));
            transform.Children.Add(new ScaleTransform3D(new Vector3D(2, 2, 2)));
            transform.Children.Add(matrixTransform);
            transform.Children.Add(translateTransform);
            visual.Transform = transform;

            return visual;
        }
    }
}
