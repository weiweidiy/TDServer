using System;
using System.Collections.Generic;
using System.Reflection;

namespace JFramework
{
    public class CombatActionFactory
    {
        /// <summary>
        /// 創建action列表 , key:actionId
        /// </summary>
        /// <param name="actionsData"></param>
        /// <param name="owner"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public List<CombatAction> CreateActions(Dictionary<int, ActionInfo> actionsInfo, IActionOwner owner, CombatContext context, float readyCd = 0f)
        {
            if (actionsInfo == null)
                return null;

            var result = new List<CombatAction>();
            foreach (var action in actionsInfo)
            {
                try
                {
                    var actionId = action.Key;
                    var actionData = action.Value;
                    var actionType = actionData.type;
                    var actionMode = actionData.mode;
                    var actionUid = actionData.uid;
                    var actionGroupId = actionData.groupId;
                    var actionSortId = actionData.sortId;
                    var actionBulletSpeed = actionData.bulletSpeed;

                    var dic = actionData.componentInfo;
                    var conditionFinders = dic[ActionComponentType.ConditionFinder]; //条件查找器
                    var conditionTriggers = dic[ActionComponentType.ConditionTrigger]; //條件觸發器
                    var delayTriggers = dic[ActionComponentType.DelayTrigger];//延遲觸發器(只能有1個)
                    var executorfinders = dic[ActionComponentType.ExecutorFinder]; //执行查找器
                    var executorFormulas = dic[ActionComponentType.ExecuteFormulator];
                    var executors = dic[ActionComponentType.Executor]; //執行器
                    var cdTriggers = dic[ActionComponentType.CdTrigger]; //cd觸發器
                    var unitAction = new CombatUnitAction();
                    unitAction.OnAttach(owner);
                    var sm = new CombatActionSM();
                    sm.Initialize(unitAction);

                    //var conditionFinder = conditionFinders.Count > 0 ? conditionFinders[0] : null;
                    var delayTrigger = delayTriggers.Count > 0 ? delayTriggers[0] : null;
                    var executorFinder = executorfinders.Count > 0 ? executorfinders[0] : null;
                    var executorFormula = executorFormulas.Count > 0 ? executorFormulas[0] : null;

                    //UnityEngine.Debug.LogError("readycd = " + readyCd);
                    var readyCdTrigger = readyCd == 0f ? null : CreateReadyTrigger(context, new float[] { readyCd });
                    //UnityEngine.Debug.LogError("readycd = " + readyCd);

                    unitAction.Initialize(context, actionId, actionUid, actionType, actionMode, actionGroupId, actionSortId
                                , readyCdTrigger
                                , CreateConditionTriggers(conditionTriggers, CreateFinders(conditionFinders, context, unitAction), context, unitAction) //条件触发器
                                , CreateTrigger(delayTrigger, null, context, unitAction) //延迟触发器
                                , CreateExecutors(executors, CreateFinder(executorFinder, context, unitAction), CreateFormula(executorFormula, context, unitAction), context, unitAction) //执行器
                                , CreateCdTriggers(cdTriggers, context, unitAction), sm, actionBulletSpeed); //cd触发器

                    result.Add(unitAction);
                }
                catch (Exception ex)
                {
                    if (context.Logger != null)
                        context.Logger.LogError(ex.Message + $" 创建action失败 检查配置 actionId:{action.Key}");

                    continue;
                }
            }
            return result;
        }

        CombatBaseTrigger CreateReadyTrigger(CombatContext context, float[] args)
        {
            var trigger = new TriggerTime(null);
            trigger.Initialize(context, args);
            return trigger;
        }

        /// <summary>
        /// 創建觸發器列表
        /// </summary>
        /// <param name="conditionTriggers"></param>
        /// <param name="finders"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<CombatBaseTrigger> CreateConditionTriggers(List<ActionComponentInfo> conditionTriggers, List<CombatBaseFinder> finders, CombatContext context, CombatAction owner)
        {
            var result = new List<CombatBaseTrigger>();

            foreach (var componentInfo in conditionTriggers)
            {
                var trigger = CreateTrigger(componentInfo, finders, context, owner);
                result.Add(trigger);
            }

            return result;
        }

        /// <summary>
        /// 創建finder列表
        /// </summary>
        /// <param name="finders"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<CombatBaseFinder> CreateFinders(List<ActionComponentInfo> finders, CombatContext context, CombatAction owner)
        {
            var result = new List<CombatBaseFinder>();

            foreach (var componentInfo in finders)
            {
                var finder = CreateFinder(componentInfo, context, owner);
                result.Add(finder);
            }

            return result;
        }

        /// <summary>
        /// 創建執行器列表
        /// </summary>
        /// <param name="executors"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<CombatBaseExecutor> CreateExecutors(List<ActionComponentInfo> executors, CombatBaseFinder finder, CombatBaseFormula formula, CombatContext context, CombatAction owner)
        {
            var result = new List<CombatBaseExecutor>();

            foreach (var componentInfo in executors)
            {
                var executor = CreateExecutor(componentInfo, finder, formula, context, owner);
                result.Add(executor);
            }

            return result;
        }

        /// <summary>
        /// 創建cd觸發器列表
        /// </summary>
        /// <param name="cdTriggers"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<CombatBaseTrigger> CreateCdTriggers(List<ActionComponentInfo> cdTriggers, CombatContext context, CombatAction owner)
        {
            var result = new List<CombatBaseTrigger>();

            foreach (var componentInfo in cdTriggers)
            {
                var trigger = CreateTrigger(componentInfo, null, context, owner);
                result.Add(trigger);
            }

            return result;
        }

