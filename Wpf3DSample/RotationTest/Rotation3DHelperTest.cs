using System;
using System.Windows.Media.Media3D;
using Blaze.Randomization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RotationTest
{
    [TestClass]
    public class Rotation3DHelperTest
    {
        const double π = Math.PI;

        static Quaternion NextQuaternion() => Rotation3DHelper.CreateQuaternionInRadians(
            new Vector3D(RandomHelper.NextDouble(-1, 1), RandomHelper.NextDouble(-1, 1), RandomHelper.NextDouble(-1, 1)),
            RandomHelper.NextDouble(-7, 7));

        [TestMethod]
        public void ToEulerAngles_01()
        {
            ToEulerAngles_One(new EulerAngles { Yaw = 0, Pitch = 0, Roll = 0 }, new Vector3D(0, 0, 1), new Vector3D(0, 1, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = π / 2, Pitch = 0, Roll = 0 }, new Vector3D(1, 0, 0), new Vector3D(0, 1, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = -π / 2, Pitch = 0, Roll = 0 }, new Vector3D(-1, 0, 0), new Vector3D(0, 1, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = -3 * π / 4, Pitch = 0, Roll = 0 }, new Vector3D(-0.7, 0, -0.7), new Vector3D(0, 1, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = π, Pitch = 0, Roll = 0 }, new Vector3D(0, 0, -1), new Vector3D(0, 1, 0));

            ToEulerAngles_One(new EulerAngles { Yaw = 0, Pitch = π / 2, Roll = 0 }, new Vector3D(0, -1, 0), new Vector3D(0, 0, 1));
            ToEulerAngles_One(new EulerAngles { Yaw = 0, Pitch = -π / 2, Roll = 0 }, new Vector3D(0, 1, 0), new Vector3D(0, 0, -1));

            ToEulerAngles_One(new EulerAngles { Yaw = 0, Pitch = 0, Roll = π / 2 }, new Vector3D(0, 0, 1), new Vector3D(-1, 0, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = 0, Pitch = 0, Roll = -π / 2 }, new Vector3D(0, 0, 1), new Vector3D(1, 0, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = 0, Pitch = 0, Roll = -3 * π / 4 }, new Vector3D(0, 0, 1), new Vector3D(0.7, -0.7, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = 0, Pitch = 0, Roll = π }, new Vector3D(0, 0, 1), new Vector3D(0, -1, 0));

            ToEulerAngles_One(new EulerAngles { Yaw = π / 2, Pitch = π / 4, Roll = 0 }, new Vector3D(1, -1, 0), new Vector3D(1, 1, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = π / 2, Pitch = 0, Roll = π / 4 }, new Vector3D(1, 0, 0), new Vector3D(0, 0.7, 0.7));

            ToEulerAngles_One(new EulerAngles { Yaw = π, Pitch = 0, Roll = π }, new Vector3D(0, 0, -1), new Vector3D(0, -1, 0));
            ToEulerAngles_One(new EulerAngles { Yaw = 0, Pitch = -π / 2, Roll = π / 2 }, new Vector3D(0, 1, 0), new Vector3D(-1, 0, 0));
        }

        static void ToEulerAngles_One(EulerAngles expected, Vector3D rotatedUnitZ, Vector3D rotatedUnitY)
        {
            var actual = Rotation3DHelper.ToEulerAngles(rotatedUnitZ, rotatedUnitY);
            AssertEulerAngles(expected, actual);
        }

        [TestMethod]
        public void QuaternionToQuaternion_Many()
        {
            for (var i = 0; i < 1000; i++)
            {
                var expected = NextQuaternion();
                var actual = expected.ToEulerAngles().ToQuaternion();

                AssertQuaternion(expected, actual);
            }
        }

        static void AssertQuaternion(Quaternion expected, Quaternion actual)
        {
            if (expected.W * actual.W < 0)
                actual = actual.Negate();

            Assert.IsTrue(Math.Abs(expected.W - actual.W) < 0.001);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Y - actual.Y) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Z - actual.Z) < 0.001);
        }

        static void AssertEulerAngles(EulerAngles expected, EulerAngles actual)
        {
            Assert.IsTrue(Math.Abs(expected.Yaw - actual.Yaw) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Pitch - actual.Pitch) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Roll - actual.Roll) < 0.001);
        }
    }
}
