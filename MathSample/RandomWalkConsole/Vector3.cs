using System;

namespace RandomWalkConsole
{
    public struct Int32Vector3
    {
        public static Int32Vector3 Zero { get; } = new Int32Vector3();
        public static Int32Vector3 XBasis { get; } = new Int32Vector3(1, 0, 0);
        public static Int32Vector3 YBasis { get; } = new Int32Vector3(0, 1, 0);
        public static Int32Vector3 ZBasis { get; } = new Int32Vector3(0, 0, 1);

        public int X { get; }
        public int Y { get; }
        public int Z { get; }

        public Int32Vector3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Int32Vector3 operator +(Int32Vector3 v1, Int32Vector3 v2) => new Int32Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        public static Int32Vector3 operator -(Int32Vector3 v1, Int32Vector3 v2) => new Int32Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        public static Int32Vector3 operator +(Int32Vector3 v1) => v1;
        public static Int32Vector3 operator -(Int32Vector3 v1) => new Int32Vector3(-v1.X, -v1.Y, -v1.Z);

        public static bool operator ==(Int32Vector3 v1, Int32Vector3 v2) => v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
        public static bool operator !=(Int32Vector3 v1, Int32Vector3 v2) => !(v1 == v2);
        public override bool Equals(object obj) => obj is Int32Vector3 && this == (Int32Vector3)obj;
        public override int GetHashCode() => X ^ Y ^ Z;
        public override string ToString() => $"({X}, {Y}, {Z})";
    }
}
