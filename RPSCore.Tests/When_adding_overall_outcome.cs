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
    public class When_adding_overall_outcome
    {
        private RPSPlayer rpsPlayer;
        private IRPSGameStore rpsGameStore;
        private IMoveMaker moveMaker;

        private static int numberOfGames = 1;
        private static int numberOfDynamite = 1;

        private readonly RPSGame rpsGame = new RPSGame(numberOfGames, numberOfDynamite);

        [SetUp]
        public void Setup()
        {
            rpsGameStore = Substitute.For<IRPSGameStore>();
            moveMaker = Substitute.For<IMoveMaker>();
            rpsGame.AddGame(Move.Rock);
            rpsGame.Games[0].UpdateOutcome(Outcome.Win, Move.Warterbomb);

            rpsGameStore.GetGame().Returns(rpsGame);

            rpsPlayer = new RPSPlayer(rpsGameStore, moveMaker);
        }

        [TestCase(Outcome.Win, Constants.OverallWin)]
        [TestCase(Outcome.Draw, Constants.OverallDraw)]
        [TestCase(Outcome.Lose, Constants.OverallLose)]
        public async Task Should_return_correct_message(Outcome outcome, string expectedMessage)
        {
            // Arrange

            // Act
            var result = await rpsPlayer.Result(outcome);

            // Assert
            result.Should().Be(expectedMessage);
            await rpsGameStore.Received(1).Update(Arg.Any<RPSGame>());
        }

        [Test]
        public async Task Should_save_overall_outcome()
        {
            // Arrange

            // Act
            var result = await rpsPlayer.Result(Outcome.Win);

            // Assert
            await rpsGameStore.Received(1).Update(Arg.Is<RPSGame>(game => game.OverallOutcome == Outcome.Win));
        }

        [Test]
        public async Task Should_throw_exception_if_not_all_games_completed()
        {
            // Arrange
            var newRpsGame = new RPSGame(10, 1);
            rpsGameStore.GetGame().Returns(newRpsGame);

            // Act && Assert
            rpsPlayer.Invoking(async r => await r.Result(Outcome.Win))
                .Should().Throw<Exception>().WithMessage(Constants.NotAllGamesCompleted);
        }
    }
}