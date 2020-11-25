using System.ComponentModel.DataAnnotations;
using Contracts;

namespace Persistence.Entities
{
    public class Game
    {
        [Key]
        public int Id { get; private set; }

        public Move Move { get; private set; }

        public Move? OpponentMove { get; private set; }

        public Outcome? Outcome { get; private set; }

        public Game(int id, Move move)
        {
            Id = id;
            Move = move;
        }

        public void UpdateOutcome(Outcome outcome, Move opponentMove)
        {
            Outcome = outcome;
            OpponentMove = opponentMove;
        }
    }
}