using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JFramework
{
    public class ActionFactory
    {
        int unitId;
        string unitUID;
        ActionDataSource actionDataSource;
        BattlePoint battlePoint;
        PVPBattleManager pvpBattleManager;
        FormulaManager formulaManager;
        BattleUnitInfo unitInfo = default;
        TriggerFactory triggerFactory = new TriggerFactory();
        FinderFactory finderFactory = new FinderFactory();
        ExecutorFactory executorFactory = new ExecutorFactory();
        public ActionFactory(BattleUnitInfo unitInfo, ActionDataSource dataSource, BattlePoint battlePoint, PVPBattleManager pvpBattleManager, FormulaManager formulaManager)
        {
            this.unitInfo = unitInfo;
            this.unitUID = unitInfo.uid;
            this.unitId = unitInfo.id;
            this.actionDataSource = dataSource;
            this.battlePoint = battlePoint;
            this.pvpBattleManager = pvpBattleManager;
            this.formulaManager = formulaManager;
        }

        public ActionFactory(ActionDataSource dataSource, PVPBattleManager pvpBattleManager, FormulaManager formulaManager) : this(default(BattleUnitInfo), dataSource, null, pvpBattleManager, formulaManager)
        {

        }


        public IBattleAction Create(int actionId)
        {
            var uid = Guid.NewGuid().ToString();
            var type = (ActionType)actionDataSource.GetType(unitUID, unitId, actionId);
            var duration = actionDataSource.GetDuration(unitUID, unitId, actionId);
            var delay = (battlePoint.Point - 1) * 0.2f; //首次攻击延迟
            var conditionTrigger = triggerFactory.Create(pvpBattleManager, actionDataSource.GetConditionTriggerType(unitUID, unitId, actionId), actionDataSource.GetConditionTriggerArg(unitUID, unitId, actionId) , delay);//CreateConditionTrigger(actionDataSource.GetConditionTriggerType(unitUID, unitId, actionId), actionDataSource.GetConditionTriggerArg(unitUID, unitId, actionId), 0f);
            var targetFinder = finderFactory.Create(pvpBattleManager, actionDataSource.GetFinderType(unitUID, unitId, actionId), battlePoint, actionDataSource.GetFinderArg(unitUID, unitId, actionId)); // CreateTargetFinder(actionDataSource.GetFinderType(unitUID, unitId, actionId), battlePoint, actionDataSource.GetFinderArg(unitUID, unitId, actionId));
            var executors = CreateExecutors(unitUID, unitId, actionId);
            var cdTrigger = triggerFactory.Create(pvpBattleManager, actionDataSource.GetCDTriggerType(unitUID, unitId, actionId), GetCDTriggerArg(actionId), 0f); // CreateCDTrigger(actionDataSource.GetCDTriggerType(unitUID, unitId, actionId), GetCDTriggerArg(actionId), 0f);
            var sm = new OldActionSM();

            switch (actionDataSource.GetActionMode(unitUID, unitId, actionId))
            {
                case ActionMode.Active:
                    return new ActiveAction(uid, actionId, type, duration, conditionTrigger, targetFinder, executors,cdTrigger, sm);
                case ActionMode.Passive:
                    return new PassiveAction(uid, actionId, type, duration, conditionTrigger, targetFinder, executors, cdTrigger, sm);
                default:
                    throw new Exception("没有实现技能模式 " + actionId);

            }

            //return new ActiveAction(Guid.NewGuid().ToString(), actionId, (ActionType)actionDataSource.GetType(unitUID, unitId, actionId), actionDataSource.GetDuration(unitUID, unitId, actionId),
            //            CreateConditionTrigger(actionDataSource.GetConditionTriggerType(unitUID, unitId, actionId), actionDataSource.GetConditionTriggerArg(unitUID, unitId, actionId), 0f)
            //            , CreateTargetFinder(actionDataSource.GetFinderType(unitUID, unitId, actionId), battlePoint, actionDataSource.GetFinderArg(unitUID, unitId, actionId))
            //            , CreateExecutors(unitUID, unitId, actionId)
            //            , CreateCDTrigger(actionDataSource.GetCDTriggerType(unitUID, unitId, actionId), GetCDTriggerArg(actionId), 0f), new ActionSM());
        }

        float[] GetCDTriggerArg(int actionId)
        {
            var actionType = actionDataSource.GetType(unitUID, unitId, actionId);
            if(actionType == 1) //普通攻击，cd参数需要附加atkspeed属性
            {
                var cd = actionDataSource.GetCDTriggerArg(unitUID, unitId, actionId)[0];
                var atkSpeed = unitInfo.atkSpeed;
                if(atkSpeed == 0)
                {
                    unitInfo.atkSpeed = 1;
                    //throw new Exception("atkspeed 不能为 0 " + unitId);
                }
                    
                var arg = cd + 1 / atkSpeed;
                return new float[] { arg };
            }

            return actionDataSource.GetCDTriggerArg(unitUID, unitId, actionId);
        }

        /// <summary>
        /// CD触发器
        /// </summary>
        /// <param name="triggerType"></param>
        /// <param name="args"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private IBattleTrigger CreateCDTrigger(int triggerType, float[] args, float delay = 0)
        {
            switch (triggerType)
            {
                case 1: //周期性触发器
                    return new CDTimeTrigger(pvpBattleManager, args, delay);
                case 2://次数触发器
                    return new AmountTrigger(pvpBattleManager, args, delay);
                default:
                    throw new Exception(triggerType + " 技能未实现的 CDTrigger type " + triggerType);
            }
        }

        ///// <summary>
        ///// 创建触发器
        ///// </summary>
        ///// <param name="triggerType"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //BaseBattleTrigger CreateConditionTrigger(int triggerType, float arg, float delay = 0)
        //{
        //    switch (triggerType)
        //    {
        //        case 1: //无
        //            return new NoneTrigger(pvpBattleManager, new float[1] { arg }, delay);
        //        case 2: //自身死亡触发
        //            return new DeathTrigger(pvpBattleManager, new float[1] { arg }, delay);
        //        case 3: //战斗开始触发
        //            return new BattleStartTrigger(pvpBattleManager, new float[1] { arg }, delay);
        //        case 4: //己方有非满血成员
        //            return new FriendsHurtTrigger(pvpBattleManager, new float[1] { arg }, delay);
        //        case 5: //指定action释放
        //            return new ActionCastTrigger(pvpBattleManager, new float[1] { arg }, delay);
        //        default:
        //            throw new Exception(triggerType + " 技能未实现的 ConditionTrigger type " + triggerType);
        //    }
        //}

        ///// <summary>
        ///// 创建目标搜索器
        ///// </summary>
        ///// <param name="finderType"></param>
        ///// <param name="point"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //IBattleTargetFinder CreateTargetFinder(int finderType, BattlePoint point, float arg)
        //{
        //    switch (finderType)
        //    {
        //        case 1: //顺序找目标（可复数）
        //            return new OrderOppoFinder(point, pvpBattleManager, arg);
        //        case 2: //倒序找目标（可复数）
        //            return new ReverseOrderOppoFinder(point, pvpBattleManager, arg);
        //        case 3: //正序找自己队伍非满血目标（可复数）
        //            return new OrderFriendsHurtFinder(point, pvpBattleManager, arg);
        //        case 4: //随机敌方
        //            return new RandomOppoFinder(point, pvpBattleManager, arg);
        //        case 6: //本体
        //            return new SelfFinder(point, pvpBattleManager, arg);
        //        case 7: //顺序己方（可复数）
        //            return new OrderFriendsFinder(point, pvpBattleManager, arg);
        //        case 8: //顺序敌方攻击最高的
        //            return new OrderOppoTopAtkFinder(point, pvpBattleManager, arg);
        //        default:
        //            throw new Exception("没有实现目标 finder type " + finderType);
        //    }
        //}

        /// <summary>
        /// 创建执行器
        /// </summary>
        /// <param name="unitId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        List<IBattleExecutor> CreateExecutors(string unitUID, int unitId, int actionId)
        {
            var result = new List<IBattleExecutor>();
            var executors = actionDataSource.GetExcutorTypes(unitUID, unitId, actionId);

            //foreach (var executorId in executors)
            for(int i = 0; i < executors.Count;i ++)
            {
                var executorId = executors[i];
                var e = executorFactory.Create(formulaManager, executorId, actionDataSource.GetExcutorArg(unitUID, unitId, actionId, i));   //CreateExcutor(executorId, actionDataSource.GetExcutorArg(unitUID, unitId, actionId, i));
                result.Add(e);
            }
            return result;
        }

        ///// <summary>
        ///// 创建执行器
        ///// </summary>
        ///// <param name="excutorType"></param>
        ///// <param name="arg"></param>
        ///// <returns></returns>
        ///// <exception cref="Exception"></exception>
        //IBattleExecutor CreateExcutor(int excutorType, float[] arg)
        //{
        //    switch (excutorType)
        //    {
        //        case 1: //按释放者攻击力对目标伤害（可多段伤害）
        //            return new ExecutorDamage(formulaManager, arg);
        //        case 2: //给目标添加buffer
        //            return new ExecutorTargetAddBuffer(formulaManager, arg);
        //        case 3: //给目标回血（加值）
        //            return new ExecutorHeal(formulaManager, arg);
        //        case 4: //按目标血量百分比伤害
        //            return new ExecutorHpDamage(formulaManager, arg);
        //        case 5://提升生命上限
        //            return new ExecutorMaxHpUp(formulaManager, arg);
        //        case 6://自己添加buffer
        //            return new ExecutorSelfAddBuffer(formulaManager, arg);
        //        case 7://递增伤害
        //            return new ExecutorIncrementalDamage(formulaManager, arg);
        //        case 8://复活
        //            return new ExecutorReborn(formulaManager, arg);
        //        case 9: //吸血
        //            return new ExecutorSuckHp(formulaManager, arg);
        //        default:
        //            throw new Exception("没有实现指定的 excutor type " + excutorType);
        //    }
        //}
    }
}