using System;
using System.Diagnostics;

namespace NumberGuessConsole
{
    public static class GameRules
    {
        public static PlayerNumber[] GetCandidates(PlayerNumber number) =>
            number.Number == 1 ?
            new[] { number.ToOpponent() + 1 } :
            new[] { number.ToOpponent() - 1, number.ToOpponent() + 1 };
    }

    [DebuggerStepThrough]
    public class PlayerIdentity
    {
        public static PlayerIdentity A { get; } = new PlayerIdentity("A");
        public static PlayerIdentity B { get; } = new PlayerIdentity("B");
        public static PlayerIdentity[] Both { get; } = new[] { A, B };

        static PlayerIdentity()
        {
            A.Opponent = B;
            B.Opponent = A;
        }

        public string Name { get; }
        public PlayerIdentity Opponent { get; private set; }

        PlayerIdentity(string name)
        {
            Name = name;
        }

        public override string ToString() => Name;
    }

    [DebuggerStepThrough]
    public struct PlayerNumber
    {
        public PlayerIdentity PlayerId { get; }
        public int Number { get; }

        public PlayerNumber(PlayerIdentity playerId, int number)
        {
            PlayerId = playerId;
            Number = number;
        }

        public PlayerNumber ToOpponent() => new PlayerNumber(PlayerId.Opponent, Number);

        public static PlayerNumber operator +(PlayerNumber v, int delta) => new PlayerNumber(v.PlayerId, v.Number + delta);
        public static PlayerNumber operator -(PlayerNumber v, int delta) => new PlayerNumber(v.PlayerId, v.Number - delta);

        public static bool operator ==(PlayerNumber v1, PlayerNumber v2) => v1.PlayerId == v2.PlayerId && v1.Number == v2.Number;
        public static bool operator !=(PlayerNumber v1, PlayerNumber v2) => !(v1 == v2);
        public override bool Equals(object obj) => obj is PlayerNumber && this == (PlayerNumber)obj;
        public override int GetHashCode() => PlayerId.GetHashCode() ^ Number.GetHashCode();
        public override string ToString() => $"{PlayerId} = {Number}";
    }

    [DebuggerStepThrough]
    public struct PlayerTurns
    {
        public PlayerIdentity PlayerId { get; }
        public int Turns { get; }

        public PlayerTurns(PlayerIdentity playerId, int turns = 0)
        {
            PlayerId = playerId;
            Turns = turns;
        }

        public PlayerTurns ToOpponent() => new PlayerTurns(PlayerId.Opponent, Turns);

        public static PlayerTurns operator +(PlayerTurns v, int delta) => new PlayerTurns(v.PlayerId, v.Turns + delta);
        public static PlayerTurns operator -(PlayerTurns v, int delta) => new PlayerTurns(v.PlayerId, v.Turns - delta);

        public static bool operator ==(PlayerTurns v1, PlayerTurns v2) => v1.PlayerId == v2.PlayerId && v1.Turns == v2.Turns;
        public static bool operator !=(PlayerTurns v1, PlayerTurns v2) => !(v1 == v2);
        public override bool Equals(object obj) => obj is PlayerTurns && this == (PlayerTurns)obj;
        public override int GetHashCode() => PlayerId.GetHashCode() ^ Turns.GetHashCode();
        public override string ToString() => $"{PlayerId} @ {Turns}";
    }
}
