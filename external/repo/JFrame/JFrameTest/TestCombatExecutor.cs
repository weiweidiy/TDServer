//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using System;

namespace JFrameTest
{
    public class TestCombatExecutor
    {
        CombatUnit unit1;
        CombatUnit unit2;
        CombatAttributeDouble hpAttr;
        CombatAttributeDouble maxHpAttr;
        CombatAttributeDouble atkAttr;
        CombatAttributeManger attributeManager;
        CombatExtraData extraData;
        CombatBaseFormula fakeFormula;

        CombatAttributeDouble hpAttr2;
        CombatAttributeDouble maxHpAttr2;
        CombatAttributeDouble atkAttr2;
        CombatAttributeManger attributeManager2;

        [SetUp]
        public void SetUp()
        {

            unit1 = Substitute.For<CombatUnit>();

            hpAttr = new CombatAttributeDouble(CombatAttribute.CurHp.ToString(), 90, double.MaxValue);
            maxHpAttr = new CombatAttributeDouble(CombatAttribute.MaxHP.ToString(), 100, double.MaxValue);
            atkAttr = new CombatAttributeDouble(CombatAttribute.ATK.ToString(), 10, double.MaxValue);
            attributeManager = new CombatAttributeManger();
            attributeManager.Add(hpAttr);
            attributeManager.Add(maxHpAttr);
            //attributeManager.Add(atkAttr);
            unit1.GetAttributeManager().Returns(attributeManager);
            extraData = new CombatExtraData();
            //extraData.Caster = unit2;
            extraData.Targets = new List<CombatUnit>();
            extraData.Targets.Add(unit1);
            fakeFormula = Substitute.For<CombatBaseFormula>();

            unit2 = Substitute.For<CombatUnit>();
            hpAttr2 = new CombatAttributeDouble(CombatAttribute.CurHp.ToString(), 90, double.MaxValue);
            maxHpAttr2 = new CombatAttributeDouble(CombatAttribute.MaxHP.ToString(), 100, double.MaxValue);
            atkAttr2 = new CombatAttributeDouble(CombatAttribute.ATK.ToString(), 10, double.MaxValue);
            attributeManager2 = new CombatAttributeManger();
            attributeManager2.Add(hpAttr2);
            attributeManager2.Add(maxHpAttr2);

            unit2.GetAttributeManager().Returns(attributeManager2);
            extraData.Caster = unit2;
        }


        [TearDown]
        public void Clear()
        {

        }

        [Test]
        public void TestExecutorDamageAndMinusHp()
        {
            //arrange
            unit1.IsAlive().Returns(true);
            unit2.IsAlive().Returns(true);
            extraData.Caster = unit2;
            unit2.GetAttributeCurValue(CombatAttribute.HpSteal).Returns(0.5d);
            unit2.GetAttributeCurValue(CombatAttribute.HpRecover).Returns(3d); // 1+3倍加成

            fakeFormula.GetHitValue(Arg.Any<CombatExtraData>()).Returns(20);
            fakeFormula.IsHit(Arg.Any<CombatExtraData>()).Returns(true);
            var executor = new ExecutorCombatDamage(null, fakeFormula);
            executor.Initialize(null, new float[] { 1f, 2f ,1});
            //extraData.Value = 10;
            var frame = new CombatFrame();

            //act
            executor.OnStart();
            executor.Execute(extraData);
            executor.Update(frame);

            //expect
            Assert.AreEqual(70, hpAttr.CurValue);
        }

        [Test]
        public void TestExecutorContinuousDamageAndMinusHp()
        {
            //arrange
            unit1.IsAlive().Returns(true);
            unit2.IsAlive().Returns(true);
            extraData.Caster = unit2;
            unit2.GetAttributeCurValue(CombatAttribute.HpSteal).Returns(0.5d);
            unit2.GetAttributeCurValue(CombatAttribute.HpRecover).Returns(3d); // 1+3倍加成

            fakeFormula.GetHitValue(Arg.Any<CombatExtraData>()).Returns(10);
            fakeFormula.IsHit(Arg.Any<CombatExtraData>()).Returns(true);
            var executor = new ExecutorCombatContinuousDamage(null, fakeFormula);
            executor.Initialize(null, new float[] { 1f, 1f, 1, 0.25f, 3 });
            //extraData.Value = 10;
            var frame = new CombatFrame();
            //act
            executor.OnStart();
            executor.Execute(extraData);
            executor.Update(frame);
            executor.Update(frame);
            executor.Update(frame);
            executor.Update(frame);

            //expect
            Assert.AreEqual(60, hpAttr.CurValue);
        }

        [Test]
        public void TestExecutorHealAndPlusHp()
        {
            //arrange
            unit1.IsAlive().Returns(true);
            unit2.IsAlive().Returns(true);
            extraData.Caster = unit1;
            unit1.GetAttributeCurValue(CombatAttribute.HpRecover).Returns(3d); // 1+3倍加成

            fakeFormula.GetHitValue(Arg.Any<CombatExtraData>()).Returns(2); //回复2点 加成后 8点
            fakeFormula.IsHit(Arg.Any<CombatExtraData>()).Returns(true);
            var executor = new ExecutorCombatHeal(null, fakeFormula);
            executor.Initialize(null, new float[] { 1f, 2f });
            //extraData.Value = 10;
            var frame = new CombatFrame();

            //act
            executor.OnStart();
            executor.Execute(extraData);
            executor.Update(frame);
            executor.Update(frame);
            executor.Update(frame);
            executor.Update(frame);

            //expect
            Assert.AreEqual(98, hpAttr.CurValue);
        }


