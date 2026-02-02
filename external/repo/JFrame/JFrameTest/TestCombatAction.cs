//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using System;
using static System.Collections.Specialized.BitVector32;
//using System.Runtime.Remoting.Contexts;
using static JFramework.PVPBattleManager;

namespace JFrameTest
{
    public class TestCombatAction
    {
        CombatUnitAction action;
        CombatContext context;
        CombatUnit my;
        CombatFrame frame;
        [SetUp]
        public void Setup()
        {
            frame = new CombatFrame();
            context = Substitute.For<CombatContext>();
            var combatManager = Substitute.For<SingleCombatManager>();
            my = Substitute.For<CombatUnit>();
            my.GetPosition().Returns(new CombatVector() { x = -1 });
            action = Substitute.For<CombatUnitAction>();
            action.Owner.Returns(my);
            var unit1 = Substitute.For<CombatUnit>();
            var unit2 = Substitute.For<CombatUnit>();
            unit1.GetPosition().Returns(new CombatVector() { x = 1 });
            unit2.GetPosition().Returns(new CombatVector() { x = 2 });
            var dicTeam1 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, new List<CombatUnitInfo>());
            var dicTeam2 = new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, new List<CombatUnitInfo>());
            combatManager.Initialize(dicTeam1, dicTeam2,null,90);
            combatManager.GetOppoTeamId(Arg.Any<CombatUnit>()).Returns(1);
            combatManager.GetUnitsInRange(Arg.Any<CombatUnit>(), Arg.Any<int>(), Arg.Any<float>()).Returns(new System.Collections.Generic.List<CombatUnit>() { unit1, unit2 });
            //component.Owner.Returns(acition);
            context.CombatManager.Returns(combatManager);
        }

        //[Test]
        //public void TestAction()
        //{
        //    //arrage
        //    action = new CombatUnitAction();
        //    action.OnAttach(my);
        //    var trigger = new TriggerTime(null);
        //    trigger.ExtraData = new CombatExtraData() { Caster = my, Action = action };
        //    trigger.OnAttach(action);
        //    trigger.Initialize(context, new float[] { 0.3f });
        //    var sm = new CombatActionSM();
        //    sm.Initialize(action);
        //    action.Initialize(1, "uid",ActionType.Normal, ActionMode.Active, 101, new List<CombatBaseTrigger>() { trigger},new TriggerTime(null), new List<CombatBaseExecutor>(), new List<CombatBaseTrigger>(), sm);
        //    bool isOn = false;
        //    CombatExtraData data = null;
        //    int count = 0;
        //    action.onTriggerOn += (extraData) => { isOn = true; data = extraData; count++; };
        //    action.SwitchToDisable();
        //    action.SwitchToTrigging();

        //    //act
        //    action.Start();
        //    action.Update(frame);
        //    action.Update(frame);
        //    action.Update(frame);

        //    //expect
        //    Assert.AreEqual(true, isOn);
        //    Assert.AreEqual(my, data.Caster);
        //    Assert.AreEqual(2, count);
        //}
    }


}
