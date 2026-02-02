//using JFrame.UI;
//using NUnit.Framework;
using JFramework;
using NUnit.Framework;
using NSubstitute;
using System.Collections.Generic;
using System;
//using System.Runtime.Remoting.Contexts;
using static JFramework.PVPBattleManager;

namespace JFrameTest
{
    public class TestCombat
    {
        CombatUnit myUnit;
        CombatUnit mySubUnit;
        CombatUnit unit1;
        CombatUnit unit2;

        CombatContext context;


        CombatFrame frame;

        List<CombatUnitInfo> team1;
        List<CombatUnitInfo> team2;
        CombatManager combatManager;

        [SetUp]
        public void Setup()
        {
            //创建combatManager
            //combatManager = new CombatManager();
            //team1 = new List<CombatUnitInfo>();
            //team2 = new List<CombatUnitInfo>();
  
            ////创建上下文
            //context = new CombatContext();
            //context.CombatManager = combatManager;

            //frame = context.CombatManager.Frame;
        }

        public void TestCombatUnitDead()
        {
            //arrage

        }

        //[Test]
        //public void TestCombatGJJ()
        //{
        //    Dictionary<int, CombatActionArgSource> dicGjjAction = new Dictionary<int, CombatActionArgSource>();
        //    dicGjjAction.Add(1, new CombatRealActionArgSource(1));

        //    Dictionary<int, CombatActionArgSource> dicGjjHeroAction = new Dictionary<int, CombatActionArgSource>();
        //    dicGjjHeroAction.Add(100, new CombatRealActionArgSource(100));
        //    dicGjjHeroAction.Add(101, new CombatRealActionArgSource(101));

        //    team1.Add(CreateUnitInfo("GJJ1", 100, 10, 1f, new CombatVector() { x = -1.5f }, new CombatVector(), new CombatVector() { x = -1.5f }, CreateActions(dicGjjAction)));
        //    team1.Add(CreateUnitInfo("heor", 10, 2, 1f, new CombatVector() , new CombatVector(), new CombatVector(), CreateActions(dicGjjHeroAction)));

        //    team2.Add(CreateUnitInfo("GJJ2", 400, 10, 1f, new CombatVector() { x = 3.5f }, new CombatVector() { x = -0.1f }, new CombatVector() { x = 3.5f }, CreateActions(dicGjjAction)));

        //    combatManager.Initialize(new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Combine, team1), new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team2), 90);

        //    //获取我自己
        //    myUnit = combatManager.GetUnit("1");
        //    unit1 = combatManager.GetUnit("2");
        //    unit2 = combatManager.GetUnit("3");

        //    //act
        //    var report = combatManager.GetResult();

        //    //expect
        //    var reporter = combatManager.Reporter;
        //}

        [Test]
        public void TestCombatResult()
        {
            //arrange
            //Dictionary<int, CombatActionArgSource> dicGjjAction = new Dictionary<int, CombatActionArgSource>();
            //dicGjjAction.Add(1, new CombatJFrameFakeActionArgSource(1));

            //team1.Add(CreateUnitInfo("1", 100, 10, 1f, new CombatVector(), new CombatVector(), new CombatVector(), CreateActions(dicGjjAction)));

            //team2.Add(CreateUnitInfo("2", 100, 1, 1f, new CombatVector() { x = 3 }, new CombatVector() { x = -1f }, new CombatVector() { x = 2 }, CreateActions(2f)));
            //team2.Add(CreateUnitInfo("3", 200, 2, 1f, new CombatVector() { x = 5 }, new CombatVector() { x = -1f }, new CombatVector() { x = 3 }, CreateActions(3f)));
            //combatManager.Initialize(new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team1), new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team2), 90);

            ////获取我自己
            //myUnit = combatManager.GetUnit("1");
            //unit1 = combatManager.GetUnit("2");
            //unit2 = combatManager.GetUnit("3");

            ////act
            //var report = combatManager.GetResult();

            ////expect
            //var reporter = combatManager.Reporter;
            //Assert.AreEqual(2f, unit1.GetPosition().x); //攻击距离1，所以不走了
            //Assert.AreEqual(3f, unit2.GetPosition().x); //攻击距离1，所以不走了
            ////Assert.AreEqual(true, triggerOn);
            ////Assert.AreEqual(true, isOn);
            //Assert.AreEqual(0, combatManager.GetUnit("2").GetAttributeCurValue(PVPAttribute.HP));
            //Assert.AreEqual(0, combatManager.GetUnit("3").GetAttributeCurValue(PVPAttribute.HP));
            //Assert.AreEqual(33, combatManager.GetUnit("1").GetAttributeCurValue(PVPAttribute.HP));
        }

