using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Propositions;
using static Blaze.Propositions.Formula;

namespace PropositionsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = Variable("P");
            var q = Variable("Q");
            var r = Variable("R");

            TestContradiction(p & !p);
            TestTautology(p | !p);
            TestTautology(Imply(p, !p));
            TestContradiction(Imply(p, !p));
            TestContradiction(Equivalent(p, !p));
        }

        static void TestTautology(Formula f)
        {
            Console.WriteLine($"{f} is{(f.IsTautology() ? "" : " not")} a tautology.");
        }

        static void TestContradiction(Formula f)
        {
            Console.WriteLine($"{f} is{(f.IsContradiction() ? "" : " not")} a contradiction.");
        }
    }
}
