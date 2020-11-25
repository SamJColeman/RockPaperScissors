using System;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Persistence;
using Persistence.Entities;

namespace RPSCore.Tests
{
    [TestFixture]
    public class When_adding_game_outcome
    {
        private RPSPlayer rpsPlayer;
        private IRPSGameStore rpsGameStore;
        private IMoveMaker moveMaker;

        private static int numberOfGames = 10;
        private static int numberOfDynamite = 10;

        private readonly RPSGame rpsGame = new RPSGame(numberOfGames, numberOfDynamite);

        [SetUp]
        public void Setup()
        {
            rpsGameStore = Substitute.For<IRPSGameStore>();
            moveMaker = Substitute.For<IMoveMaker>();

            rpsGame.AddGame(Move.Paper);
            rpsGameStore.GetGame().Returns(rpsGame);
            moveMaker.MakeMove(Arg.Any<int>()).Returns(Move.Rock);

            rpsPlayer = new RPSPlayer(rpsGameStore, moveMaker);
        }

        [Test]
        public async Task Should_save_outcome()
        {
            // Arrange

            // Act
            await rpsPlayer.GameResult(Outcome.Win, Move.Rock);

            // Assert
            await rpsGameStore.Received(1).Update(Arg.Is<RPSGame>(game =>
                game.Games.FirstOrDefault(g => g.Outcome == Outcome.Win && g.OpponentMove == Move.Rock) != null));
        }

        [Test]
        public void Should_throw_exception_if_no_game_in_progress()
        {
            // Arrange
            var newRpsGame = new RPSGame(10, 0);
            rpsGameStore.GetGame().Returns(newRpsGame);

            // Act && Assert
            rpsPlayer.Invoking(async r => await r.GameResult(Outcome.Win, Move.Rock))
                .Should().Throw<Exception>().WithMessage(Constants.NoGameProgress);
        }

        [Test]
        public void Should_throw_exception_if_opponent_dynamites_greater_than_allowed()
        {
            // Arrange
            var newRpsGame = new RPSGame(10, 0);
            newRpsGame.AddGame(Move.Paper);
            rpsGameStore.GetGame().Returns(newRpsGame);

            // Act && Assert
            rpsPlayer.Invoking(async r => await r.GameResult(Outcome.Win, Move.Dynamite))
                .Should().Throw<ArgumentException>().WithMessage(string.Format(Constants.TooManyDynamite, 0));
        }

        [TestCase(Outcome.Lose, Move.Rock, Move.Scissors)]
        [TestCase(Outcome.Lose, Move.Scissors, Move.Paper)]
        [TestCase(Outcome.Lose, Move.Paper, Move.Rock)]
        [TestCase(Outcome.Lose, Move.Dynamite, Move.Scissors)]
        public void Should_throw_exception_if_outcome_doesnt_match_moves(Outcome yourOutcome, Move move, Move opponentMove)
        {
            // Arrange
            var newRpsGame = new RPSGame(10, 0);
            newRpsGame.AddGame(move);
            rpsGameStore.GetGame().Returns(newRpsGame);

            // Act & Assert
            rpsPlayer.Invoking(async r => await r.GameResult(yourOutcome, opponentMove)).Should().Throw<ArgumentException>()
                .WithMessage(Constants.InvalidOutcome);
        }
    }
}