        //[Test]
        //public void TestCombatSubUnit()
        //{
        //    //arrange
        //    Dictionary<int, CombatActionArgSource> dicGjjAction = new Dictionary<int, CombatActionArgSource>();
        //    dicGjjAction.Add(1, new CombatJFrameFakeActionArgSource(1));
        //    team1.Add(CreateUnitInfo("1", 100, 10, 1f, new CombatVector(), new CombatVector(), new CombatVector(), CreateActions(dicGjjAction)));
        //    team1.Add(CreateUnitInfo("11", 1000000, 10, 1f, new CombatVector(), new CombatVector(), new CombatVector(), CreateActions(dicGjjAction)));

        //    team2.Add(CreateUnitInfo("2", 100, 50, 1f, new CombatVector() { x = 3 }, new CombatVector() { x = -1f }, new CombatVector() { x = 2 }, CreateActions(2f)));
        //    team2.Add(CreateUnitInfo("3", 200, 50, 1f, new CombatVector() { x = 5 }, new CombatVector() { x = -1f }, new CombatVector() { x = 4 }, CreateActions(3f)));
        //    combatManager.Initialize(new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Combine, team1), new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team2), 90);

        //    //获取我自己
        //    myUnit = combatManager.GetUnit("1");
        //    mySubUnit = combatManager.GetUnit("11");
        //    unit1 = combatManager.GetUnit("2");
        //    unit2 = combatManager.GetUnit("3");

        //    //act
        //    var report = combatManager.GetResult();
        //    var reporter = combatManager.Reporter;
        //    var allReportData = reporter.GetAllReportData();
        //    var hasStartMoveReport = 0;
        //    var hasEndMoveReport = 0;
        //    var usbUnitAct = false;
        //    foreach (var data in allReportData)
        //    {
        //        if (data.ReportType == ReportType.StartMove)
        //        {
        //            hasStartMoveReport++;
        //        }

        //        if (data.ReportType == ReportType.EndMove)
        //        {
        //            hasEndMoveReport++;
        //        }

        //        if(data.ReportType == ReportType.ActionCast && data.ReportData.CastUnitUid == "11")
        //        {
        //            usbUnitAct = true;
        //        }
        //    }

        //    //expect
        //    Assert.AreEqual(2f, unit1.GetPosition().x); //攻击距离1，所以不走了
        //    Assert.AreEqual(4f, unit2.GetPosition().x); //攻击距离1，所以不走了
        //    Assert.AreEqual(0, combatManager.GetUnit("11").GetAttributeCurValue(PVPAttribute.HP));
        //    Assert.AreEqual(80, combatManager.GetUnit("2").GetAttributeCurValue(PVPAttribute.HP));
        //    Assert.AreEqual(200, combatManager.GetUnit("3").GetAttributeCurValue(PVPAttribute.HP));
        //    Assert.AreEqual(0, combatManager.GetUnit("1").GetAttributeCurValue(PVPAttribute.HP));
        //    Assert.IsTrue(usbUnitAct);
        //}

        //[Test]
        //public void TestCombatUnitMoveReport()
        //{
        //    //arrange
        //    Dictionary<int, CombatActionArgSource> dicGjjAction = new Dictionary<int, CombatActionArgSource>();
        //    dicGjjAction.Add(1, new CombatJFrameFakeActionArgSource(1));
        //    team1.Add(CreateUnitInfo("1", 100, 10, 1f, new CombatVector(), new CombatVector(), new CombatVector(), CreateActions(dicGjjAction)));
        //    team1.Add(CreateUnitInfo("11", 1000000, 10, 1f, new CombatVector(), new CombatVector(), new CombatVector(), CreateActions(dicGjjAction)));