        /// <summary>
        /// 創建觸發器
        /// </summary>
        /// <param name="componentInfo"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        CombatBaseTrigger CreateTrigger(ActionComponentInfo componentInfo, List<CombatBaseFinder> finders, CombatContext context, CombatAction owner)
        {
            CombatBaseTrigger trigger = null;
            switch (componentInfo.id)
            {
                case 1:
                    {
                        trigger = new TriggerFinder(finders);
                    }
                    break;
                case 2:
                    {
                        trigger = new TriggerAmount(finders); //战斗开始时
                    }
                    break;
                case 3:
                    {
                        trigger = new TriggerTime(finders); //時長觸發器
                    }
                    break;
                case 4:
                    {
                        trigger = new TriggerUnitHurt(finders);//受傷觸發
                    }
                    break;
                case 5:
                    {
                        trigger = new TriggerActionCast(finders);
                    }
                    break;
                case 6:
                    {
                        trigger = new TriggerActionHitted(finders);
                    }
                    break;
                case 7:
                    {
                        trigger = new TriggerActionHitting(finders);
                    }
                    break;
                case 8:
                    {
                        trigger = new TriggerActionStartCd(finders);
                    }
                    break;
                default:
                    throw new NotImplementedException("沒有實現trigger組件類型 " + componentInfo.id);
            }
            //trigger.OnAttach(owner);
            trigger.Initialize(context, componentInfo.args);
            return trigger;
        }

        /// <summary>
        /// 創建finder
        /// </summary>
        /// <param name="componentInfo"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        CombatBaseFinder CreateFinder(ActionComponentInfo componentInfo, CombatContext context, CombatAction owner)
        {
            if (componentInfo == null)
                return null;

            CombatBaseFinder finder = null;
            switch (componentInfo.id)
            {
                case 1:
                    {
                        finder = new FinderFindSelf();
                    }
                    break;
                case 2:
                    {
                        finder = new FinderFindNearest();
                    }
                    break;
                case 3:
                    {
                        finder = new FinderFindFartest();
                    }
                    break;
                case 4:
                    {
                        finder = new FinderFindHpLessThanPercent();
                    }
                    break;
                case 5:
                    {
                        finder = new FinderFindRangeByTargets();
                    }
                    break;
                case 6:
                    {
                        finder = new FinderFindRangeByScreen();
                    }
                    break;
                case 7:
                    {
                        finder = new FinderFindUnitsActions();
                    }
                    break;
                case 8:
                    {
                        finder = new FinderFindSelfActions();
                    }
                    break;
                default:
                    throw new NotImplementedException("沒有實現finder組件類型 " + componentInfo.id);
            }
            //finder.OnAttach(owner);
            finder.Initialize(context, componentInfo.args);
            return finder;
        }

        /// <summary>
        /// 公式计算器
        /// </summary>
        /// <param name="componentInfo"></param>
        /// <param name="context"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        CombatBaseFormula CreateFormula(ActionComponentInfo componentInfo, CombatContext context, CombatAction owner)
        {
            if (componentInfo == null)
                return null;

            CombatBaseFormula formula = null;

            switch (componentInfo.id)
            {
                case 1:
                    {
                        formula = new CombatFormula1();
                    }
                    break;
                case 2:
                    {
                        formula = new CombatFormula2();
                    }
                    break;
                case 3:
                    {
                        formula = new CombatFormula3();
                    }
                    break;
                case 4:
                    {
                        formula = new CombatFormula4();
                    }
                    break;
                case 5:
                    {
                        formula = new CombatFormula5();
                    }
                    break;
                case 6:
                    {
                        formula = new CombatFormula6();
                    }
                    break;
                default:
                    throw new NotImplementedException("没有实现 formula id: " + componentInfo.id);
            }
            //formula.OnAttach(owner);
            formula.Initialize(context, componentInfo.args);
            return formula;
        }

        /// <summary>
        /// 創建執行器
        /// </summary>
        /// <param name="componentInfo"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        CombatBaseExecutor CreateExecutor(ActionComponentInfo componentInfo, CombatBaseFinder finder, CombatBaseFormula formula, CombatContext context, CombatAction owner)
        {
            CombatBaseExecutor executor = null;
            switch (componentInfo.id)
            {
                case 1:
                    {
                        executor = new ExecutorCombatDamage(finder, formula);
                    }
                    break;
                case 2:
                    {
                        executor = new ExecutorCombatContinuousDamage(finder, formula);
                    }
                    break;
                case 3:
                    {
                        executor = new ExecutorCombatHeal(finder, formula);
                    }
                    break;
                case 4:
                    {
                        executor = new ExecutorCombatChangeAttribute(finder, formula);
                    }
                    break;
                case 5:
                    {
                        executor = new ExecutorCombatAddBuffer(finder, formula);
                    }
                    break;
                case 6:
                    {
                        executor = new ExecutorCombatContinuousHeal(finder, formula);
                    }
                    break;
                case 7:
                    {
                        executor = new ExecutorCombatRemoveBuffer(finder, formula);
                    }
                    break;
                case 8:
                    {
                        executor = new ExecutorCombatIncreaseDamage(finder, formula);
                    }
                    break;
                case 9:
                    {
                        executor = new ExecutorCombatChangeActionArg(finder, formula);
                    }
                    break;
                case 10:
                    {
                        executor = new ExecutorCombatTurnBackDamage(finder, formula);
                    }
                    break;
                case 11:
                    {
                        executor = new ExecutorCombatChangeAttrByLevel(finder, formula);
                    }
                    break;
                case 12:
                    {
                        executor = new ExecutorCombatSingleThreadDamage(finder, formula);
                    }
                    break;
                default:
                    throw new NotImplementedException("沒有實現executor組件類型 " + componentInfo.id);
            }
            //executor.OnAttach(owner);
            executor.Initialize(context, componentInfo.args);
            return executor;
        }
    }
}