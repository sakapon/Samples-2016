using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Randomization;

namespace PrisonersConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var bulb = new Bulb();
            var prisoners = Enumerable.Range(0, Prisoner.NumberOfPrisoners - 1)
                .Select(i => new Prisoner())
                .Concat(new[] { new CountingPrisoner() })
                .ToArray();

            var called = new HashSet<Prisoner>();

            for (var i = 0; ; i++)
            {
                var prisoner = prisoners.GetRandomElement();
                called.Add(prisoner);

                var answer = prisoner.Call(bulb);
                if (answer)
                {
                    var isCorrect = called.Count == Prisoner.NumberOfPrisoners;
                    Console.WriteLine($"{i} calls");
                    Console.WriteLine(isCorrect ? "OK" : "NG");
                    break;
                }
            }
        }
    }

    public class Bulb
    {
        public bool IsLighted { get; set; }
    }

    public class Prisoner
    {
        public const int NumberOfPrisoners = 100;

        bool hasLighted;

        public virtual bool Call(Bulb bulb)
        {
            if (bulb.IsLighted || hasLighted) return false;

            bulb.IsLighted = true;
            hasLighted = true;
            return false;
        }
    }

    public class CountingPrisoner : Prisoner
    {
        int count;

        public override bool Call(Bulb bulb)
        {
            if (!bulb.IsLighted) return false;

            bulb.IsLighted = false;
            count++;
            return count == NumberOfPrisoners - 1;
        }
    }
}
