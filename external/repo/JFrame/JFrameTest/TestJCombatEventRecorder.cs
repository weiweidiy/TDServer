using NUnit.Framework;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace JFramework.Game.Tests
{
    [TestFixture]
    public class JCombatEventRecorderTests
    {
        // 测试专用子类（因为原类为abstract）
        public class TestableJCombatEventRecorder : JCombatTurnBasedEventRecorder
        {
            public TestableJCombatEventRecorder(
                IJCombatFrameRecorder frameRecorder,
                Func<JCombatTurnBasedEvent, string> keySelector
            ) : base(frameRecorder, keySelector) { }
        }

        private IJCombatFrameRecorder _frameRecorder;
        private TestableJCombatEventRecorder _recorder;
        private Func<JCombatTurnBasedEvent, string> _keySelector = e => e.Uid;

        [SetUp]
        public void Setup()
        {
            _frameRecorder = Substitute.For<IJCombatFrameRecorder>();
            _recorder = new TestableJCombatEventRecorder(_frameRecorder, _keySelector);
        }

        [Test]
        public void OnDamage_NewEvent_CreatesCorrectStructure()
        {
            // 准备新伤害数据
            var damageData = Substitute.For<IJCombatDamageData>();
            damageData.Uid.Returns("new_uid");
            damageData.GetTargetUid().Returns("target_1");
            damageData.GetDamage().Returns(100);
            damageData.GetCasterUid().Returns("caster_1");
            damageData.GetActionSourceUid().Returns("action_123");
            _frameRecorder.GetCurFrame().Returns(42);

            // 执行
            _recorder.OnDamage(damageData);

            // 验证
            var events = _recorder.GetAllCombatEvents();
            Assert.That(events, Has.Count.EqualTo(1));

            var combatEvent = events[0];
            Assert.That(combatEvent.Uid, Is.EqualTo("new_uid"));
            Assert.That(combatEvent.CurFrame, Is.EqualTo(42));
            Assert.That(combatEvent.CasterUid, Is.EqualTo("caster_1"));
            Assert.That(combatEvent.CastActionUid, Is.EqualTo("action_123"));

            var damageEffects = combatEvent.ActionEffect[CombatEventType.Damage];
            Assert.That(damageEffects, Has.Count.EqualTo(1));
            Assert.That(damageEffects[0].Key, Is.EqualTo("target_1"));
            Assert.That(damageEffects[0].Value, Is.EqualTo(100));
        }

        [Test]
        public void OnDamage_ExistingEvent_MergesDamageData()
        {
            // 准备初始伤害数据
            var initialData = Substitute.For<IJCombatDamageData>();
            initialData.Uid.Returns("existing_uid");
            initialData.GetTargetUid().Returns("target_1");
            initialData.GetDamage().Returns(50);
            initialData.GetCasterUid().Returns("caster_1");
            initialData.GetActionSourceUid().Returns("action_123");

            // 准备新伤害数据（相同UID）
            var newData = Substitute.For<IJCombatDamageData>();
            newData.Uid.Returns("existing_uid");
            newData.GetTargetUid().Returns("target_2");
            newData.GetDamage().Returns(75);

            // 执行初始事件
            _recorder.OnDamage(initialData);

            // 执行更新事件
            _recorder.OnDamage(newData);

            // 验证
            var events = _recorder.GetAllCombatEvents();
            Assert.That(events, Has.Count.EqualTo(1));

            var damageEffects = events[0].ActionEffect[CombatEventType.Damage];
            Assert.That(damageEffects, Has.Count.EqualTo(2));

            Assert.That(damageEffects, Contains.Item(new KeyValuePair<string, int>("target_1", 50)));
            Assert.That(damageEffects, Contains.Item(new KeyValuePair<string, int>("target_2", 75)));
        }

        [Test]
        public void GetAllCombatEvents_ReturnsAllAddedEvents()
        {
            // 添加两个独立事件
            var data1 = CreateDamageData("uid1", "targetA", 10);
            var data2 = CreateDamageData("uid2", "targetB", 20);

            _recorder.OnDamage(data1);
            _recorder.OnDamage(data2);

            // 验证
            var events = _recorder.GetAllCombatEvents();
            Assert.That(events, Has.Count.EqualTo(2));
            Assert.That(events.Exists(e => e.Uid == "uid1"));
            Assert.That(events.Exists(e => e.Uid == "uid2"));
        }

        private IJCombatDamageData CreateDamageData(string uid, string target, int damage)
        {
            var data = Substitute.For<IJCombatDamageData>();
            data.Uid.Returns(uid);
            data.GetTargetUid().Returns(target);
            data.GetDamage().Returns(damage);
            data.GetCasterUid().Returns("default_caster");
            data.GetActionSourceUid().Returns("default_action");
            return data;
        }
    }
}