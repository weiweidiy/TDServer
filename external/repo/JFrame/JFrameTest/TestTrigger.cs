//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NSubstitute;
using NUnit.Framework;
using static JFramework.PVPBattleManager;
using System.Collections.Generic;
using System;
using static System.Collections.Specialized.BitVector32;

namespace JFrameTest
{
    public class TestTrigger
    {
        CombatFrame frame = Substitute.For<CombatFrame>();
        IPVPBattleManager simBattle = Substitute.For<PVPBattleManager>();

        [SetUp]
        public void SetUp()
        {

        }


        [TearDown]
        public void Clear()
        {

        }

        /// <summary>
        /// 测试死亡触发器
        /// </summary>
        [Test]
        public void TestDeathTrigger()
        {
            //arrange
            var trigger = new DeathTrigger(simBattle, new float[] { 1 });
            var unit1 = new BattleUnit(new BattleUnitInfo() { atk = 1, hp = 10, uid = "1" }, null, null);
            var action = Substitute.For<IBattleAction, IAttachOwner>();
            action.Owner.Returns(unit1);
            //bool triggerOn = false;
            //int count = 0;
            //trigger.onTrigger += () => { triggerOn = true; count++; };

            //action
            trigger.OnAttach(action as IAttachOwner);
            unit1.OnDamage(null, null, new ExecuteInfo() { Value = 10 });
            unit1.OnDamage(null, null, new ExecuteInfo() { Value = 10 });
            trigger.Update(frame);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
            //Assert.AreEqual(1, count);
        }

        /// <summary>
        /// cd 触发器
        /// </summary>
        [Test]
        public void TestCDTrigger()
        {
            //arrange
            var trigger = new CDTimeTrigger(simBattle, new float[] { 3 });
            //bool triggerOn = false;
            //int count = 0;
            //trigger.onTrigger += () => { triggerOn = true; count++; };
            frame.DeltaTime.Returns(3);

            //action
            trigger.Update(frame);
            trigger.Restart();
            trigger.Update(frame);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
            //Assert.AreEqual(2, count); //因为只会触
        }

        /// <summary>
        /// 战斗开始触发器
        /// </summary>
        [Test]
        public void TestBattleStartTrigger()
        {
            //arrange
            var trigger = new BattleStartTrigger(simBattle, new float[] { 3 }, 1);
            //bool triggerOn = false;
            //int count = 0;
            //trigger.onTrigger += () => { triggerOn = true; count++; };
            frame.DeltaTime.Returns(2);

            //action
            trigger.Update(frame);
            trigger.Update(frame);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
            //Assert.AreEqual(1, count);
        }

        [Test]
        public void TestFriendHurtTrigger()
        {
            //arrange
            var trigger = new FriendsHurtTrigger(simBattle, new float[] { 1 });
            var friend = Substitute.For<IBattleUnit>();
            friend.IsHpFull().Returns(false);
            friend.IsAlive().Returns(true);
            var friend2 = Substitute.For<IBattleUnit>();
            friend2.IsHpFull().Returns(true);
            friend2.IsAlive().Returns(true);
            simBattle.GetUnits(Arg.Any<Team>()).Returns(new List<IBattleUnit>() { friend, friend2 });
            simBattle.GetFriendTeam(Arg.Any<IBattleUnit>()).Returns(Team.Attacker);
            frame.DeltaTime.Returns(2);
            var action = Substitute.For<IBattleAction, IAttachOwner>();
            action.Owner.Returns(friend2);

            //action
            trigger.OnAttach(action as IAttachOwner);
            trigger.Update(frame);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }


        [Test]
        public void TestActionCastTrigger()
        {
            //arrange
            var targetAcitionId = 9100;
            var trigger = new ActionCastTrigger(simBattle, new float[] { targetAcitionId });
            var owner = Substitute.For<IBattleUnit>();
            var ownerAction = Substitute.For<IBattleAction, IAttachOwner>();
            var targetAcition = Substitute.For<IBattleAction, IAttachOwner>();
            ownerAction.Owner.Returns(owner);
            owner.GetAction(targetAcitionId).Returns(targetAcition);
            var targets = new List<IBattleUnit>() { Substitute.For<IBattleUnit>()};

            //action
            trigger.OnAttach(ownerAction as IAttachOwner);
            targetAcition.onStartCast += Raise.Event<Action<IBattleAction, List<IBattleUnit>, float>>(targetAcition, targets, 1f);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }

        [Test]
        public void TestHurtTrigger()
        {
            //arrange
            var trigger = new HurtTrigger(simBattle, new float[] { 1, 0});
            bool result = false;
            trigger.onTriggerOn += (sender, arg) => { result = true; };
            var owner = Substitute.For<IAttachOwner>();
            var unit = Substitute.For<IBattleUnit>();
            owner.Owner.Returns(unit);
            var info = new ExecuteInfo() { Value = 10 };

            //action
            trigger.OnAttach(owner);
            //unit.onDamaging += Raise.Event<Action<IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo>>(owner, Substitute.For<IBattleAction>(), owner, info);

            unit.onDamaging += Raise.Event<Action<IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo>>(null, Substitute.For<IBattleAction>(), null, info);

            //expect
            Assert.AreEqual(true, result);
        }

