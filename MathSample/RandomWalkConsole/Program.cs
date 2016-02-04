using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RandomWalkConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var unfinished = Enumerable.Range(0, 1000)
                .Select(_ => Walk2())
                .Aggregate(0, (u, t) => u + (t.HasValue ? 0 : 1));
            Console.WriteLine(unfinished);
        }

        static readonly Random random = new Random();
        static readonly Int32Vector3[] Directions2 = new[] { Int32Vector3.XBasis, Int32Vector3.YBasis, -Int32Vector3.XBasis, -Int32Vector3.YBasis };

        static int? Walk2()
        {
            var current = Int32Vector3.Zero;

            for (var i = 1; i <= 1000000; i++)
            {
                current += Directions2[random.Next(0, Directions2.Length)];

                if (current == Int32Vector3.Zero) return i;
            }

            Console.WriteLine(current);
            return null;
        }
    }
}
