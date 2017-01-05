using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Propositions;
using static Blaze.Propositions.Formula;

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

                yield return Variable(new PlayerTurns(playerA.Id, i), resultA.HasValue);
                yield return Variable(new PlayerTurns(playerB.Id, i), resultB.HasValue);

                if (resultA.HasValue) yield return Variable(resultA.Value, true);
                if (resultB.HasValue) yield return Variable(resultB.Value, true);

                if (resultA.HasValue || resultB.HasValue) yield break;
            }
        }
    }

    public class GameSimulator
    {
        // 処理の最適化のため、必要となる結果だけを返します。
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

                if (targetResult.HasValue) return Variable(new PlayerTurns(targetId, i), true);
                if (opponentResult.HasValue) return Variable(new PlayerTurns(targetId.Opponent, i), true);

                if (targetNumber > opponentNumber && i == targetNumber - 2)
                    return Variable(new PlayerTurns(targetId, i), false);
            }
        }

        [Obsolete]
        public VariableFormula<PlayerTurns> Start_Stub(PlayerNumber[] numbers, PlayerIdentity targetId)
        {
            var targetNumber = numbers.First(n => n.PlayerId == targetId).Number;
            var opponentNumber = numbers.First(n => n.PlayerId == targetId.Opponent).Number;

            return targetNumber < opponentNumber ?
                Variable(new PlayerTurns(targetId, targetNumber), true) :
                Variable(new PlayerTurns(targetId, targetNumber - 2), false);
        }
    }
}
