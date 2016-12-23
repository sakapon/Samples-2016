using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Propositions;

namespace NumberGuessConsole
{
    public class GameMaster
    {
        public IEnumerable<VariableFormula> Start(int numberA, int numberB)
        {
            var playerA = new Player(PlayerIdentity.A);
            var playerB = new Player(PlayerIdentity.B);

            playerA.Initialize(numberA);
            playerB.Initialize(numberB);

            for (var i = 1; ; i++)
            {
                var resultA = playerA.Guess(i);
                var resultB = playerB.Guess(i);

                yield return FormulaHelper.CreateProposition(new PlayerTurns(playerA.Id, i), resultA.HasValue);
                yield return FormulaHelper.CreateProposition(new PlayerTurns(playerB.Id, i), resultB.HasValue);

                if (resultA.HasValue) yield return FormulaHelper.CreateProposition(resultA.Value, true);
                if (resultB.HasValue) yield return FormulaHelper.CreateProposition(resultB.Value, true);

                if (resultA.HasValue || resultB.HasValue) yield break;
            }
        }
    }

    public class GameSimulator
    {
        public VariableFormula<PlayerTurns> Start(PlayerNumber[] numbers, PlayerIdentity targetId)
        {
            var targetNumber = numbers.First(n => n.PlayerId == targetId).Number;
            var opponentNumber = numbers.First(n => n.PlayerId == targetId.Opponent).Number;

            var targetPlayer = new SimulationPlayer(targetId);
            var opponentPlayer = new SimulationPlayer(targetId.Opponent);

            targetPlayer.Initialize(numbers, this);
            opponentPlayer.Initialize(numbers, this);

            for (var i = 1; ; i++)
            {
                var targetResult = targetPlayer.Guess(i);
                var opponentResult = opponentPlayer.Guess(i);

                if (targetResult.HasValue) return FormulaHelper.CreateProposition(new PlayerTurns(targetId, i), true);
                if (opponentResult.HasValue) return FormulaHelper.CreateProposition(new PlayerTurns(targetId.Opponent, i), true);

                if (targetNumber > opponentNumber && i == targetNumber - 2)
                    return FormulaHelper.CreateProposition(new PlayerTurns(targetId, i), false);
            }
        }

        [Obsolete]
        public VariableFormula<PlayerTurns> Start_Stub(PlayerNumber[] numbers, PlayerIdentity targetId)
        {
            var targetNumber = numbers.First(n => n.PlayerId == targetId).Number;
            var opponentNumber = numbers.First(n => n.PlayerId == targetId.Opponent).Number;

            return targetNumber < opponentNumber ?
                FormulaHelper.CreateProposition(new PlayerTurns(targetId, targetNumber), true) :
                FormulaHelper.CreateProposition(new PlayerTurns(targetId, targetNumber - 2), false);
        }
    }
}
