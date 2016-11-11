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

namespace DiceRotationWpf
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

        public Transform3D CubeTransform
        {
            get { return (Transform3D)GetValue(CubeTransformProperty); }
            set { SetValue(CubeTransformProperty, value); }
        }

        public static readonly DependencyProperty CubeTransformProperty =
            DependencyProperty.Register(nameof(CubeTransform), typeof(Transform3D), typeof(MainWindow), new PropertyMetadata(null));

        MatrixTransform3D matrixTransform = new MatrixTransform3D();

        public MainWindow()
        {
            InitializeComponent();

            InitializeCube();
        }

        void InitializeCube()
        {
            var faces = new Dictionary<string, string>
            {
                { "-1,1,1 -1,-1,1 1,-1,1 1,1,1", "Face1" },
                { "-1,-1,1 -1,-1,-1 1,-1,-1 1,-1,1", "Face2" },
            };
            CubeModel = CubeUtility.CreateCubeModel(faces);

            var transform = new Transform3DGroup();
            transform.Children.Add(matrixTransform);
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0.6, 0.3, 0.7), -70)));
            CubeTransform = transform;
        }
    }
}