        [Test]
        public void TestExecutorContinuousHealAndPlusHp()
        {
            //arrange
            unit1.IsAlive().Returns(true);
            unit2.IsAlive().Returns(true);
            extraData.Caster = unit1;
            unit1.GetAttributeCurValue(CombatAttribute.HpRecover).Returns(0d); // 1+3倍加成

            fakeFormula.GetHitValue(Arg.Any<CombatExtraData>()).Returns(1);
            fakeFormula.IsHit(Arg.Any<CombatExtraData>()).Returns(true);
            var executor = new ExecutorCombatContinuousHeal(null, fakeFormula);
            executor.Initialize(null, new float[] { 1f, 1f, 0.25f, 3 });
            //extraData.Value = 10;
            var frame = new CombatFrame();
            //act
            executor.OnStart();
            executor.Execute(extraData);
            executor.Update(frame);
            executor.Update(frame);
            executor.Update(frame);
            executor.Update(frame);

            //expect
            Assert.AreEqual(93, hpAttr.CurValue);
        }


        [Test]
        public void TestExecutorAttr()
        {
            //arrange
            unit1.IsAlive().Returns(true);
            unit2.IsAlive().Returns(true);
            extraData.Action = new CombatAction();
            extraData.FoldCount = 1;
            extraData.Action.Initialize(null, 1, "action", ActionType.All, ActionMode.Active, 1, 0,null, null, null, null, null, null);
            //hpAttr = new CombatAttributeDouble(PVPAttribute.HP.ToString(), 90, double.MaxValue);
            unit1.GetAttribute(CombatAttribute.MaxHP).Returns(maxHpAttr);
            var executor = new ExecutorCombatChangeAttribute(null, null);
            executor.Initialize(null, new float[] { 0.1f, 102, 0.1f });

            //act
            executor.OnStart();
            executor.Execute(extraData);
            executor.Update(null);

            //expect
            unit1.Received(1).PlusExtraValue(CombatAttribute.MaxHP, "action", Arg.Any<double>());
            Assert.AreEqual(100, (long)hpAttr.CurValue);
        }

        [Test]
        public void TestExecutorAttrRemove()
        {
            //arrange
            unit1.IsAlive().Returns(true);
            unit2.IsAlive().Returns(true);
            extraData.Action = new CombatAction();
            extraData.FoldCount = 1;
            extraData.Action.Initialize(null,1, "action", ActionType.All, ActionMode.Active,1, 0,null, null, null, null, null, null);

            //hpAttr = new CombatAttributeDouble(PVPAttribute.HP.ToString(), 90, double.MaxValue);
            unit1.GetAttribute(CombatAttribute.MaxHP).Returns(maxHpAttr);
            var executor = new ExecutorCombatChangeAttribute(null, null);
            executor.Initialize(null, new float[] { 0.1f, 102, 0.1f });

            //act
            executor.OnStart();
            executor.Execute(extraData);
            executor.Update(null);
            executor.OnStop();
            //expect
            unit1.Received(1).PlusExtraValue(CombatAttribute.MaxHP, "action", Arg.Any<double>());
            unit1.Received(1).MinusExtraValue(CombatAttribute.MaxHP, "action", Arg.Any<double>());
            Assert.AreEqual(100, (int)hpAttr.CurValue);
        }

        [Test]
        public void TestExecutorAddBuffer()
        {
            //arrange

            unit2.IsAlive().Returns(true);
            var extraData = Substitute.For<CombatExtraData>();     
            var context = Substitute.For<CombatContext>();
            var bufferFactoy = Substitute.For<CombatBufferFactory>();
            context.CombatBufferFactory.Returns(bufferFactoy);
            var unit1 = Substitute.For<CombatUnit>();
            var bufferManager = Substitute.For<CombatBufferManager>();
            unit1.GetBufferManager().Returns(bufferManager);
            unit1.IsAlive().Returns(true);
            var buffer = Substitute.For<CombatBuffer>();
            bufferFactoy.CreateBuffer(1001, Arg.Any<CombatExtraData>(), Arg.Any<int>()).Returns(buffer);
            extraData.Targets.Returns(new List<CombatUnit>() { unit1 });
            var executor = new ExecutorCombatAddBuffer(null, null);
            executor.Initialize(context, new float[] { 0.1f, 1001, 2, 5f , 1f});

            //act
            executor.OnStart();
            executor.Execute(extraData);
            executor.Update(new CombatFrame());

            //expect
            unit1.Received(1).AddBuffer(buffer);
            //buffer.Received(1).SetCurFoldCount(2);
            buffer.Received(1).SetDuration(5f);

        }
    }


}
