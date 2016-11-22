using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Media.Media3D;
using Reactive.Bindings;
using Windows.Devices.Sensors;

namespace DiceOrientationWpf
{
    public class AppModel
    {
        public Transform3D CubeTransform { get; }
        MatrixTransform3D matrixTransform = new MatrixTransform3D();

        public AppModel()
        {
            CubeTransform = InitializeCubeTransform();
        }

        Transform3D InitializeCubeTransform()
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(matrixTransform);
            return transform;
        }
    }
}
