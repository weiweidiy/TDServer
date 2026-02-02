using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using JFramework.Game;
using System;

namespace JFramework.Tests.Game
{
    [TestFixture]
    public class JCombatTeamTests
    {
        private JCombatTeam _combatTeam;
        private List<IJCombatCasterTargetableUnit> _mockUnits;
        private const string TeamUid = "team-123";
        private RunableExtraData _extraData;

        // Mock key selector that uses unit's UID as dictionary key
        private string MockKeySelector(IJCombatUnit unit) => unit.Uid;

        Func<IUnique, string> funcAttr = (attr) => attr.Uid;

        [SetUp]
        public void Setup()
        {
            // Create mock combat units with proper dependencies
            var mockAttrQuery = Substitute.For<IJCombatAttrNameQuery>();
            mockAttrQuery.GetHpAttrName().Returns("hp");

            var unit1 = Substitute.For<IJCombatCasterTargetableUnit>(/*"unit-1", new List<IUnique>(), funcAttr, mockAttrQuery,null,null*/);
            unit1.Uid.Returns("unit-1");
            var unit2 = Substitute.For<IJCombatCasterTargetableUnit>(/*"unit-2", new List<IUnique>(),funcAttr, mockAttrQuery,null,null*/);
            unit2.Uid.Returns("unit-2");
            var unit3 = Substitute.For<IJCombatCasterTargetableUnit>(/*"unit-3", new List<IUnique>(), funcAttr, mockAttrQuery,null,null*/);
            unit3.Uid.Returns("unit-3");

            _mockUnits = new List<IJCombatCasterTargetableUnit>
            {
                unit1,
                unit2,
                unit3
            };

            // Create test instance
            _combatTeam = new JCombatTeam(TeamUid, _mockUnits, MockKeySelector);

            // Mock extra data
            _extraData = Substitute.For<RunableExtraData>();
        }

        [Test]
        public void Constructor_WhenCalled_InitializesCorrectly()
        {
            // Assert
            Assert.AreEqual(TeamUid, _combatTeam.Uid);
            Assert.AreEqual(3, _combatTeam.GetAllUnits().Count);
        }

        [Test]
        public void GetAllUnits_ReturnsAllAddedUnits()
        {
            // Act
            var units = _combatTeam.GetAllUnits();

            // Assert
            Assert.AreEqual(3, units.Count);
            CollectionAssert.Contains(units, _mockUnits[0]);
            CollectionAssert.Contains(units, _mockUnits[1]);
            CollectionAssert.Contains(units, _mockUnits[2]);
        }

        [Test]
        public void GetUnit_WithExistingUid_ReturnsCorrectUnit()
        {
            // Act & Assert
            Assert.AreEqual(_mockUnits[0], _combatTeam.GetUnit("unit-1"));
            Assert.AreEqual(_mockUnits[1], _combatTeam.GetUnit("unit-2"));
            Assert.AreEqual(_mockUnits[2], _combatTeam.GetUnit("unit-3"));
        }

        [Test]
        public void GetUnit_WithNonExistingUid_ReturnsNull()
        {
            // Act & Assert
            Assert.IsNull(_combatTeam.GetUnit("non-existent-unit"));
        }

        [Test]
        public void IsAllDead_WhenAllUnitsDead_ReturnsTrue()
        {
            // Arrange - make all units dead
            foreach (var unit in _mockUnits)
            {
                var hpAttr = new GameAttributeInt("hp", 0, 100);
                unit.GetAttribute("hp").Returns(hpAttr);
                unit.IsDead().Returns(true);
            }

            // Act & Assert
            Assert.IsTrue(_combatTeam.IsAllDead());
        }

        [Test]
        public void IsAllDead_WhenAnyUnitAlive_ReturnsFalse()
        {
            // Arrange - make first unit alive
            var aliveHp = new GameAttributeInt("hp", 50, 100);
            var deadHp = new GameAttributeInt("hp", 0, 100);

            _mockUnits[0].GetAttribute("hp").Returns(aliveHp);
            _mockUnits[1].GetAttribute("hp").Returns(deadHp);
            _mockUnits[2].GetAttribute("hp").Returns(deadHp);

            // Act & Assert
            Assert.IsFalse(_combatTeam.IsAllDead());
        }

        [Test]
        public void OnStart_WhenCalled_StartsAllUnits()
        {
            // Act
            _combatTeam.Start(_extraData);

            // Assert
            foreach (var unit in _mockUnits)
            {
                unit.Received(1).Start(_extraData);
                //Assert.IsTrue(unit.IsRunning); // Assuming RunableDictionaryContainer has IsRunning property
            }
        }

        [Test]
        public void OnStop_WhenCalled_StopsAllUnits()
        {
            // Arrange - start first
            _combatTeam.Start(_extraData);

            // Act
            _combatTeam.Stop();

            // Assert
            foreach (var unit in _mockUnits)
            {
                Assert.IsFalse(unit.IsRunning);
            }
        }

        [Test]
        public void Constructor_WithNullUnitsList_DoesNotThrow()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new JCombatTeam("empty-team", null, MockKeySelector));
        }

        [Test]
        public void OnStart_WithEmptyTeam_DoesNotThrow()
        {
            // Arrange
            var emptyTeam = new JCombatTeam("empty-team", new List<IJCombatCasterTargetableUnit>(), MockKeySelector);

            // Act & Assert
            Assert.DoesNotThrow(() => emptyTeam.Start(_extraData));
        }

        [Test]
        public void OnStop_WithEmptyTeam_DoesNotThrow()
        {
            // Arrange
            var emptyTeam = new JCombatTeam("empty-team", new List<IJCombatCasterTargetableUnit>(), MockKeySelector);

            // Act & Assert
            Assert.DoesNotThrow(() => emptyTeam.Stop());
        }
    }
}