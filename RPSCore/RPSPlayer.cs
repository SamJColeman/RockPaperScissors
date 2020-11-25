using System;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Contracts;
using RPSCore;
using Persistence;
using Persistence.Entities;

namespace RPSCore
{
    public class RPSPlayer : IRPSPlayer
    {
        private readonly IRPSGameStore rpsGameStore;
        private readonly IMoveMaker moveMaker;

        public RPSPlayer(IRPSGameStore rpsGameStore, IMoveMaker moveMaker)
        {
            this.rpsGameStore = rpsGameStore;
            this.moveMaker = moveMaker;
        }

        public async Task<string> GetReady(int numGames, int numDynamite)
        {
            if (await rpsGameStore.GetGame() != null)
            {
                throw new Exception(Constants.MatchAlreadyInProgress);
            }
            if (numGames <= 0 || numDynamite < 0)
            {
                throw new ArgumentException(Constants.InvalidParameters);
            }
            if (numDynamite > numGames)
            {
                throw new ArgumentException(Constants.InvalidNumberOfDynamite);
            }

            var rpsGame = new RPSGame(numGames, numDynamite);
            await rpsGameStore.Add(rpsGame);
            return Constants.GameSuccessfullySetup;
        }

        public async Task<Move> MakeMove()
        {
            var rpsGame = await rpsGameStore.GetGame();
            if (rpsGame == null)
            {
                throw new Exception(Constants.NoMatchInProgress);
            }

            if (rpsGame.Games.Count == rpsGame.NumberOfGames)
            {
                throw new Exception(Constants.NoGamesLeftToPlay);
            }

            if (rpsGame.Games.Any(g => g.OpponentMove == null))
            {
                throw new Exception(Constants.GameInProgress);
            }

            var move = moveMaker.MakeMove(rpsGame.NumberOfDynamite);

            rpsGame.AddGame(move);

            if (move == Move.Dynamite)
            {
                rpsGame.DynamiteUsed();
            }

            await rpsGameStore.Update(rpsGame);

            return move;
        }

        public async Task GameResult(Outcome yourOutcome, Move opponentMove)
        {
            var rpsGame = await rpsGameStore.GetGame();
            if (!rpsGame.Games.Any())
            {
                throw new Exception(Constants.NoGameProgress);
            }
            var game = rpsGame.Games.OrderBy(g => g.Id).Last();
            
            if (opponentMove == Move.Dynamite &&
                rpsGame.Games.Select(g => g.OpponentMove).Count(g => g == Move.Dynamite) + 1 > rpsGame.NumberOfDynamite)
            {
                throw new ArgumentException(string.Format(Constants.TooManyDynamite, rpsGame.NumberOfDynamite));
            }

            ValidateOutcome(yourOutcome, game.Move, opponentMove);
            game.UpdateOutcome(yourOutcome, opponentMove);
            await rpsGameStore.Update(rpsGame);
        }

        private void ValidateOutcome(Outcome yourOutcome, Move move, Move opponentMove)
        {
            var isValid = true;
            switch (move)
            {
                case Move.Rock:
                {
                    switch (opponentMove)
                    {
                        case Move.Rock:
                        {
                            isValid = yourOutcome == Outcome.Draw;
                            break;
                        }
                        case Move.Dynamite:
                        case Move.Paper:
                        {
                            isValid = yourOutcome == Outcome.Lose;
                            break;
                        }
                        default:
                        {
                            isValid = yourOutcome == Outcome.Win;
                            break;
                        }
                    }
                    break;
                }
                case Move.Paper:
                {
                    switch (opponentMove)
                    {
                        case Move.Paper:
                        {
                            isValid = yourOutcome == Outcome.Draw;
                            break;
                        }
                        case Move.Dynamite:
                        case Move.Scissors:
                        {
                            isValid = yourOutcome == Outcome.Lose;
                            break;
                        }
                        default:
                        {
                            isValid = yourOutcome == Outcome.Win;
                            break;
                        }
                    }
                    break;
                }
                case Move.Scissors:
                {
                    switch (opponentMove)
                    {
                        case Move.Scissors:
                        {
                            isValid = yourOutcome == Outcome.Draw;
                            break;
                        }
                        case Move.Dynamite:
                        case Move.Rock:
                        {
                            isValid = yourOutcome == Outcome.Lose;
                            break;
                        }
                        default:
                        {
                            isValid = yourOutcome == Outcome.Win;
                            break;
                        }
                    }
                    break;
                }
                case Move.Dynamite:
                {
                    switch (opponentMove)
                    {
                        case Move.Dynamite:
                        {
                            isValid = yourOutcome == Outcome.Draw;
                            break;
                        }
                        case Move.Warterbomb:
                        {
                            isValid = yourOutcome == Outcome.Lose;
                            break;
                        }
                        default:
                        {
                            isValid = yourOutcome == Outcome.Win;
                            break;
                        }
                    }
                    break;
                }
                case Move.Warterbomb:
                {
                    switch (opponentMove)
                    {
                        case Move.Warterbomb:
                        {
                            isValid = yourOutcome == Outcome.Draw;
                            break;
                        }
                        case Move.Dynamite:
                        {
                            isValid = yourOutcome == Outcome.Win;
                            break;
                        }
                        default:
                        {
                            isValid = yourOutcome == Outcome.Lose;
                            break;
                        }
                    }
                    break;
                }
            }

            if (!isValid)
            {
                throw new ArgumentException(Constants.InvalidOutcome);
            }
        }

        public async Task<string> Result(Outcome yourOutcome)
        {
            var rpsGame = await rpsGameStore.GetGame();

            if (rpsGame.Games.Count(g => g.Outcome != null) != rpsGame.NumberOfGames)
            {
                throw new Exception(Constants.NotAllGamesCompleted);
            }

            rpsGame.SetOverallResult(yourOutcome);
            await rpsGameStore.Update(rpsGame);

            switch (yourOutcome)
            {
                case Outcome.Win:
                    return Constants.OverallWin;
                case Outcome.Draw:
                    return Constants.OverallDraw;
            }

            return Constants.OverallLose;
        }
    }
}
