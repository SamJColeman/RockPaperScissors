using Contracts;
using System;
using System.Collections.Generic;

namespace Persistence.Entities
{
    public class RPSGame
    {
        public Guid Id { get; private set; }

        public int NumberOfGames { get; private set; }

        public int NumberOfDynamite { get; private set; }

        public List<Game> Games { get; } = new List<Game>();

        public Outcome? OverallOutcome { get; private set; }

        private int gamesPlayed = 0;

        public RPSGame(int numberOfGames, int numberOfDynamite)
        {
            Id = new Guid();
            NumberOfGames = numberOfGames;
            NumberOfDynamite = numberOfDynamite;
        }

        public void AddGame(Move move)
        {
            Games.Add(new Game(gamesPlayed++, move));
        }

        public void DynamiteUsed()
        {
            NumberOfDynamite--;
        }

        public void SetOverallResult(Outcome yourOutcome)
        {
            OverallOutcome = yourOutcome;
        }
    }
}