using System.Threading.Tasks;
using Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using RPSCore;
using RPSWebApi.Controllers;

namespace RPSWebApi.Tests
{
    [TestFixture]
    public class When_using_game_controller
    {
        private GameController gameController;
        private IRPSPlayer rpsPlayer;

        [SetUp]
        public void Setup()
        {
            rpsPlayer = Substitute.For<IRPSPlayer>();
            rpsPlayer.GetReady(Arg.Any<int>(), Arg.Any<int>()).Returns(Constants.GameSuccessfullySetup);
            rpsPlayer.MakeMove().Returns(Move.Rock);
            rpsPlayer.Result(Arg.Any<Outcome>()).Returns(Constants.OverallWin);
            gameController = new GameController(rpsPlayer);
        }

        [Test]
        public async Task Should_return_message_for_game_ready()
        {
            // Arrange

            // Act
            var response = await gameController.GetReady(1, 1);

            // Assert
            response.Result.Should().BeOfType<CreatedResult>();
            response.Result.As<CreatedResult>().Value.Should().Be(Constants.GameSuccessfullySetup);
        }

        [Test]
        public async Task Should_return_move()
        {
            // Arrange

            // Act
            var response = await gameController.GetMove();

            // Assert
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>().Value.Should().Be(Move.Rock);
        }

        [Test]
        public async Task Should_add_game_outcome()
        {
            // Arrange

            // Act
            var response = await gameController.AddGameResult(Outcome.Win, Move.Rock);

            // Assert
            response.Should().BeOfType<OkResult>();
        }

        [Test]
        public async Task Should_return_overall_message()
        {
            // Arrange

            // Act
            var response = await gameController.AddResult(Outcome.Win);

            // Assert
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>().Value.Should().Be(Constants.OverallWin);
        }
    }
}