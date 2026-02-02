//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;

namespace JFrameTest
{
    public class TestCombatTriggers
    {

        [Test]
        public void TestTriggerFinder()
        {
            //arrange
            var frame = Substitute.For<CombatFrame>();

            var mySelf = Substitute.For<CombatUnit>();

            var extraData = Substitute.For<CombatExtraData>();

            var finder = Substitute.For<CombatBaseFinder>();
            finder.FindTargets(extraData).Returns(new List<CombatUnit>() { mySelf});


            var trigger = new TriggerFinder(new List<CombatBaseFinder>() { finder });
            trigger.ExtraData = extraData;

            //act
            trigger.OnStart();
            trigger.Update(frame);

            //expect
            Assert.AreEqual(true, trigger.IsOn());
        }

        //[Test]
        //public void TestCombatTriggerHurt()
        //{
        //    //arrange
        //    var trigger = new TriggerCombatHurt(Substitute.For<CombatBaseFinder>());
        //    trigger

        //    //act
        //    trigger.OnEnterState();

        //    //expect

        //}



        //CombatUnitAction action;
        //CombatContext context;

        //CombatUnit my;

        //CombatUnit unit1;
        //CombatUnit unit2;

        //[SetUp]
        //public void Setup()
        //{
        //    context = Substitute.For<CombatContext>();
        //    var attributeManager1 = new CombatAttributeManger();
        //    attributeManager1.Add(new CombatAttributeLong(PVPAttribute.HP.ToString(), 100, 100));
        //    var attributeManager2 = new CombatAttributeManger();
        //    attributeManager2.Add(new CombatAttributeLong(PVPAttribute.HP.ToString(), 100, 101));

        //    var combatManager = Substitute.For<CombatManager>();
        //    my = Substitute.For<CombatUnit>();
        //    my.GetPosition().Returns(new CombatVector() { x = -1 });
        //    action = Substitute.For<CombatUnitAction>();
        //    action.Owner.Returns(my);
        //    unit1 = Substitute.For<CombatUnit>();
        //    unit2 = Substitute.For<CombatUnit>();
        //    unit1.GetPosition().Returns(new CombatVector() { x = 1 });
        //    unit2.GetPosition().Returns(new CombatVector() { x = 2 });
        //    unit1.GetAttributeCurValue(PVPAttribute.HP).Returns(100L);
        //    unit2.GetAttributeCurValue(PVPAttribute.HP).Returns(200L);
        //    unit1.GetAttributeMaxValue(PVPAttribute.HP).Returns(100L);
        //    unit2.GetAttributeMaxValue(PVPAttribute.HP).Returns(201L);

        //    unit1.GetAttributeManager().Returns(attributeManager1);
        //    unit2.GetAttributeManager().Returns(attributeManager2);

        //    var dicTeam1 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, new List<CombatUnitInfo>());
        //    var dicTeam2 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, new List<CombatUnitInfo>());
        //    combatManager.Initialize(dicTeam1, dicTeam2, 90);
        //    combatManager.GetOppoTeamId(Arg.Any<CombatUnit>()).Returns(1);
        //    combatManager.GetUnitsInRange(Arg.Any<CombatUnit   >(), Arg.Any<int>(), Arg.Any<float>(), Arg.Any<bool>(), Arg.Any<bool>()).Returns(new List<CombatUnit>() { unit1, unit2 });
        //    combatManager.GetUnits(Arg.Any<int>(), Arg.Any<bool>()).Returns(new List<CombatUnit>() { unit1, unit2 });
        //    combatManager.GetFriendTeamId(Arg.Any<CombatUnit>()).Returns(0);
        //    //component.Owner.Returns(acition);
        //    context.CombatManager.Returns(combatManager);
        //}

        //[Test]
        //public void TestTriggerRangeNearest()
        //{
        //    //arrange
        //    var component = new TriggerRangeNearest(null);
        //    component.ExtraData = new CombatExtraData() { Caster = my,  Action = action };
        //    component.OnAttach(action);
        //    component.Initialize(context, new float[] { 1,1,4 });//攻擊距離

        //    //act   
        //    component.OnStart();
        //    component.Update(null);

        //    //expect
        //    Assert.AreEqual(true, component.IsOn());
        //}

        //[Test]  
        //public void TestTriggerTime()
        //{
        //    //arrange
        //    var component = new TriggerTime(null);
        //    component.ExtraData = new CombatExtraData() { Caster = my, Action = action };
        //    component.OnAttach(action);
        //    component.Initialize(context, new float[] { 0.5f});//攻擊距離
        //    var frame = new BattleFrame();
        //    //act   
        //    component.OnStart();
        //    component.Update(frame);
        //    component.Update(frame);

        //    //expect
        //    Assert.AreEqual(true, component.IsOn());
        //    Assert.AreEqual(action, component.ExtraData.Action);
        //}

        //[Test]
        //public void TestTriggerHp()
        //{
        //    //arrange
        //    var component = new TriggerHp(null);
        //    component.ExtraData = new CombatExtraData() { Caster = my, Action = action };
        //    component.OnAttach(action);
        //    component.Initialize(context, new float[] { 2, 0 });
        //    var frame = new BattleFrame();
        //    //act   
        //    component.OnStart();
        //    component.Update(frame);

        //    //expect
        //    Assert.AreEqual(true, component.IsOn());
        //    Assert.AreEqual(unit2, component.ExtraData.Targets[0]);
        //}

        //[Test]
        //public void TestTriggerCombatStart()
        //{
        //    //arrange
        //    var component = new TriggerCombatStart(null);
        //    component.ExtraData = new CombatExtraData() { Caster = my, Action = action };
        //    component.OnAttach(action);
        //    component.Initialize(context, new float[] {});
        //    var frame = new BattleFrame();
        //    //act   
        //    component.OnStart();
        //    component.Update(frame);

        //    //expect
        //    Assert.AreEqual(true, component.IsOn());
        //}



    }


}
