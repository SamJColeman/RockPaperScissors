using System;
using System.Threading.Tasks;
using Contracts;
using FluentAssertions;
using NUnit.Framework;
using NSubstitute;
using Persistence;
using Persistence.Entities;

namespace RPSCore.Tests
{
    public class Tests
    {
        private IRPSGameStore rpsGameStore;
        private IMoveMaker moveMaker;
        private RPSPlayer rpsPlayer;

        [SetUp]
        public void Setup()
        {
            rpsGameStore = Substitute.For<IRPSGameStore>();
            moveMaker = Substitute.For<IMoveMaker>();
            rpsPlayer = new RPSPlayer(rpsGameStore, moveMaker);
        }

        [Test]
        public async Task Should_add_return_message_when_get_ready_called()
        {
            // Arrange

            // Act
            var result = await rpsPlayer.GetReady(10, 1);

            // Assert
            result.Should().Be(Constants.GameSuccessfullySetup);
            await rpsGameStore.Received(1).Add(Arg.Any<RPSGame>());
        }

        [Test]
        public void Should_throw_exception_when_num_dynamite_greater_than_num_games()
        {
            // Arrange

            // Act && Assert
            rpsPlayer.Invoking(async r => await r.GetReady(1, 10))
                .Should().Throw<Exception>().WithMessage(Constants.InvalidNumberOfDynamite);
        }

        [Test]
        public void Should_throw_exception_when_game_already_in_progress()
        {
            // Arrange
            rpsGameStore.GetGame().Returns(new RPSGame(1, 1));

            // Act && Assert
            rpsPlayer.Invoking(async r => await r.GetReady(1, 1))
                .Should().Throw<Exception>().WithMessage(Constants.MatchAlreadyInProgress);
        }
    }
}