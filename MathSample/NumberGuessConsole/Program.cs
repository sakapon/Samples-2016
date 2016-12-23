using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Blaze.Propositions;
using static System.Console;
using static Blaze.Propositions.Formula;

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

    public static class FormulaHelper
    {
        // formula の中から変数を取得します。なければ作成します。
        public static VariableFormula<TStatement> GetVariable<TStatement>(this Formula formula, TStatement statement) =>
            formula.GetDescendants()
                .OfType<VariableFormula<TStatement>>()
                .Distinct()
                .Where(v => Equals(v.Statement, statement))
                .FirstOrDefault() ?? Variable(statement);

        public static VariableFormula<TStatement> GetVariable<TStatement>(this Formula formula, VariableFormula<TStatement> variable) =>
            formula.GetVariable(variable.Statement);

        public static VariableFormula<TStatement> CreateProposition<TStatement>(TStatement statement, bool? truthValue)
        {
            var v = Variable(statement);
            v.Value = truthValue;
            return v;
        }
    }
}
