using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using JFramework;
using JFramework.Game;
using static JFramework.PVPBattleManager;

namespace JFrameTest
{



    [TestFixture]
    public class JCombatQueryTests
    {
        private IJCombatFrameRecorder _frameRecorder;
        private List<IJCombatTeam> _teams;
        private JCombatQuery _combatQuery;

        [SetUp]
        public void Setup()
        {
            _frameRecorder = Substitute.For<IJCombatFrameRecorder>();
            _teams = new List<IJCombatTeam>();
            _combatQuery = new JCombatQuery(_teams, t => t.Uid, _frameRecorder);
        }

        [TearDown] 
        public void Teardown() {
            _teams.Clear();
        }


        #region GetTeam 测试
        [Test]
        public void GetTeam_WithExistingTeam_ShouldReturnTeam()
        {
            // 准备
            var mockTeam = Substitute.For<IJCombatTeam>();
            mockTeam.Uid.Returns("team1");
            _teams.Add(mockTeam);
            _combatQuery = new JCombatQuery(_teams, t => t.Uid, _frameRecorder);

            // 执行
            var result = _combatQuery.GetTeam("team1");

            // 验证
            Assert.That(result, Is.EqualTo(mockTeam));
        }

        [Test]
        public void GetTeam_WithNonExistingTeam_ShouldReturnNull()
        {
            // 执行
            var result = _combatQuery.GetTeam("nonexistent");

            // 验证
            Assert.That(result, Is.Null);
        }
        #endregion

        #region GetUnit 测试
        [Test]
        public void GetUnit_WithExistingUnit_ShouldReturnUnit()
        {
            // 准备
            var mockTeam1 = Substitute.For<IJCombatTeam>();
            var mockTeam2 = Substitute.For<IJCombatTeam>();
            mockTeam1.Uid.Returns("team1");
            mockTeam2.Uid.Returns("team2");

            var mockUnit1 = Substitute.For<IJCombatUnit>();
            mockUnit1.Uid.Returns("unit1");
            mockTeam1.GetUnit("unit1").Returns(mockUnit1);


            var mockUnit2 = Substitute.For<IJCombatUnit>();
            mockUnit2.Uid.Returns("unit2");
            mockTeam2.GetUnit("unit2").Returns(mockUnit2);

            _teams.AddRange(new[] { mockTeam1, mockTeam2 });
            _combatQuery = new JCombatQuery(_teams, t => t.Uid, _frameRecorder);

            // 执行
            var result = _combatQuery.GetUnit("unit1");

            // 验证
            Assert.That(result, Is.EqualTo(mockUnit1));
        }

        [Test]
        public void GetUnit_WithNonExistingUnit_ShouldReturnNull()
        {
            // 执行
            var result = _combatQuery.GetUnit("nonexistent");

            // 验证
            Assert.That(result, Is.Null);
        }
        #endregion

        #region GetUnits 测试
        [Test]
        public void GetUnits_ShouldReturnAllUnitsFromAllTeams()
        {
            // 准备
            var mockTeam1 = Substitute.For<IJCombatTeam>();
            var mockTeam2 = Substitute.For<IJCombatTeam>();
            var units1 = new List<IJCombatUnit> { Substitute.For<IJCombatUnit>() };
            var units2 = new List<IJCombatUnit> { Substitute.For<IJCombatUnit>(), Substitute.For<IJCombatUnit>() };

            mockTeam1.GetAllUnits().Returns(units1);
            mockTeam2.GetAllUnits().Returns(units2);
            mockTeam1.Uid.Returns("team1");
            mockTeam2.Uid.Returns("team2");
            _teams.AddRange(new[] { mockTeam1, mockTeam2 });
            _combatQuery = new JCombatQuery(_teams, t => t.Uid, _frameRecorder);

            // 执行
            var result = _combatQuery.GetUnits();

            // 验证
            Assert.That(result.Count, Is.EqualTo(3));
        }

        [Test]
        public void GetUnits_WithTeamUid_ShouldReturnTeamUnits()
        {
            // 准备
            var mockTeam = Substitute.For<IJCombatTeam>();
            var expectedUnits = new List<IJCombatUnit> { Substitute.For<IJCombatUnit>() };

            mockTeam.Uid.Returns("team1");
            mockTeam.GetAllUnits().Returns(expectedUnits);
            _teams.Add(mockTeam);

            // 执行
            _combatQuery = new JCombatQuery(_teams, t => t.Uid, _frameRecorder);
            var result = _combatQuery.GetUnits("team1");

            // 验证
            Assert.That(result, Is.EqualTo(expectedUnits));
        }