        //    team2.Add(CreateUnitInfo("2", 100, 50, 1f, new CombatVector() { x = 3 }, new CombatVector() { x = -1f }, new CombatVector() { x = 3 }, CreateActions(2f)));
        //    team2.Add(CreateUnitInfo("3", 200, 50, 1f, new CombatVector() { x = 5 }, new CombatVector() { x = -1f }, new CombatVector() { x = 5 }, CreateActions(3f)));
        //    combatManager.Initialize(new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Combine, team1), new KeyValuePair<CombatTeamType, List<CombatUnitInfo>>(CombatTeamType.Single, team2), 90);

        //    //act
        //    var report = combatManager.GetResult();
        //    var reporter = combatManager.Reporter;
        //    var allReportData = reporter.GetAllReportData();
        //    var hasStartMoveReport = 0;
        //    var hasEndMoveReport = 0;
        //    foreach (var data in allReportData)
        //    {
        //        if (data.ReportType == ReportType.StartMove)
        //        {
        //            hasStartMoveReport ++;
        //        }

        //        if (data.ReportType == ReportType.EndMove)
        //        {
        //            hasEndMoveReport ++;
        //        }
        //    }

        //    //expect
        //    Assert.AreEqual(4, hasStartMoveReport);
        //    Assert.AreEqual(4, hasEndMoveReport);
        //}


        CombatUnitInfo CreateUnitInfo(string uid, int hp, int atk, float atkSpeed, CombatVector position, CombatVector moveSpeed, CombatVector targetPosition, Dictionary<int, ActionInfo> actionsData = null)
        {
            var unitInfo = new CombatUnitInfo();
            unitInfo.uid = uid;
            unitInfo.hp = hp;
            unitInfo.atk = atk;
            unitInfo.maxHp = hp;
            unitInfo.atkSpeed = atkSpeed;
            unitInfo.position = position;
            unitInfo.moveSpeed = moveSpeed;
            unitInfo.actionsData = actionsData;
            unitInfo.targetPosition = targetPosition;
            return unitInfo;
        }


        Dictionary<int, ActionInfo> CreateActions(Dictionary<int, CombatActionArgSource>  actions)
        {
            var result = new Dictionary<int, ActionInfo>();

            foreach(var item in actions)
            {
                var actionId = item.Key;
                var argSource = item.Value;
                var actionInfo = CreateActionInfo(actionId, argSource);
                result.Add(actionId, actionInfo);
            }

            return result;
        }


        ActionInfo CreateActionInfo(int actionId, CombatActionArgSource actionArgSource)
        {
            var actionInfo = new ActionInfo();
            if(actionArgSource == null)
                 actionArgSource = new CombatJFrameFakeActionArgSource(actionId);

            actionInfo.type = actionArgSource.GetActionType();
            actionInfo.mode = actionArgSource.GetActionMode();
            actionInfo.uid = Guid.NewGuid().ToString();

            var dicComponentInfo = new Dictionary<ActionComponentType, List<ActionComponentInfo>>();


            //条件查找器
            var conditionFinders = new List<ActionComponentInfo>();
            var conditionFindersId = actionArgSource.GetConditionFindersId();
            for (int i = 0; i < conditionFindersId.Length; i++)
            {
                var id = conditionFindersId[i];
                var index = i;
                var conditionFinder = new ActionComponentInfo() {id = id, args = actionArgSource.GetConditionFindersArgs(index) }; //时间触发器， 时长
                conditionFinders.Add(conditionFinder);
            }

            dicComponentInfo.Add(ActionComponentType.ConditionFinder, conditionFinders);


            //条件触发器
            var conditionTriggers = new List<ActionComponentInfo>();
            var conditionTriggersId = actionArgSource.GetConditionTriggersId();
            for (int i = 0; i < conditionTriggersId.Length; i++)
            {
                var id = conditionTriggersId[i];
                var index = i;
                var conditionTrigger = new ActionComponentInfo() { id = id, args = actionArgSource.GetConditionTriggersArgs(index) }; //查找最近单位触发器 攻击距离， 查找个数
                conditionTriggers.Add(conditionTrigger);
            }
            dicComponentInfo.Add(ActionComponentType.ConditionTrigger, conditionTriggers);

            //延迟触发器
            var delayTriggers = new List<ActionComponentInfo>();
            var delayTrigger = new ActionComponentInfo() { id = actionArgSource.GetDelayTriggerId(), args = actionArgSource.GetDelayTriggerArgs() }; //时间触发器， 时长
            delayTriggers.Add(delayTrigger);
            dicComponentInfo.Add(ActionComponentType.DelayTrigger, delayTriggers);


            //查找器
            var finders = new List<ActionComponentInfo>();
            var findersId = actionArgSource.GetFindersId();
            for (int i = 0; i < findersId.Length; i++)
            {
                var id = findersId[i];
                var index = i;
                var finder = new ActionComponentInfo() { id = id, args = actionArgSource.GetFindersArgs(index) }; //查找最近单位触发器 攻击距离， 查找个数
                finders.Add(finder);
            }
            dicComponentInfo.Add(ActionComponentType.ExecutorFinder, finders);

            //执行器
            var executors = new List<ActionComponentInfo>();
            var executorsId = actionArgSource.GetExecutorsId();
            for (int i = 0; i < executorsId.Length; i++)
            {
                var id = executorsId[i];
                var index = i;
                var executor = new ActionComponentInfo() { id = id, args = actionArgSource.GetExecutorsArgs(index) };//伤害触发器， 时长， 伤害倍率
                executors.Add(executor);
            }
            dicComponentInfo.Add(ActionComponentType.Executor, executors);


            //cd触发器
            var cdTriggers = new List<ActionComponentInfo>();
            var cdTriggersId = actionArgSource.GetCdTriggersId();
            for (int i = 0; i < cdTriggersId.Length; i++)
            {
                var id = cdTriggersId[i];
                var index = i;
                var cdTrigger = new ActionComponentInfo() { id = id, args = actionArgSource.GetCdTriggersArgs(index) };//时间触发器， 时长
                cdTriggers.Add(cdTrigger);
            }
            dicComponentInfo.Add(ActionComponentType.CdTrigger, cdTriggers);

            actionInfo.componentInfo = dicComponentInfo;
            //actionsData.Add(actionId, actionInfo);//actionID

            return actionInfo;
        }


