using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Propositions;
using static Blaze.Propositions.Formula;

namespace NumberGuessConsole
{
    class Program
    {
        static void Main(string[] args)
        {
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
