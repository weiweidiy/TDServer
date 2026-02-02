using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using JFramework.Game;
using JFramework;
using System.Runtime.InteropServices;

namespace JFramework.Game.Tests
{
    [TestFixture]
    public class JCombatUnitTests
    {
        private JCombatCasterTargetableUnit _combatUnit;
        private List<IUnique> _attributes;
        private GameAttributeInt _hpAttribute;
        private IJCombatAttrNameQuery _jCombatAttrNameQuery;

        [SetUp]
        public void Setup()
        {
            // Create a mock HP attribute
            _hpAttribute = new GameAttributeInt("Hp", 100, 100);

            // Create other mock attributes
            var strengthAttribute = Substitute.For<IUnique>();
            strengthAttribute.Uid.Returns("Strength");

            // Setup attributes list
            _attributes = new List<IUnique> { _hpAttribute, strengthAttribute };

            _jCombatAttrNameQuery = Substitute.For<IJCombatAttrNameQuery>();
            _jCombatAttrNameQuery.GetHpAttrName().Returns("Hp");

            // Create the combat unit
            _combatUnit = new JCombatCasterTargetableUnit("unit1", _attributes, attr => attr.Uid, _jCombatAttrNameQuery,null,null);
        }

        [Test]
        public void Constructor_InitializesWithAttributes_ShouldContainAllAttributes()
        {
            // Assert
            //Assert.AreEqual(2, _combatUnit.Count);
            Assert.IsNotNull(_combatUnit.Get("Hp"));
            Assert.IsNotNull(_combatUnit.Get("Strength"));
        }

        [Test]
        public void GetAttribute_WithExistingUid_ReturnsCorrectAttribute()
        {
            // Act
            var result = _combatUnit.GetAttribute("Hp");

            // Assert
            Assert.AreEqual(_hpAttribute, result);
        }

        [Test]
        public void GetAttribute_WithNonExistingUid_ReturnsNull()
        {
            // Act
            var result = _combatUnit.GetAttribute("NonExisting");

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void IsDead_WhenHpIsZero_ReturnsTrue()
        {
            // Arrange
            //_hpAttribute.CurValue.Returns(0);
            _hpAttribute.Minus(100);

            // Act & Assert
            Assert.IsTrue(_combatUnit.IsDead());
        }

        [Test]
        public void IsDead_WhenHpIsNegative_ReturnsTrue()
        {
            // Arrange
            _hpAttribute.Minus(200);

            // Act & Assert
            Assert.IsTrue(_combatUnit.IsDead());
        }

        [Test]
        public void IsDead_WhenHpIsPositive_ReturnsFalse()
        {
            // Arrange
            //_hpAttribute.CurValue.Returns(10);
            //_hpAttribute.Minus(100);

            // Act & Assert
            Assert.IsFalse(_combatUnit.IsDead());
        }

        [Test]
        public void IsDead_WhenHpAttributeMissing_ReturnsTrue()
        {
            // Arrange - create a combat unit without HP attribute
            var attributes = new List<IUnique>
            {
                Substitute.For<IUnique>()
            };
            attributes[0].Uid.Returns("Strength");

            var unitWithoutHp = new JCombatCasterTargetableUnit("unit1", attributes, attr => attr.Uid, _jCombatAttrNameQuery,null, null);

            // Act & Assert
            Assert.IsTrue(unitWithoutHp.IsDead());
        }

        //[Test]
        //public void Uid_Property_CanBeSetAndGet()
        //{
        //    // Arrange
        //    const string testUid = "TestUID123";

        //    // Act
        //    _combatUnit.Uid = testUid;

        //    // Assert
        //    Assert.AreEqual(testUid, _combatUnit.Uid);
        //}
    }
}