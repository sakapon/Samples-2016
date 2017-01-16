using System;
using System.Diagnostics;
using System.Windows.Media.Media3D;

namespace HandRotationLeap
{
    /// <summary>
    /// Provides a set of methods to interconvert expressions of 3D rotation.
    /// </summary>
    /// <remarks>
    /// Euler angles depend on combinations of (x, y, z) and (roll, pitch, yaw), and its operation order.
    /// This class operates yaw on y, pitch on x, and roll on z.
    /// System.Numerics also uses these combinations and order.
    /// </remarks>
    public static class Rotation3DHelper
    {
        public static readonly Vector3D UnitX = new Vector3D(1, 0, 0);
        public static readonly Vector3D UnitY = new Vector3D(0, 1, 0);
        public static readonly Vector3D UnitZ = new Vector3D(0, 0, 1);

        public static double ToDegrees(double radians) => radians * 180 / Math.PI;
        public static double ToRadians(double degrees) => degrees * Math.PI / 180;

        public static Quaternion CreateQuaternionInRadians(Vector3D axis, double angleInRadians) => new Quaternion(axis, ToDegrees(angleInRadians));
        public static Quaternion Negate(this Quaternion q) => new Quaternion(-q.X, -q.Y, -q.Z, -q.W);

        public static Vector3D Multiply(this Vector3D v, Quaternion q) => v * q.ToMatrix3D();

        public static Matrix3D ToMatrix3D(this EulerAngles e) => e.ToQuaternion().ToMatrix3D();

        public static Matrix3D ToMatrix3D(this Quaternion q)
        {
            var m = new Matrix3D();
            m.Rotate(q);
            return m;
        }

        public static Quaternion ToQuaternion(this Matrix3D m) => m.ToEulerAngles().ToQuaternion();

        public static Quaternion ToQuaternion(this EulerAngles e) =>
            CreateQuaternionInRadians(UnitY, e.Yaw) *
            CreateQuaternionInRadians(UnitX, e.Pitch) *
            CreateQuaternionInRadians(UnitZ, e.Roll);

        public static EulerAngles ToEulerAngles(this Quaternion q) => q.ToMatrix3D().ToEulerAngles();

        public static EulerAngles ToEulerAngles(this Matrix3D m) => ToEulerAngles(UnitZ * m, UnitY * m);

        public static EulerAngles ToEulerAngles(Vector3D rotatedUnitZ, Vector3D rotatedUnitY)
        {
            var y_yaw = Math.Atan2(rotatedUnitZ.X, rotatedUnitZ.Z);

            var m_yaw_inv = CreateQuaternionInRadians(UnitY, -y_yaw).ToMatrix3D();
            rotatedUnitZ = rotatedUnitZ * m_yaw_inv;
            rotatedUnitY = rotatedUnitY * m_yaw_inv;

            var x_pitch = Math.Atan2(-rotatedUnitZ.Y, rotatedUnitZ.Z);

            var m_pitch_inv = CreateQuaternionInRadians(UnitX, -x_pitch).ToMatrix3D();
            rotatedUnitY = rotatedUnitY * m_pitch_inv;

            // 本来は -rotatedUnitY.X だけでよいはずです。しかし、X=0 のときに π でなく -π となってしまうため、場合分けします。
            var z_roll = Math.Atan2(rotatedUnitY.X == 0 ? 0 : -rotatedUnitY.X, rotatedUnitY.Y);

            return new EulerAngles { Yaw = y_yaw, Pitch = x_pitch, Roll = z_roll };
        }
    }

    [DebuggerDisplay(@"\{Yaw={Yaw}, Pitch={Pitch}, Roll={Roll}\}")]
    public struct EulerAngles
    {
        public double Yaw { get; set; }
        public double Pitch { get; set; }
        public double Roll { get; set; }
    }
}
