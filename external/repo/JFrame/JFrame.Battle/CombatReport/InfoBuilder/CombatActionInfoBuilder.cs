using System.Collections.Generic;
using System;

namespace JFramework
{
    public abstract class CombatActionInfoBuilder<T>
    {
        Dictionary<int, CombatActionArgSource> dicActionArgSource;
        ILogger logger;

        public CombatActionInfoBuilder(CombatActionArgSourceBuilder actionArgBuilder, ILogger logger = null)
        {
            dicActionArgSource = actionArgBuilder.Build();
            this.logger = logger;
        }

        public abstract T Build();

        /// <summary>
        /// 返回对应的action参数源对象
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual CombatActionArgSource GetActionArgSource(int actionId)
        {
            if (dicActionArgSource != null && dicActionArgSource.ContainsKey(actionId))
                return dicActionArgSource[actionId];

            throw new Exception($"没有找到 action {actionId} 对应的 CombatActionArgSource");
        }

        protected Dictionary<int, ActionInfo> CreateActions(List<int> actionsId)
        {
            var result = new Dictionary<int, ActionInfo>();

            foreach (var id in actionsId)
            {
                try
                {
                    var actionInfo = CreateActionInfo(id);
                    result.Add(id, actionInfo);
                }
                catch (Exception e)
                {
                    if (logger != null)
                        logger.LogError(e.Message + " 创建action失败 " + id);
                }
            }

            return result;
        }

        protected ActionInfo CreateActionInfo(int actionId)
        {
            var actionInfo = new ActionInfo();

            var argSource = GetActionArgSource(actionId);
            if (argSource == null)
                throw new Exception("没有找到指定actionid 的argsource " + actionId);

            actionInfo.type = argSource.GetActionType();
            actionInfo.mode = argSource.GetActionMode();
            actionInfo.groupId = argSource.GetActionGroupId();
            actionInfo.sortId = argSource.GetActionSortId();
            actionInfo.bulletSpeed = argSource.GetBulletSpeed();
            actionInfo.uid = Guid.NewGuid().ToString();

            var dicComponentInfo = new Dictionary<ActionComponentType, List<ActionComponentInfo>>();

            //条件finder
            var conditionFinders = new List<ActionComponentInfo>();
            var conditionFindersId = argSource.GetConditionFindersId();
            for (int i = 0; i < conditionFindersId.Length; i++)
            {
                var id = conditionFindersId[i];
                var index = i;
                var conditionFinder = new ActionComponentInfo() { id = id, args = argSource.GetConditionFindersArgs(index) }; //时间触发器， 时长
                conditionFinders.Add(conditionFinder);
            }

            dicComponentInfo.Add(ActionComponentType.ConditionFinder, conditionFinders);


            //条件触发器
            var conditionTriggers = new List<ActionComponentInfo>();
            var conditionTriggersId = argSource.GetConditionTriggersId();
            for (int i = 0; i < conditionTriggersId.Length; i++)
            {
                var id = conditionTriggersId[i];
                var index = i;
                var conditionTrigger = new ActionComponentInfo() { id = id, args = argSource.GetConditionTriggersArgs(index) }; //查找最近单位触发器 攻击距离， 查找个数
                conditionTriggers.Add(conditionTrigger);
            }
            dicComponentInfo.Add(ActionComponentType.ConditionTrigger, conditionTriggers);

            //延迟触发器
            var delayTriggers = new List<ActionComponentInfo>();
            var delayTrigger = new ActionComponentInfo() { id = argSource.GetDelayTriggerId(), args = argSource.GetDelayTriggerArgs() }; //时间触发器， 时长
            delayTriggers.Add(delayTrigger);
            dicComponentInfo.Add(ActionComponentType.DelayTrigger, delayTriggers);



            //公式计算器
            var formulas = new List<ActionComponentInfo>();
            if (argSource.GetFormulaId() != 0)
            {
                var formula = new ActionComponentInfo() { id = argSource.GetFormulaId(), args = argSource.GetFormulaArgs() }; //
                formulas.Add(formula);
            }
            dicComponentInfo.Add(ActionComponentType.ExecuteFormulator, formulas);


            //查找器
            var finders = new List<ActionComponentInfo>();
            var findersId = argSource.GetFindersId();
            for (int i = 0; i < findersId.Length; i++)
            {
                var id = findersId[i];
                var index = i;
                var finder = new ActionComponentInfo() { id = id, args = argSource.GetFindersArgs(index) }; //查找最近单位触发器 攻击距离， 查找个数
                finders.Add(finder);
            }
            dicComponentInfo.Add(ActionComponentType.ExecutorFinder, finders);

            //执行器
            var executors = new List<ActionComponentInfo>();
            var executorsId = argSource.GetExecutorsId();
            for (int i = 0; i < executorsId.Length; i++)
            {
                var id = executorsId[i];
                var index = i;
                var executor = new ActionComponentInfo() { id = id, args = argSource.GetExecutorsArgs(index) };//伤害触发器， 时长， 伤害倍率
                executors.Add(executor);
            }
            dicComponentInfo.Add(ActionComponentType.Executor, executors);


            //cd触发器
            var cdTriggers = new List<ActionComponentInfo>();
            var cdTriggersId = argSource.GetCdTriggersId();
            for (int i = 0; i < cdTriggersId.Length; i++)
            {
                var id = cdTriggersId[i];
                var index = i;
                var cdTrigger = new ActionComponentInfo() { id = id, args = argSource.GetCdTriggersArgs(index) };//时间触发器， 时长
                cdTriggers.Add(cdTrigger);
            }
            dicComponentInfo.Add(ActionComponentType.CdTrigger, cdTriggers);

            actionInfo.componentInfo = dicComponentInfo;
            //actionsData.Add(actionId, actionInfo);//actionID

            return actionInfo;
        }

    }
}