        //Dictionary<int, ActionInfo> CreateActions(float range)
        //{
        //    var actionsData = new Dictionary<int, ActionInfo>();
        //    var actionInfo = new ActionInfo();
        //    actionInfo.type = ActionType.Normal;
        //    actionInfo.mode = ActionMode.Active;
        //    actionInfo.uid = Guid.NewGuid().ToString();

        //    var dicComponentInfo = new Dictionary<ActionComponentType, List<ActionComponentInfo>>();


        //    //条件触发器
        //    var conditionTriggers = new List<ActionComponentInfo>();
        //    var conditionTrigger = new ActionComponentInfo() { id = 1, args = new float[] { 1, 1, range } }; //查找最近单位触发器 攻击距离， 查找个数
        //    conditionTriggers.Add(conditionTrigger);
        //    dicComponentInfo.Add(ActionComponentType.ConditionTrigger, conditionTriggers);
        //    //延迟触发器
        //    var delayTriggers = new List<ActionComponentInfo>();
        //    var delayTrigger = new ActionComponentInfo() { id = 3, args = new float[] { 0.1f } }; //时间触发器， 时长
        //    delayTriggers.Add(delayTrigger);
        //    dicComponentInfo.Add(ActionComponentType.DelayTrigger, delayTriggers);
        //    //查找器
        //    var finders = new List<ActionComponentInfo>();
        //    //var finder = new ActionComponentInfo() { id = 1, } // 执行器不需要
        //    dicComponentInfo.Add(ActionComponentType.ExecutorFinder, finders);

        //    //执行器
        //    var executors = new List<ActionComponentInfo>();
        //    var executor = new ActionComponentInfo() { id = 1, args = new float[] { 0.1f, 1f } };//伤害触发器， 时长， 伤害倍率
        //    executors.Add(executor);
        //    dicComponentInfo.Add(ActionComponentType.Executor, executors);
        //    //cd触发器
        //    var cdTriggers = new List<ActionComponentInfo>();
        //    var cdTrigger = new ActionComponentInfo() { id = 3, args = new float[] { 0.1f } };//时间触发器， 时长
        //    cdTriggers.Add(cdTrigger);
        //    dicComponentInfo.Add(ActionComponentType.CdTrigger, cdTriggers);

        //    actionInfo.componentInfo = dicComponentInfo;
        //    actionsData.Add(1, actionInfo);//actionID

        //    return actionsData;
        //}
    }


}
