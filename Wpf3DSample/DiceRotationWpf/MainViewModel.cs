using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Media3D;

namespace DiceRotationWpf
{
    public class MainViewModel
    {
        const double AngleDelta = 5.0;

        public Transform3D CubeTransform { get; }

        MatrixTransform3D matrixTransform = new MatrixTransform3D();

        public MainViewModel()
        {
            CubeTransform = InitializeCubeTransform();
        }

        Transform3D InitializeCubeTransform()
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(-0.8, 0.3, 0.5), 60)));
            transform.Children.Add(matrixTransform);
            return transform;
        }

        public void RotateDelta(Vector3D axis)
        {
            matrixTransform.Rotate(axis, AngleDelta);
        }

        public void RotateDelta(DeltaInfo info)
        {
            var delta = info.End - info.Start;
            if (delta.Length == 0) return;

            matrixTransform.Rotate(new Vector3D(delta.Y, delta.X, 0), delta.Length);
        }
    }

    public static class Media3DUtility
    {
        public static void Rotate(this MatrixTransform3D transform, Vector3D axis, double angle)
        {
            var matrix = transform.Matrix;
            matrix.Rotate(new Quaternion(axis, angle));
            transform.Matrix = matrix;
        }

        public static void RotateAt(this MatrixTransform3D transform, Vector3D axis, double angle, Point3D center)
        {
            var matrix = transform.Matrix;
            matrix.RotateAt(new Quaternion(axis, angle), center);
            transform.Matrix = matrix;
        }
    }
}
