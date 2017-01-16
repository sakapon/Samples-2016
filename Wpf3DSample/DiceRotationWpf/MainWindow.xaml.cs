using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        MainViewModel ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            ViewModel = (MainViewModel)DataContext;
            InitializeCube();

            var events = new EventsExtension<Grid>(DicePanel);
            events.MouseDragDelta.Subscribe(ViewModel.RotateDelta);
            // For using Trackball.
            //var events = new EventsExtension_ForTrackball<Grid>(DicePanel);
            //events.MouseDragDelta.Subscribe(ViewModel.RotateDelta_ForTrackball);
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

        static readonly Dictionary<string, Vector3D> Axes = new Dictionary<string, Vector3D>
        {
            { "-x", Vector3D.Parse("-1,0,0") },
            { "+x", Vector3D.Parse("1,0,0") },
            { "-y", Vector3D.Parse("0,-1,0") },
            { "+y", Vector3D.Parse("0,1,0") },
            { "-z", Vector3D.Parse("0,0,-1") },
            { "+z", Vector3D.Parse("0,0,1") },
        };

        void Rotate_Click(object sender, RoutedEventArgs e)
        {
            var button = (RepeatButton)sender;
            var command = (string)button.CommandParameter;

            ViewModel.RotateDelta(Axes[command]);
        }
    }
}
