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
            Console.WriteLine();

            TestTautology(Equivalent(!(p & q), !p | !q));
            TestTautology(Equivalent(!(p | q), !p & !q));
            Console.WriteLine();

            TestTautology(Imply(Imply(p, q), Imply(q, p)));
            TestContradiction(Imply(Imply(p, q), Imply(q, p)));
            TestTautology(Imply(p & Imply(p, q), q));
            Console.WriteLine();

            Console.WriteLine("三段論法：");
            TestTautology(Imply(Imply(p, q) & Imply(q, r), Imply(p, r)));
            Console.WriteLine("背理法：");
            TestTautology(Imply(Imply(p, q) & Imply(p, !q), !p));
            Console.WriteLine("対偶：");
            TestTautology(Equivalent(Imply(p, q), Imply(!q, !p)));
            Console.WriteLine();
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
