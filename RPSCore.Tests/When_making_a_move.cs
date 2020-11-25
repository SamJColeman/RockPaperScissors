using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Persistence;
using Persistence.Entities;

namespace RPSCore.Tests
{
    [TestFixture]
    public class When_making_a_move
    {
        private RPSPlayer rpsPlayer;
        private IRPSGameStore rpsGameStore;
        private IMoveMaker moveMaker;

        private static int numberOfGames = 1;
        private static int numberOfDynamite = 1;

        [SetUp]
        public void Setup()
        {
            rpsGameStore = Substitute.For<IRPSGameStore>();
            moveMaker = Substitute.For<IMoveMaker>();

            rpsGameStore.GetGame().Returns(new RPSGame(numberOfGames, numberOfDynamite));
            moveMaker.MakeMove(Arg.Any<int>()).Returns(Move.Rock);

            rpsPlayer = new RPSPlayer(rpsGameStore, moveMaker);
        }

        [Test]
        public async Task Should_return_a_move()
        {
            // Arrange

            // Act
            var result = await rpsPlayer.MakeMove();

            // Assert
            result.Should().BeOfType(typeof(Move));
            await rpsGameStore.Received(1).Update(Arg.Any<RPSGame>());
        }

        [Test]
        public async Task Should_decrease_number_of_dynamites_used()
        {
            // Arrange
            moveMaker.MakeMove(Arg.Any<int>()).Returns(Move.Dynamite);

            // Act
            await rpsPlayer.MakeMove();

            // Assert
            (await rpsGameStore.GetGame()).NumberOfDynamite.Should().Be(numberOfDynamite - 1);
        }

        [Test]
        public void Should_throw_exception_if_no_match_in_progress()
        {
            // Arrange
            rpsGameStore.GetGame().ReturnsNull();

            // Act & Assert
            rpsPlayer.Invoking(async r => await r.MakeMove()).Should().Throw<Exception>().WithMessage(Constants.NoMatchInProgress);
        }

        [Test]
        public void Should_throw_exception_if_game_already_in_progress()
        {
            // Arrange
            var rpsGame = new RPSGame(2, 1);
            rpsGame.AddGame(Move.Rock);
            rpsGameStore.GetGame().Returns(rpsGame);

            // Act & Assert
            rpsPlayer.Invoking(async r => await r.MakeMove()).Should().Throw<Exception>().WithMessage(Constants.GameInProgress);
        }

        [Test]
        public void Should_throw_exception_if_no_games_left_to_play()
        {
            // Arrange
            var rpsGame = new RPSGame(1, 1);
            rpsGame.AddGame(Move.Rock);
            rpsGameStore.GetGame().Returns(rpsGame);

            // Act & Assert
            rpsPlayer.Invoking(async r => await r.MakeMove())
                .Should().Throw<Exception>().WithMessage(Constants.NoGamesLeftToPlay);
        }
    }
}