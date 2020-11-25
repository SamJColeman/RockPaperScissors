using System;
using System.Collections.Generic;
using System.Linq;
using Contracts;

namespace RPSCore
{
    public class MoveMaker : IMoveMaker
    {
        private List<Move> possibleMoves = new List<Move>
        {
            Move.Paper,
            Move.Paper,
            Move.Paper,
            Move.Paper,
            Move.Paper,
            Move.Paper,
            Move.Paper,
            Move.Rock,
            Move.Rock,
            Move.Rock,
            Move.Rock,
            Move.Rock,
            Move.Rock,
            Move.Rock,
            Move.Scissors,
            Move.Scissors,
            Move.Scissors,
            Move.Scissors,
            Move.Scissors,
            Move.Scissors,
            Move.Scissors,
            Move.Warterbomb,
            Move.Warterbomb
        };

        public Move MakeMove(int dynamitesLeft)
        {
            if (dynamitesLeft > 0)
            {
                var minDynamites = Math.Min(dynamitesLeft, 14);
                possibleMoves.AddRange(Enumerable.Range(0, minDynamites).Select(r => Move.Dynamite));
            }
            possibleMoves.Shuffle();

            var rnd = new Random().Next(0, possibleMoves.Count);
            var move = possibleMoves[rnd];
            possibleMoves.RemoveAll(m => m == Move.Dynamite);
            return move;
        }
    }
}