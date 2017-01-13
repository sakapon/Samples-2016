using System;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RotationTest
{
    [TestClass]
    public class NRotationHelperTest
    {
        const float π = (float)Math.PI;

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
            // Special case: -π
            ToEulerAngles_One(new NEulerAngles { Yaw = 0, Pitch = 0, Roll = -π }, new Vector3(0, 0, 1), new Vector3(0, -1, 0));

            // Special case: -π
            ToEulerAngles_One(new NEulerAngles { Yaw = π, Pitch = 0, Roll = -π }, new Vector3(0, 0, -1), new Vector3(0, -1, 0));
        }

        static void ToEulerAngles_One(NEulerAngles expected, Vector3 rotatedUnitZ, Vector3 rotatedUnitY)
        {
            var actual = NRotationHelper.ToEulerAngles(rotatedUnitZ, rotatedUnitY);
            AssertEulerAngles(expected, actual);
        }

        static void AssertEulerAngles(NEulerAngles expected, NEulerAngles actual)
        {
            Assert.IsTrue(Math.Abs(expected.Yaw - actual.Yaw) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Pitch - actual.Pitch) < 0.001);
            Assert.IsTrue(Math.Abs(expected.Roll - actual.Roll) < 0.001);
        }
    }
}
