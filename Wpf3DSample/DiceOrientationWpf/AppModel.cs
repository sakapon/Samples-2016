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

        public ReactiveProperty<InclinometerReading> InclinationData { get; } = new ReactiveProperty<InclinometerReading>(mode: ReactivePropertyMode.None);
        public ReactiveProperty<OrientationSensorReading> OrientationData { get; } = new ReactiveProperty<OrientationSensorReading>(mode: ReactivePropertyMode.None);

        public ReadOnlyReactiveProperty<Quaternion> RotationQuaternion { get; }
        public ReadOnlyReactiveProperty<string> RotationQuaternionString { get; }
        public ReadOnlyReactiveProperty<Matrix3D> RotationMatrix { get; }

        public AppModel()
        {
            CubeTransform = InitializeCubeTransform();

            var inclinometer = Inclinometer.GetDefault();
            if (inclinometer != null)
            {
                inclinometer.ReportInterval = 200;
                inclinometer.ReadingChanged += (o, e) => InclinationData.Value = e.Reading;
            }

            var orientationSensor = OrientationSensor.GetDefault();
            if (orientationSensor != null)
            {
                orientationSensor.ReportInterval = 200;
                orientationSensor.ReadingChanged += (o, e) => OrientationData.Value = e.Reading;
            }

            RotationQuaternion = OrientationData.Select(d => d.Quaternion.ToQuaternion()).ToReadOnlyReactiveProperty();
            RotationQuaternionString = RotationQuaternion.Select(q => $"{q.W:F2}; ({q.X:F2}, {q.Y:F2}, {q.Z:F2})").ToReadOnlyReactiveProperty();

            RotationMatrix = OrientationData.Select(d => d.RotationMatrix.ToMatrix3D()).ToReadOnlyReactiveProperty();
        }

        Transform3D InitializeCubeTransform()
        {
            var transform = new Transform3DGroup();
            transform.Children.Add(matrixTransform);
            return transform;
        }
    }

    public static class SensorsHelper
    {
        public static Matrix3D ToMatrix3D(this SensorRotationMatrix m) =>
            new Matrix3D(m.M11, m.M21, m.M31, 0, m.M12, m.M22, m.M32, 0, m.M13, m.M23, m.M33, 0, 0, 0, 0, 1);

        public static Quaternion ToQuaternion(this SensorQuaternion q) =>
            new Quaternion(q.X, q.Y, q.Z, q.W);

        public static Matrix3D Transpose(this Matrix3D m) =>
            new Matrix3D(m.M11, m.M21, m.M31, m.M14, m.M12, m.M22, m.M32, m.M24, m.M13, m.M23, m.M33, m.M34, m.OffsetX, m.OffsetY, m.OffsetZ, m.M44);

        public static Matrix3D ToMatrix3D(this Quaternion q)
        {
            var m = new Matrix3D();
            m.Rotate(q);
            return m;
        }
    }
}
