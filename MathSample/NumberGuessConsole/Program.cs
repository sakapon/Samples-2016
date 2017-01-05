using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blaze.Propositions;
using static System.Console;

namespace NumberGuessConsole
{
    static class Program
    {
        static void Main(string[] args)
        {
            var master = new GameMaster();

            master.Start(1, 2).WriteLines();
            WriteLine();
            master.Start(2, 3).WriteLines();
            WriteLine();
            master.Start(3, 4).WriteLines();
            WriteLine();

            master.Start(2, 1).WriteLines();
            WriteLine();
            master.Start(3, 2).WriteLines();
            WriteLine();
            master.Start(4, 3).WriteLines();
            WriteLine();

            master.Start(100, 101).WriteLines();
            WriteLine();
        }

        [DebuggerHidden]
        static void WriteLines(this IEnumerable<VariableFormula> source)
        {
            foreach (var v in source)
                WriteLine($"{v}: {v.Value}");
        }
    }
}
