using Contracts;
using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace RPSCore.Tests
{
    public class When_generating_a_move
    {
        private MoveMaker moveMaker;

        [SetUp]
        public void Setup()
        {
            moveMaker = new MoveMaker();
        }

        [Test]
        public void Should_return_a_move()
        {
            // Arrange

            // Act
            var move = moveMaker.MakeMove(1);

            // Assert
            move.Should().BeOfType<Move>();
        }

        [Test]
        public void Should_return_at_more_rock_paper_scissors_than_waterbomb()
        {
            // Arrange
            var results = new List<Move>();

            // Act
            for (var i = 0; i < 10000; i++)
            {
                results.Add(moveMaker.MakeMove(0));
            }
            
            // Assert
            var rocks = results.Count(r => r == Move.Rock);
            var papers = results.Count(r => r == Move.Paper);
            var scissors = results.Count(r => r == Move.Scissors);
            var waterBombs = results.Count(r => r == Move.Warterbomb);

            waterBombs.Should().BeGreaterThan(0);
            waterBombs.Should().BeLessOrEqualTo(rocks);
            waterBombs.Should().BeLessOrEqualTo(papers);
            waterBombs.Should().BeLessOrEqualTo(scissors);
        }

        [Test]
        public void Should_return_at_more_rock_paper_scissors_and_dynamites_than_waterbomb()
        {
            // Arrange
            var results = new List<Move>();

            // Act
            for (var i = 0; i < 10000; i++)
            {
                results.Add(moveMaker.MakeMove(100));
            }
            
            // Assert
            var rocks = results.Count(r => r == Move.Rock);
            var papers = results.Count(r => r == Move.Paper);
            var scissors = results.Count(r => r == Move.Scissors);
            var dynamites = results.Count(r => r == Move.Dynamite);
            var waterBombs = results.Count(r => r == Move.Warterbomb);

            waterBombs.Should().BeGreaterThan(0);
            waterBombs.Should().BeLessOrEqualTo(rocks);
            waterBombs.Should().BeLessOrEqualTo(papers);
            waterBombs.Should().BeLessOrEqualTo(scissors);
            waterBombs.Should().BeLessOrEqualTo(dynamites);
        }
    }
}