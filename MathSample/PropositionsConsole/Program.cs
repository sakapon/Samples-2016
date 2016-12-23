using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Propositions;
using static System.Console;
using static Blaze.Propositions.Formula;

namespace PropositionsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckTautology();
        }

        static void CheckTautology()
        {
            var p = Variable("P");
            var q = Variable("Q");
            var r = Variable("R");

            TestContradiction(p & !p);
            TestTautology(p | !p);
            TestTautology(Imply(p, !p));
            TestContradiction(Imply(p, !p));
            TestContradiction(Equivalent(p, !p));
            WriteLine();

            TestTautology(Equivalent(!(p & q), !p | !q));
            TestTautology(Equivalent(!(p | q), !p & !q));
            WriteLine();

            TestTautology(Imply(Imply(p, q), Imply(q, p)));
            TestContradiction(Imply(Imply(p, q), Imply(q, p)));
            TestTautology(Imply(p & Imply(p, q), q));
            WriteLine();

            WriteLine("三段論法：");
            TestTautology(Imply(Imply(p, q) & Imply(q, r), Imply(p, r)));
            WriteLine("背理法：");
            TestTautology(Imply(Imply(p, q) & Imply(p, !q), !p));
            WriteLine("対偶：");
            TestTautology(Equivalent(Imply(p, q), Imply(!q, !p)));
            WriteLine();
        }

        static void TestTautology(Formula f)
        {
            WriteLine($"{f} is{(f.IsTautology() ? "" : " not")} a tautology.");
        }

        static void TestContradiction(Formula f)
        {
            WriteLine($"{f} is{(f.IsContradiction() ? "" : " not")} a contradiction.");
        }
    }
}