        [Test]
        public void GetUnits_WithPredicate_ShouldReturnFilteredUnits()
        {
            // 准备
            var mockTeam = Substitute.For<IJCombatTeam>();
            var unit1 = Substitute.For<IJCombatUnit>();
            var unit2 = Substitute.For<IJCombatUnit>();

            unit1.IsDead().Returns(false);
            unit2.IsDead().Returns(true);
            unit1.Uid.Returns("1");
            unit2.Uid.Returns("2");
            mockTeam.GetAllUnits().Returns(new List<IJCombatUnit> { unit1, unit2 });
            mockTeam.Uid.Returns("team");
            _teams.Add(mockTeam);
            
            _combatQuery = new JCombatQuery(_teams, t => t.Uid, _frameRecorder);

            // 执行
            var result = _combatQuery.GetUnits(u => !u.IsDead());

            // 验证
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(unit1));
        }
        #endregion

        #region IsCombatOver 测试
        [Test]
        public void IsCombatOver_WhenMaxFrameReached_ShouldReturnTrue()
        {
            // 准备
            _frameRecorder.IsMaxFrame().Returns(true);

            // 执行
            var result = _combatQuery.IsCombatOver();

            // 验证
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsCombatOver_WhenAllTeamsDead_ShouldReturnTrueWithNoWinner()
        {
            // 准备
            var mockTeam = Substitute.For<IJCombatTeam>();
            mockTeam.IsAllDead().Returns(true);
            _teams.Add(mockTeam);

            // 执行
            var result = _combatQuery.IsCombatOver();

            // 验证
            Assert.That(result, Is.True);
            Assert.That(_combatQuery.GetWinner(), Is.Null);
        }

        [Test]
        public void IsCombatOver_WhenOneTeamAlive_ShouldReturnTrueWithWinner()
        {
            // 准备
            var deadTeam = Substitute.For<IJCombatTeam>();
            var aliveTeam = Substitute.For<IJCombatTeam>();

            deadTeam.IsAllDead().Returns(true);
            aliveTeam.IsAllDead().Returns(false);
            aliveTeam.Uid.Returns("1");
            deadTeam.Uid.Returns("2");
            _teams.AddRange(new[] { deadTeam, aliveTeam });

            // 执行
            _combatQuery = new JCombatQuery(_teams, t => t.Uid, _frameRecorder);
            var result = _combatQuery.IsCombatOver();

            // 验证
            Assert.That(result, Is.True);
            Assert.That(_combatQuery.GetWinner(), Is.EqualTo(aliveTeam));
        }

        [Test]
        public void IsCombatOver_WhenMultipleTeamsAlive_ShouldReturnFalseWithNoWinner()
        {
            // 准备
            var team1 = Substitute.For<IJCombatTeam>();
            var team2 = Substitute.For<IJCombatTeam>();

            team1.IsAllDead().Returns(false);
            team2.IsAllDead().Returns(false);
            team1.Uid.Returns("1");
            team2.Uid.Returns("2");
            _teams.AddRange(new[] { team1, team2 });

            // 执行
            _combatQuery = new JCombatQuery(_teams, t => t.Uid, _frameRecorder);
            var result = _combatQuery.IsCombatOver();

            // 验证
            Assert.That(result, Is.False);
            Assert.That(_combatQuery.GetWinner(), Is.Null);
        }
        #endregion

        #region Frame相关测试
        [Test]
        public void IsMaxFrame_ShouldReturnFrameRecorderValue()
        {
            // 准备
            _frameRecorder.IsMaxFrame().Returns(true);

            // 执行 & 验证
            Assert.That(_combatQuery.IsMaxFrame(), Is.True);
        }

        [Test]
        public void GetCurFrame_ShouldReturnFrameRecorderValue()
        {
            // 准备
            _frameRecorder.GetCurFrame().Returns(10);

            // 执行 & 验证
            Assert.That(_combatQuery.GetCurFrame(), Is.EqualTo(10));
        }

        [Test]
        public void GetMaxFrame_ShouldReturnFrameRecorderValue()
        {
            // 准备
            _frameRecorder.GetMaxFrame().Returns(100);

            // 执行 & 验证
            Assert.That(_combatQuery.GetMaxFrame(), Is.EqualTo(100));
        }
        #endregion
    }
}