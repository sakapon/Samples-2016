using System;
using System.Numerics;
using Blaze.Randomization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RotationTest
{
    [TestClass]
    public class NRotationHelperTest
    {
        const float π = (float)Math.PI;

        static Vector3 NextVector3() => Vector3.Normalize(
            new Vector3((float)RandomHelper.NextDouble(-1, 1), (float)RandomHelper.NextDouble(-1, 1), (float)RandomHelper.NextDouble(-1, 1)));

        static Quaternion NextQuaternion() => Quaternion.Normalize(Quaternion.CreateFromAxisAngle(
            new Vector3((float)RandomHelper.NextDouble(-1, 1), (float)RandomHelper.NextDouble(-1, 1), (float)RandomHelper.NextDouble(-1, 1)),
            (float)RandomHelper.NextDouble(-7, 7)));

        [TestMethod]
        public void ToEulerAngles_01()
        {
            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = 0, Roll = 0 }, new Vector3(0, 0, 1), new Vector3(0, 1, 0));

            ToEulerAngles_One(new NEulerAngles { Yaw = π / 2, Pitch = 0, Roll = 0 }, new Vector3(1, 0, 0), new Vector3(0, 1, 0));
            ToEulerAngles_One(new NEulerAngles { Yaw = -π / 2, Pitch = 0, Roll = 0 }, new Vector3(-1, 0, 0), new Vector3(0, 1, 0));
            ToEulerAngles_One(new NEulerAngles { Yaw = -3 * π / 4, Pitch = 0, Roll = 0 }, new Vector3(-0.7f, 0, -0.7f), new Vector3(0, 1, 0));
            ToEulerAngles_One(new NEulerAngles { Yaw = π, Pitch = 0, Roll = 0 }, new Vector3(0, 0, -1), new Vector3(0, 1, 0));

            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = π / 2, Roll = 0 }, new Vector3(0, -1, 0), new Vector3(0, 0, 1));
            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = -π / 2, Roll = 0 }, new Vector3(0, 1, 0), new Vector3(0, 0, -1));

            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = 0, Roll = π / 2 }, new Vector3(0, 0, 1), new Vector3(-1, 0, 0));
            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = 0, Roll = -π / 2 }, new Vector3(0, 0, 1), new Vector3(1, 0, 0));
            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = 0, Roll = -3 * π / 4 }, new Vector3(0, 0, 1), new Vector3(0.7f, -0.7f, 0));
            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = 0, Roll = π }, new Vector3(0, 0, 1), new Vector3(0, -1, 0));

            ToEulerAngles_One(new NEulerAngles { Yaw = π / 2, Pitch = π / 4, Roll = 0 }, new Vector3(1, -1, 0), new Vector3(1, 1, 0));
            ToEulerAngles_One(new NEulerAngles { Yaw = π / 2, Pitch = 0, Roll = π / 4 }, new Vector3(1, 0, 0), new Vector3(0, 0.7f, 0.7f));

            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = -π / 2, Roll = π / 2 }, new Vector3(0, 1, 0), new Vector3(-1, 0, 0));
            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = π / 2, Roll = π }, new Vector3(0, -1, 0), new Vector3(0, 0, -1));
            ToEulerAngles_One(new NEulerAngles { Yaw = -π / 2, Pitch = 0, Roll = -π / 2 }, new Vector3(-1, 0, 0), new Vector3(0, 0, 1));
            ToEulerAngles_One(new NEulerAngles { Yaw = π, Pitch = 0, Roll = π }, new Vector3(0, 0, -1), new Vector3(0, -1, 0));
        }

        static void ToEulerAngles_One(NEulerAngles expected, Vector3 rotatedUnitZ, Vector3 rotatedUnitY)
        {
            var actual = NRotationHelper.ToEulerAngles(rotatedUnitZ, rotatedUnitY);
            AssertNEulerAngles(expected, actual);
        }

        [TestMethod]
        public void PointsToPoints_Many()
        {
            for (var i = 0; i < 1000; i++)
            {
                var rotatedUnitZ = NextVector3();
                var rotatedUnitY = NextOrthogonalVector3(rotatedUnitZ);

                var e = NRotationHelper.ToEulerAngles(rotatedUnitZ, rotatedUnitY);
                var m = Matrix4x4.CreateFromYawPitchRoll(e.Yaw, e.Pitch, e.Roll);
                AssertVector3(rotatedUnitZ, Vector3.Transform(Vector3.UnitZ, m));
                AssertVector3(rotatedUnitY, Vector3.Transform(Vector3.UnitY, m));
            }
        }

        static Vector3 NextOrthogonalVector3(Vector3 v)
        {
            var a0 = v.X == 0 ? Vector3.UnitX : new Vector3(v.Y, -v.X, 0);
            a0 = Vector3.Normalize(a0);
            var angle = (float)RandomHelper.NextDouble(-π, π);
            var q = Quaternion.CreateFromAxisAngle(v, angle);
            return Vector3.Transform(a0, q);
        }

        [TestMethod]
        public void QuaternionToQuaternion_Many()
        {
            for (var i = 0; i < 1000; i++)
            {
                var expected = NextQuaternion();
                var e = expected.ToEulerAngles();
                var actual = Quaternion.CreateFromYawPitchRoll(e.Yaw, e.Pitch, e.Roll);

                AssertQuaternion(expected, actual);
            }
        }

        static void AssertVector3(Vector3 expected, Vector3 actual)
        {
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Y - actual.Y) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Z - actual.Z) < 0.001);
        }

        static void AssertQuaternion(Quaternion expected, Quaternion actual)
        {
            if (expected.W * actual.W < 0)
                actual = -actual;

            Assert.IsTrue(Math.Abs(expected.W - actual.W) < 0.001);
            Assert.IsTrue(Math.Abs(expected.X - actual.X) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Y - actual.Y) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Z - actual.Z) < 0.001);
        }

        static void AssertNEulerAngles(NEulerAngles expected, NEulerAngles actual)
        {
            Assert.IsTrue(Math.Abs(expected.Yaw - actual.Yaw) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Pitch - actual.Pitch) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Roll - actual.Roll) < 0.001);
        }
    }
}
