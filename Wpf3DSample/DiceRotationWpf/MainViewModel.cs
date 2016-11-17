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

        const double AngleRatio = 0.5;
        const double AxisXYLength = 50.0;

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
            var deltaLength = delta.Length;
            if (deltaLength == 0) return;

            var distance = GetDistance((Vector)info.Start, delta);
            delta *= AxisXYLength / deltaLength;
            matrixTransform.Rotate(new Vector3D(delta.Y, delta.X, distance), AngleRatio * deltaLength);
        }

        // 原点から delta までの符号付き距離を求めます。
        static double GetDistance(Vector start, Vector delta)
        {
            var angle = Vector.AngleBetween(delta, start);
            return start.Length * Math.Sin(angle * Math.PI / 180);
        }

        public void RotateDelta_ForTrackball(DeltaInfo info)
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