        [Test]
        public void TestFriendCastActionTrigger()
        {
            //arrange
            var trigger = new FriendsActionCastTrigger(simBattle, new float[] {2,1});
            var action = Substitute.For<IBattleAction, IAttachOwner>();
            var unit = Substitute.For<IBattleUnit>();
            action.Owner.Returns(unit);

            var friendAction = Substitute.For<IBattleAction, IAttachOwner>();
            var friend = Substitute.For<IBattleUnit>();
            friend.IsHpFull().Returns(false);
            friend.IsAlive().Returns(true);
            friend.GetActions(ActionType.Skill).Returns(new IBattleAction[] { friendAction });
            simBattle.GetUnits(Arg.Any<Team>()).Returns(new List<IBattleUnit>() { friend });

            //action
            trigger.OnAttach(action as IAttachOwner);
            friendAction.onStartCast += Raise.Event<Action<IBattleAction, List<IBattleUnit>, float>>(action, null, 1f);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }

        [Test]
        public void TestKillTrigger()
        {
            //arrange
            var trigger = new ActionKillTrigger(simBattle, new float[] { 913300 });
            var action = Substitute.For<IBattleAction>();
            var targetAction = Substitute.For<IBattleAction>();
            var unit = Substitute.For<IBattleUnit>();
            action.Owner.Returns(unit);
            unit.GetAction(Arg.Any<int>()).Returns(targetAction);
            var target = Substitute.For<IBattleUnit>();
            target.IsAlive().Returns(false);

            //action
            trigger.OnAttach(action);
            targetAction.onHittedComplete += Raise.Event<Action<IBattleAction, IBattleUnit, ExecuteInfo, IBattleUnit>>(action, null, null, target);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }

        [Test]
        public void TestHittingTrigger()
        {
            //arrange
            var trigger = new ActionHittingTrigger(simBattle, new float[] { 913300 });
            var action = Substitute.For<IBattleAction>();
            var targetAction = Substitute.For<IBattleAction>();
            var unit = Substitute.For<IBattleUnit>();
            action.Owner.Returns(unit);
            unit.GetAction(Arg.Any<int>()).Returns(targetAction);
            var target = Substitute.For<IBattleUnit>();
            target.IsAlive().Returns(false);

            //action
            trigger.OnAttach(action);
            targetAction.onHittingTarget += Raise.Event<Action<IBattleAction, IBattleUnit, ExecuteInfo>>(action, null, null);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }

        [Test]
        public void TestHittedTrigger()
        {
            //arrange
            var trigger = new ActionHittedTrigger(simBattle, new float[] { 913300 });
            var action = Substitute.For<IBattleAction>();
            var targetAction = Substitute.For<IBattleAction>();
            var unit = Substitute.For<IBattleUnit>();
            action.Owner.Returns(unit);
            unit.GetAction(Arg.Any<int>()).Returns(targetAction);
            var target = Substitute.For<IBattleUnit>();
            target.IsAlive().Returns(false);

            //action
            trigger.OnAttach(action);
            targetAction.onHittedComplete += Raise.Event<Action<IBattleAction, IBattleUnit, ExecuteInfo, IBattleUnit>>(action, null, null,null);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }


        [Test]
        public void TestActionCastTimeTrigger()
        {
            //arrange
            var targetAcitionId = 9100;
            var trigger = new ActionCastTimeTrigger(simBattle, new float[] { 0.3f, targetAcitionId});
            var owner = Substitute.For<IBattleUnit>();
            var ownerAction = Substitute.For<IBattleAction, IAttachOwner>();
            var targetAcition = Substitute.For<IBattleAction, IAttachOwner>();
            ownerAction.Owner.Returns(owner);
            owner.GetAction(targetAcitionId).Returns(targetAcition);
            frame.DeltaTime.Returns(0.5f);

            //action
            trigger.OnAttach(ownerAction as IAttachOwner);
            targetAcition.onStartCast += Raise.Event<Action<IBattleAction, List<IBattleUnit>, float>>(targetAcition, null, 1f);
            trigger.Update(frame);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }

        [Test]
        public void TestActionCastHittedTrigger()
        {
            //arrange
            var targetAcitionId = 9100;
            var trigger = new ActionCastHittedTrigger(simBattle, new float[] { targetAcitionId, 0.3f });
            var owner = Substitute.For<IBattleUnit>();
            var ownerAction = Substitute.For<IBattleAction, IAttachOwner>();
            var targetAcition = Substitute.For<IBattleAction, IAttachOwner>();
            ownerAction.Owner.Returns(owner);
            owner.GetAction(targetAcitionId).Returns(targetAcition);
            //frame.DeltaTime.Returns(0.5f);

            //action
            trigger.OnAttach(ownerAction as IAttachOwner);
            targetAcition.onStartCast += Raise.Event<Action<IBattleAction, List<IBattleUnit>, float>>(targetAcition, null, 1f);
            owner.onDamaged += Raise.Event<Action<IBattleUnit,IBattleAction, IBattleUnit, ExecuteInfo>>(owner, targetAcition, null, null);
            

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }


        [Test]
        public void TestFriendAddedBuffTrigger()
        {
            //arrange
            var trigger = new FriednsAddedBufferTrigger(simBattle, new float[] { 999 });
            var buffer = Substitute.For<IBuffer, IAttachOwner>();
            var unit = Substitute.For<IBattleUnit>();
            buffer.Owner.Returns(unit);
            buffer.Id.Returns(999);

            var friend = Substitute.For<IBattleUnit>();
            friend.IsAlive().Returns(true);
            simBattle.GetUnits(Arg.Any<Team>()).Returns(new List<IBattleUnit>() { friend });

            //action
            trigger.OnAttach(buffer);
            friend.onBufferAdded += Raise.Event<Action<IBattleUnit,IBuffer>>(unit, buffer);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }
    }


}
