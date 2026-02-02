using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;

namespace JFramework.Game.Tests
{
    [TestFixture]
    public class JCombatJudgerTests
    {
        private IJCombatFrameRecorder _frameRecorder;
        private List<IJCombatTeam> _teams;
        private JCombatQuery _judger;

        [SetUp]
        public void Setup()
        {
            _frameRecorder = Substitute.For<IJCombatFrameRecorder>();
            _teams = new List<IJCombatTeam>
            {
                Substitute.For<IJCombatTeam>(),
                Substitute.For<IJCombatTeam>()
            };

            // Default setup - not max frame, teams alive
            _frameRecorder.IsMaxFrame().Returns(false);
            _teams[0].IsAllDead().Returns(false);
            _teams[1].IsAllDead().Returns(false);

            _judger = new JCombatQuery(_teams, team => team.GetHashCode().ToString(), _frameRecorder);

        }

        [Test]
        public void IsCombatOver_WhenMaxFrameReached_ReturnsTrue()
        {
            // Arrange
            _frameRecorder.IsMaxFrame().Returns(true);

            // Act
            var result = _judger.IsCombatOver();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(_judger.GetWinner());
        }

        [Test]
        public void IsCombatOver_WhenOneTeamAlive_ReturnsTrueAndSetsWinner()
        {
            // Arrange
            _teams[0].IsAllDead().Returns(true);
            _teams[1].IsAllDead().Returns(false);

            // Act
            var result = _judger.IsCombatOver();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(_teams[1], _judger.GetWinner());
        }

        [Test]
        public void IsCombatOver_WhenAllTeamsDead_ReturnsTrueWithNoWinner()
        {
            // Arrange
            _teams[0].IsAllDead().Returns(true);
            _teams[1].IsAllDead().Returns(true);

            // Act
            var result = _judger.IsCombatOver();

            // Assert
            Assert.IsTrue(result);
            Assert.IsNull(_judger.GetWinner());
        }

        [Test]
        public void IsCombatOver_WhenMultipleTeamsAlive_ReturnsFalseWithNoWinner()
        {
            // Arrange (default setup has both teams alive)

            // Act
            var result = _judger.IsCombatOver();

            // Assert
            Assert.IsFalse(result);
            Assert.IsNull(_judger.GetWinner());
        }

        [Test]
        public void GetWinner_BeforeCombatOver_ReturnsNull()
        {
            // Act
            var winner = _judger.GetWinner();

            // Assert
            Assert.IsNull(winner);
        }

        [Test]
        public void IsCombatOver_WithThreeTeams_WhenOneAlive_ReturnsTrueWithWinner()
        {
            // Arrange
            var thirdTeam = Substitute.For<IJCombatTeam>();
            _teams.Add(thirdTeam);

            _teams[0].IsAllDead().Returns(true);
            _teams[1].IsAllDead().Returns(true);
            thirdTeam.IsAllDead().Returns(false);

            // Need to recreate judger with the new team list
            _judger = new JCombatQuery(_teams, team => team.GetHashCode().ToString(), _frameRecorder);

            // Act
            var result = _judger.IsCombatOver();

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(thirdTeam, _judger.GetWinner());
        }
    }
}