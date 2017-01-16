using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Media.Media3D;
using Monsoon.Reactive.Leap;
using Reactive.Bindings;

namespace HandRotationLeap
{
    public class AppModel
    {
        public Transform3D CubeTransform { get; }
        MatrixTransform3D matrixTransform = new MatrixTransform3D();

        public LeapManager LeapManager { get; } = new LeapManager();

        public ReadOnlyReactiveProperty<Leap.Hand> FrontHand { get; }
        public ReadOnlyReactiveProperty<double> InnerProduct { get; }

        public ReadOnlyReactiveProperty<EulerAngles> EulerAngles_org { get; }
        public ReadOnlyReactiveProperty<EulerAngles> EulerAngles { get; }

        public ReadOnlyReactiveProperty<Matrix3D> Rotation_org { get; }
        public ReadOnlyReactiveProperty<Matrix3D> Rotation { get; }

        public AppModel()
        {
            CubeTransform = InitializeCubeTransform();

            FrontHand = LeapManager.FrameArrived
                .Select(f => f.Hands.Frontmost)
                .Select(h => h?.IsValid == true ? h : null)
                .ToReadOnlyReactiveProperty();
            InnerProduct = FrontHand
                .Select(h => h != null ? h.Direction.Dot(h.PalmNormal) : double.NaN)
                .ToReadOnlyReactiveProperty();

            EulerAngles_org = FrontHand
                .Select(h => h != null ? h.GetEulerAngles_org() : new EulerAngles())
                .ToReadOnlyReactiveProperty();
            EulerAngles = FrontHand
                .Select(h => h != null ? h.GetEulerAngles() : new EulerAngles())
                .ToReadOnlyReactiveProperty();

            Rotation_org = EulerAngles_org
                .Select(Rotation3DHelper.ToMatrix3D)
                .ToReadOnlyReactiveProperty();
            Rotation = EulerAngles
                .Select(Rotation3DHelper.ToMatrix3D)
                .ToReadOnlyReactiveProperty();

            Rotation
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(m => matrixTransform.Matrix = m);
        }

        Transform3D InitializeCubeTransform()
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(-0.8, 0.3, 0.5), 60)));
            transform.Children.Add(matrixTransform);
            return transform;
        }
    }

    public static class Leap3DHelper
    {
        public static Vector3D ToVector3D(this Leap.Vector v) => new Vector3D(v.x, v.y, v.z);

        public static EulerAngles GetEulerAngles_org(this Leap.Hand h) => new EulerAngles
        {
            Yaw = h.Direction.Yaw,
            Pitch = h.Direction.Pitch,
            Roll = h.PalmNormal.Roll,
        };

        public static EulerAngles GetEulerAngles(this Leap.Hand h) =>
            Rotation3DHelper.ToEulerAngles(-h.Direction.ToVector3D(), -h.PalmNormal.ToVector3D());
    }
}
