using NUnit.Framework;
using NSubstitute;
using JFramework;
using System.Collections.Generic;
using System;

namespace JFramework.Game.Tests
{
    [TestFixture]
    public class JCombatSpeedBasedActionSelectorTests
    {
        private JCombatTurnBasedActionSelector _selector;
        private Func<IJAttributeable, string> _keySelector;
        private IJCombatTurnBasedUnit _unit1;
        private IJCombatTurnBasedUnit _unit2;
        private IJCombatTurnBasedUnit _unit3;

        [SetUp]
        public void SetUp()
        {
            _keySelector = unit => unit.GetHashCode().ToString();

            // 创建模拟战斗单位
            _unit1 = Substitute.For<IJCombatTurnBasedUnit>();
            //_unit1.GetHashCode().Returns(1);
            _unit1.GetActionPoint().Returns(10);
            _unit1.CanCast().Returns(true);

            _unit2 = Substitute.For<IJCombatTurnBasedUnit>();
            //_unit2.GetHashCode().Returns(2);
            _unit2.GetActionPoint().Returns(20);
            _unit2.CanCast().Returns(true);

            _unit3 = Substitute.For<IJCombatTurnBasedUnit>();
            //_unit3.GetHashCode().Returns(3);
            _unit3.GetActionPoint().Returns(15);
            _unit3.CanCast().Returns(true);

            _selector = new JCombatTurnBasedActionSelector(new List<IJCombatTurnBasedUnit>() { },  _keySelector);
        }

        [Test]
        public void SetUnits_ShouldSortUnitsByActionPointDescending()
        {
            // Arrange
            var units = new List<IJCombatTurnBasedUnit> { _unit1, _unit2, _unit3 };

            // Act
            _selector.AddUnits(units);
            var actionUnits = _selector.GetActionUnits();

            // Assert
            Assert.AreEqual(3, actionUnits.Count);
            Assert.AreEqual(_unit2, actionUnits[0]); // 最高行动点
            Assert.AreEqual(_unit3, actionUnits[1]);
            Assert.AreEqual(_unit1, actionUnits[2]); // 最低行动点
        }

        [Test]
        public void PopActionUnit_ShouldReturnAndRemoveFirstUnit()
        {
            // Arrange
            var units = new List<IJCombatTurnBasedUnit> { _unit1, _unit2, _unit3 };
            _selector.AddUnits(units);

            // Act
            var firstUnit = _selector.PopActionUnit();
            var remainingUnits = _selector.GetActionUnits();

            // Assert
            Assert.AreEqual(_unit2, firstUnit); // 最高行动点的单位
            Assert.AreEqual(2, remainingUnits.Count);
            Assert.AreEqual(_unit3, remainingUnits[0]);
            Assert.AreEqual(_unit1, remainingUnits[1]);
        }

        [Test]
        public void IsAllComplete_ShouldReturnFalse_WhenAnyUnitCanAction()
        {
            // Arrange
            var units = new List<IJCombatTurnBasedUnit> { _unit1, _unit2, _unit3 };
            _selector.AddUnits(units);

            // Act & Assert
            Assert.IsFalse(_selector.IsAllComplete());
        }

        [Test]
        public void IsAllComplete_ShouldReturnTrue_WhenNoUnitCanAction()
        {
            // Arrange
            _unit1.CanCast().Returns(false);
            _unit2.CanCast().Returns(false);
            _unit3.CanCast().Returns(false);

            var units = new List<IJCombatTurnBasedUnit> { _unit1, _unit2, _unit3 };
            _selector.AddUnits(units);

            // Act & Assert
            Assert.IsTrue(_selector.IsAllComplete());
        }

        [Test]
        public void PopActionUnit_ShouldThrow_WhenListIsEmpty()
        {
            // Arrange
            var emptyList = new List<IJCombatTurnBasedUnit>();
            _selector.AddUnits(emptyList);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _selector.PopActionUnit());
        }

        [Test]
        public void ResetActionUnits_ShouldResortAndResetActionList()
        {
            // Arrange
            var units = new List<IJCombatTurnBasedUnit> { _unit1, _unit2, _unit3 };
            _selector.AddUnits(units);

            // 修改单位行动点以改变排序顺序
            _unit1.GetActionPoint().Returns(30); // 现在 unit1 应该排在最前面
            _unit2.GetActionPoint().Returns(5);  // 现在 unit2 应该排在最后面

            // 先弹出几个单位改变actionList状态
            _selector.PopActionUnit(); // 弹出原来的第一个(_unit2)
            _selector.PopActionUnit(); // 弹出原来的第二个(_unit3)

            // Act
            _selector.ResetActionUnits();
            var actionUnits = _selector.GetActionUnits();

            // Assert
            Assert.AreEqual(3, actionUnits.Count); // 确认重置后包含所有单位
            Assert.AreEqual(_unit1, actionUnits[0]); // 现在unit1行动点最高(30)
            Assert.AreEqual(_unit3, actionUnits[1]); // unit3行动点保持15
            Assert.AreEqual(_unit2, actionUnits[2]); // unit2行动点最低(5)
        }

        [Test]
        public void ResetActionUnits_ShouldHandleEmptyList()
        {
            // Arrange
            var emptyList = new List<IJCombatTurnBasedUnit>();
            _selector.AddUnits(emptyList);

            // Act
            _selector.ResetActionUnits();
            var actionUnits = _selector.GetActionUnits();

            // Assert
            Assert.AreEqual(0, actionUnits.Count);
        }

        [Test]
        public void ResetActionUnits_ShouldMaintainOriginalOrder_WhenActionPointsEqual()
        {
            // Arrange
            _unit1.GetActionPoint().Returns(12);
            _unit2.GetActionPoint().Returns(11);
            _unit3.GetActionPoint().Returns(10);

            var units = new List<IJCombatTurnBasedUnit> { _unit1, _unit2, _unit3 };
            _selector.AddUnits(units);

            // 改变actionList状态
            _selector.PopActionUnit();

            // Act
            _selector.ResetActionUnits();
            var actionUnits = _selector.GetActionUnits();

            // Assert
            Assert.AreEqual(3, actionUnits.Count);
            // 当行动点相同时，应保持原始添加顺序
            Assert.AreEqual(_unit1, actionUnits[0]);
            Assert.AreEqual(_unit2, actionUnits[1]);
            Assert.AreEqual(_unit3, actionUnits[2]);
        }
    }
}