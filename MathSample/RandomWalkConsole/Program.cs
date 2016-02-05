using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;
using static System.Math;

namespace RandomWalkConsole
{
    class Program
    {
        const int Trials = 1 * 1000;
        const int MaxSteps = 100 * 1000 * 1000;
        const int MaxDistance = 10 * 1000;

        static readonly Int32Vector3[] Directions2 = new[] { Int32Vector3.XBasis, Int32Vector3.YBasis, -Int32Vector3.XBasis, -Int32Vector3.YBasis };
        static readonly Int32Vector3[] Directions3 = new[] { Int32Vector3.XBasis, Int32Vector3.YBasis, Int32Vector3.ZBasis, -Int32Vector3.XBasis, -Int32Vector3.YBasis, -Int32Vector3.ZBasis };

        static readonly Random RandomMaster = new Random();

        static Random NextRandomizer()
        {
            lock (RandomMaster)
            {
                return new Random(RandomMaster.Next());
            }
        }

        static void Main(string[] args)
        {
            //TryWalks();
            TryWalksInParallel();
        }

        static void TryWalks()
        {
            var returned = Enumerable.Range(0, Trials)
                .Select(_ => Walk(Directions2, RandomMaster))
                .Aggregate(0, (u, t) => u + (t.HasValue ? 1 : 0));
            WriteLine($"Returned Rate: {(double)returned / Trials}");
        }

        static void TryWalksInParallel()
        {
            var returned = 0;
            var returnedLock = new object();

            Parallel.For(0, Trials, _ =>
            {
                var steps = Walk(Directions2, NextRandomizer());
                if (!steps.HasValue) return;
                lock (returnedLock)
                {
                    returned++;
                }
            });

            WriteLine($"Returned Rate: {(double)returned / Trials}");
        }

        static int? Walk(Int32Vector3[] directions, Random random)
        {
            var current = Int32Vector3.Zero;

            for (var i = 1; i <= MaxSteps; i++)
            {
                current += directions[random.Next(0, directions.Length)];

                if (current == Int32Vector3.Zero)
                {
                    WriteLine($"{i:N0}");
                    return i;
                }
                else if (Abs(current.X) >= MaxDistance || Abs(current.Y) >= MaxDistance || Abs(current.Z) >= MaxDistance)
                {
                    WriteLine(current);
                    return null;
                }
            }

            WriteLine(current);
            return null;
        }
    }
}
