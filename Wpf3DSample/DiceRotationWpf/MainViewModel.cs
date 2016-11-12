using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace DiceRotationWpf
{
    public class MainViewModel
    {
        public Transform3D CubeTransform { get; private set; }

        MatrixTransform3D matrixTransform = new MatrixTransform3D();

        public MainViewModel()
        {
            InitializeCube();
        }

        void InitializeCube()
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(matrixTransform);
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0.6, 0.3, 0.7), -70)));
            CubeTransform = transform;
        }
    }
}
