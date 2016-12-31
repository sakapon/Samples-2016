using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Randomization;

namespace MontyHallConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 1000;
            var wins = Enumerable.Range(0, count)
                .Count(i => MontyHall.Execute());
            Console.WriteLine($"{(double)wins / count:P2}");
        }
    }

    public static class MontyHall
    {
        const int Doors = 3;
        static readonly int[] Indexes = Enumerable.Range(0, Doors).ToArray();

        public static bool Execute()
        {
            var answer = Indexes.GetRandomElement();
            var choice1 = Indexes.GetRandomElement();

            var candidatesToOpen = Indexes
                .Where(i => i != choice1)
                .Where(i => i != answer)
                .ToArray();
            var open = candidatesToOpen.Shuffle()
                .Take(Doors - 2)
                .ToArray();

            var choice2 = Indexes
                .Except(open)
                .Where(i => i != choice1)
                .Single();
            return answer == choice2;
        }
    }
}
