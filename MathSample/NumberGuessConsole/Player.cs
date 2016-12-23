using System;
using System.Collections.Generic;
using System.Linq;
using Blaze.Propositions;
using static Blaze.Propositions.Formula;

namespace NumberGuessConsole
{
    public abstract class PlayerBase
    {
        public PlayerIdentity Id { get; }

        protected PlayerBase(PlayerIdentity id)
        {
            Id = id;
        }

        public override string ToString() => $"{Id}";

        protected VariableFormula<PlayerNumber>[] Candidates { get; set; }
        protected Formula Knowledge { get; set; }

        public PlayerNumber? Guess(int turns)
        {
            if (turns > 1)
            {
                var previous = Knowledge.GetVariable(new PlayerTurns(Id.Opponent, turns - 1));
                // 本来は
                // Knowledge &= !previous;
                // ですが、処理の最適化のため、真偽値を代入します。
                previous.Value = false;
            }

            foreach (var v in Candidates)
            {
                Knowledge.Determine(v);
                if (v.Value == true)
                    return v.Statement;
            }

            return null;
        }
    }

    public class Player : PlayerBase
    {
        public Player(PlayerIdentity id) : base(id) { }

        public void Initialize(int selfNumber)
        {
            var self = new PlayerNumber(Id, selfNumber);
            Candidates = GameRules.GetCandidates(self).Select(Variable).ToArray();
            Knowledge = Xor(Candidates);

            if (Candidates.Length <= 1) return;

            var simulator = new GameSimulator();
            foreach (var v in Candidates)
            {
                var simulationResult = simulator.Start(new[] { self, v.Statement }, Id.Opponent);
                var r = Knowledge.GetVariable(simulationResult);
                Knowledge &= Imply(v, simulationResult.Value == true ? r : !r);
            }
        }
    }

    public class SimulationPlayer : PlayerBase
    {
        public SimulationPlayer(PlayerIdentity id) : base(id) { }

        public void Initialize(PlayerNumber[] numbers, GameSimulator simulator)
        {
            var self = numbers.First(n => n.PlayerId == Id);
            var opponent = numbers.First(n => n.PlayerId == Id.Opponent);
            Candidates = GameRules.GetCandidates(self).Select(Variable).ToArray();
            Knowledge = Xor(Candidates);

            if (Candidates.Length <= 1) return;

            // 特定のプレイヤーの思考の中で無限ループになる場合、「わからない」と結論付けます (知識を追加しない)。
            foreach (var v in Candidates.Where(c => c.Statement.Number < opponent.Number))
            {
                var simulationResult = simulator.Start(new[] { self, v.Statement }, Id.Opponent);
                var r = Knowledge.GetVariable(simulationResult);
                Knowledge &= Imply(v, simulationResult.Value == true ? r : !r);
            }
        }
    }
}
