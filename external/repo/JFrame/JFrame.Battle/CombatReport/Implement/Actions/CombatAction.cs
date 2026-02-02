//using Framework.Extension;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace JFramework
{

    public class CombatAction : ICombatAction, ICombatUpdatable, IUnique, IActionOwner, IUpdateable
    {
        public event Action<CombatExtraData> onTriggerOn;
        public event Action<CombatExtraData> onStartExecuting;
        public event Action<CombatExtraData> onStartCD;
        public event Action<CombatExtraData> onCDChanged; //當前cd變化了
        public event Action<CombatExtraData> onHittingTargets; //动作命中之前（单次所有目标）
        public event Action<CombatExtraData> onTargetsHittedComplete; //动作命中之后（单个目标）
        public event Action<CombatExtraData> onHittingTarget;
        public event Action<CombatExtraData> onTargetHittedComplete;

        public void NotifyTriggerOn()
        {
            onTriggerOn?.Invoke(ExtraData);
        }

        public void NotifyStartExecuting()
        {
            onStartExecuting?.Invoke(ExtraData);
        }

        public void NotifyStartCD()
        {
            onStartCD?.Invoke(ExtraData);
        }

        public void NotifyCDChanged()
        {
            onCDChanged?.Invoke(ExtraData);
        }

        public void NotifyHittingTargets(CombatExtraData extraData)
        {
            //ExtraData = extraData;
            onHittingTargets?.Invoke(extraData);
        }

        public void NotifyHittedTargets(CombatExtraData extraData)
        {
            //ExtraData = extraData;
            onTargetsHittedComplete?.Invoke(extraData);
        }

        public void NotifyHittingTarget(CombatExtraData extraData)
        {
            onHittingTarget?.Invoke(extraData);
        }

        public void NotifyHittedTarget(CombatExtraData extraData)
        {
            onTargetHittedComplete?.Invoke(extraData);
        }

        /// <summary>
        /// cd触发器参数发生改变
        /// </summary>
        /// <param name="extraData"></param>
        public void NotifyCdTriggerArgChanged(CombatExtraData extraData)
        {

        }

        public string Uid { get; private set; }

        public int Id { get; private set; }

        public ActionType Type { get; protected set; }

        public ActionMode Mode { get; private set; }

        public int GroupId { get; private set; }

        public int SortId { get; private set; }

        public float BulletSpeed { get; private set; }

        public CombatActionManager ActionManager { get; set; }

        /// <summary>
        /// 透傳對象
        /// </summary>
        CombatExtraData _extraData;
        public CombatExtraData ExtraData
        {
            get => _extraData;
            set
            {
                _extraData = value;
                _extraData.Action = this;


                if (conditionTriggers != null)
                {
                    foreach (var trigger in conditionTriggers)
                    {
                        trigger.ExtraData = _extraData.Clone() as CombatExtraData;
                    }
                }


                if (delayTrigger != null)
                    delayTrigger.ExtraData = _extraData.Clone() as CombatExtraData;

                if (cdTriggers != null)
                {
                    foreach (var trigger in cdTriggers)
                    {
                        trigger.ExtraData = _extraData.Clone() as CombatExtraData;
                    }
                }

            }
        }

        CombatBaseTrigger readyTrigger;

        /// <summary>
        /// 觸發器列表
        /// </summary>
        List<CombatBaseTrigger> conditionTriggers;

        /// <summary>
        /// 延遲出發器（從觸發到執行中間的延遲，有時間延遲類，還有距離/速度類等）
        /// </summary>
        CombatBaseTrigger delayTrigger;
        List<CombatBaseExecutor> executors;
        List<CombatBaseTrigger> cdTriggers;
        CombatActionSM sm;
        CombatContext context;
        /// <summary>
        /// 初始化actions
        /// </summary>
        /// <param name="conditionTriggers"></param>
        /// <param name="finder"></param>
        /// <param name="executors"></param>
        /// <param name="cdTriggers"></param>
        public void Initialize(CombatContext context, int id, string uid, ActionType type, ActionMode mode, int groupId, int sortId, CombatBaseTrigger readyCdTrigger, List<CombatBaseTrigger> conditionTriggers, CombatBaseTrigger delayTrigger, List<CombatBaseExecutor> executors, List<CombatBaseTrigger> cdTriggers, CombatActionSM sm, float bulletSpeed = 0f)
        {
            this.context = context;
            Uid = uid;
            this.readyTrigger = readyCdTrigger;
            //if (readyCdTrigger != null)
            //    UnityEngine.Debug.LogError("readyCdTrigger != null " + id);
            this.conditionTriggers = conditionTriggers;
            this.delayTrigger = delayTrigger;
            this.executors = executors;
            this.cdTriggers = cdTriggers;
            this.sm = sm;
            Id = id;

            this.Type = type;
            this.Mode = mode;
            this.GroupId = groupId;
            this.SortId = sortId;
            this.BulletSpeed = bulletSpeed;

            if (readyCdTrigger != null)
            {
                readyCdTrigger.OnAttach(this);
            }


            if (conditionTriggers != null)
            {
                foreach (var trigger in conditionTriggers)
                    trigger.OnAttach(this);
            }

            if (delayTrigger != null)
                delayTrigger.OnAttach(this);

            //监听执行器命中等消息
            if (executors != null)
            {
                foreach (var executor in executors)
                {
                    executor.onHittingTargets += Executor_onHittingTargets; ;
                    executor.onTargetsHittedComplete += Executor_onTargetsHittedComplete;
                    executor.onHittingTarget += Executor_onHittingTarget;
                    executor.onTargetHittedComplete += Executor_onTargetHittedComplete;
                    executor.OnAttach(this);
                }
            }

            if (cdTriggers != null)
            {
                foreach (var trigger in cdTriggers)
                    trigger.OnAttach(this);
            }
        }

        private void Executor_onTargetHittedComplete(CombatExtraData extraData)
        {
            NotifyHittedTarget(extraData);
        }

        private void Executor_onHittingTarget(CombatExtraData extraData)
        {
            NotifyHittingTarget(extraData);
        }

        private void Executor_onHittingTargets(CombatExtraData extraData)
        {
            NotifyHittingTargets(extraData);
        }

        private void Executor_onTargetsHittedComplete(CombatExtraData extraData)
        {
            NotifyHittedTargets(extraData);
        }

        public void Start()
        {
            readyTrigger?.OnStart();

            foreach (var trigger in conditionTriggers)
            {
                trigger.OnStart();
            }

            delayTrigger.OnStart();

            foreach (var executor in executors)
            {
                executor.OnStart();
            }

            foreach (var cdTrigger in cdTriggers)
            {
                cdTrigger.OnStart();
            }
        }

        public void Stop()
        {
            readyTrigger?.OnStop();

            foreach (var trigger in conditionTriggers)
            {
                trigger.OnStop();
            }

            delayTrigger.OnStop();

            foreach (var executor in executors)
            {
                executor.OnStop();
            }

            foreach (var cdTrigger in cdTriggers)
            {
                cdTrigger.OnStop();
            }
        }

        public void Update(CombatFrame frame)
        {
            try
            {
                sm.Update(frame);

                //context.combatBulletManager.Update(frame);

                //context.CombatManager.logger?.Log("frame : " + frame.CurFrame);
            }
            catch (Exception e)
            {
                if (context != null && context.Logger != null)
                    context.Logger.LogError(e.Message + $"  action执行失败，检查配置 actionID: {Id}");
            }

        }

        public void AddBullet(CombatBullet bullet)
        {
            context.combatBulletManager.AddItem(bullet);
        }

        public void RemvoeBullet(CombatBullet bullet)
        {
            context.combatBulletManager.RemoveItem(bullet);
        }

        #region 给状态机调用的接口
        /// <summary>
        /// 更新條件觸發器
        /// </summary>
        /// <param name="frame"></param>
        public void UpdateConditionTriggers(CombatFrame frame)
        {
            foreach (var trigger in conditionTriggers)
            {
                trigger.Update(frame);
            }
        }

        /// <summary>
        /// 是否已經觸發（只要有1个触发器触发了就触发）
        /// </summary>
        /// <returns></returns>
        public bool IsConditionTriggerOn()
        {
            foreach (var trigger in conditionTriggers)
            {
                if (trigger.IsOn())
                {
                    ExtraData = trigger.ExtraData;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void ResetConiditionTriggers()
        {
            if (conditionTriggers == null)
                return;

            foreach (var trigger in conditionTriggers)
            {
                trigger.Reset();
            }
        }

        public void EnterConditionTriggers()
        {
            if (conditionTriggers == null)
                return;
            foreach (var trigger in conditionTriggers)
            {
                trigger.OnEnterState();
            }
        }

        public void ExitConditionTriggers()
        {
            if (conditionTriggers == null)
                return;
            foreach (var trigger in conditionTriggers)
            {
                trigger.OnExitState();
            }
        }


        public void UpdateDelayTrigger(CombatFrame frame)
        {
            delayTrigger.Update(frame);
        }

        public bool IsDelayTriggerOn()
        {
            return delayTrigger.IsOn();
        }

        public void ResetDelayTrigger()
        {
            delayTrigger?.Reset();
        }

        public void SetDelayTriggerOriginArgs(float[] args)
        {
            delayTrigger?.SetOrginArgs(args);

            //if (context.Logger != null)
            //    context.Logger.Log("SetDelayTriggerOriginArgs " + args[0]);
        }

        #region 準備狀態，開始戰鬥的前置CD
        public void UpdateReadyCdTrigger(CombatFrame frame)
        {
            readyTrigger?.Update(frame);
        }

        public bool IsReadyCdTriggerOn()
        {
            return readyTrigger == null ? true : readyTrigger.IsOn();
        }

        public void ResetReadyCdTrigger()
        {
            readyTrigger?.Reset();
        }

        public void EnterReadyCdTriggers()
        {
            if (readyTrigger == null)
                return;
            readyTrigger.OnEnterState();
        }

        public void ExitReadyCdTriggers()
        {
            if (readyTrigger == null)
                return;
            readyTrigger.OnExitState();
        }
        #endregion

        public void DoExecutors()
        {
            if (executors == null)
                return;
            foreach (var executor in executors)
            {
                executor.Execute(ExtraData);
            }
        }

        public void UpdateExecutors(CombatFrame frame)
        {
            if (executors == null)
                return;
            foreach (var executor in executors)
            {
                executor.Update(frame);
            }
        }

        public void ResetExecutors()
        {
            if (executors == null)
                return;

            foreach (var executor in executors)
            {
                executor.Reset();
            }
        }

        public CombatBaseExecutor GetExecutor(int index)
        {
            return executors[index];
        }

        public void UpdateCdTriggers(CombatFrame frame)
        {
            foreach (var trigger in cdTriggers)
            {
                trigger.Update(frame);
            }
        }

        public void EnterCdTriggers()
        {
            if (cdTriggers == null)
                return;
            foreach (var trigger in cdTriggers)
            {
                trigger.OnEnterState();
            }
        }

        public void ExitCdTriggers()
        {
            if (cdTriggers == null)
                return;

            foreach (var trigger in cdTriggers)
            {
                trigger.OnExitState();
            }
        }

        /// <summary>
        /// 是否已經觸發 (必须所有触发器都触发才算触发）
        /// </summary>
        /// <returns></returns>
        public bool IsCdTriggerOn()
        {
            bool isCdTriggerOn = true;
            foreach (var trigger in cdTriggers)
            {
                if (!trigger.IsOn())
                {
                    isCdTriggerOn = false;
                }
            }
            return isCdTriggerOn;
        }

        public void ResetCdTriggers()
        {
            if (cdTriggers == null)
                return;

            foreach (var trigger in cdTriggers)
            {
                trigger.Reset();
            }
        }
        #endregion

        #region 状态机切换
        public void SwitchToDisable()
        {
            var curState = sm.GetCurState();
            if (curState.Name == nameof(ActionDisableState))
                return;

            sm.SwitchToDisable();
        }

        public void SwitchToTrigging()
        {
            sm.SwitchToStandby();
        }

        public void SwitchToReady()
        {
            sm.SwitchToReady();
        }

        public float SwitchToExecuting()
        {
            var duration = GetExecutingDuration();
            ExtraData.CastDuration = duration;

            sm.SwitchToExecuting();

            return duration;
        }

        public float SwitchToCd()
        {
            var duration = GetCdTriggerDuration();
            ExtraData.CdDuration = duration;
            sm.SwitchToCding();
            return duration;
        }

        /// <summary>
        /// 获取当前状态
        /// </summary>
        /// <returns></returns>
        public string GetCurState()
        {
            return sm.GetCurState().Name;
        }
        #endregion

        #region 操作组件数据
        /// <summary>
        /// 獲取執行時常
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public float GetExecutingDuration()
        {
            if (executors == null)
                return 0;

            float duration = 0f;
            foreach (var executor in executors)
            {
                var d = executor.GetDuration();
                if (d > duration)
                    duration = d;
            }
            return duration;
        }

        /// <summary>
        /// 獲取cd時長
        /// </summary>
        /// <returns></returns>
        public float GetCdTriggerDuration()
        {
            foreach (var trigger in cdTriggers)
            {

                if (trigger is TriggerTime)
                {
                    var triggerTime = trigger as TriggerTime;
                    return triggerTime.GetDuration();
                }
            }
            return 0f;
        }

        public float GetReadyCdTriggerDuration()
        {
            if (readyTrigger is TriggerTime)
            {
                var triggerTime = readyTrigger as TriggerTime;
                return triggerTime.GetDuration();
            }
            return 0f;
        }

        ///// <summary>
        ///// 设置cd时常
        ///// </summary>
        ///// <param name="duration"></param>
        //public void SetCdTriggerArg(float duration)
        //{
        //    foreach (var trigger in cdTriggers)
        //    {

        //        if (trigger is TriggerTime)
        //        {
        //            var triggerTime = trigger as TriggerTime;
        //            triggerTime.SetDuration(duration);
        //        }
        //    }
        //}

        /// <summary>
        /// 设置cd触发器参数
        /// </summary>
        /// <param name="triggerIndex"></param>
        /// <param name="argIndex"></param>
        /// <param name="argValue"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetCdTriggerArg(int triggerIndex, int argIndex, float argValue)
        {
            if (triggerIndex >= cdTriggers.Count)
                throw new ArgumentOutOfRangeException($"设置cd触发器时， triggerIndex {triggerIndex} 越界 , 检查cd触发器类型个数配置 actionId {Id}");

            var trigger = cdTriggers[triggerIndex];
            var argCount = trigger.GetValidArgsCount();
            if (argIndex >= argCount)
                throw new ArgumentOutOfRangeException($"设置cd触发器时， argIndex {argIndex} 越界 , 检查cd触发器参数个数配置 actionId {Id}");

            trigger.SetCurArg(argIndex, argValue);
        }

        /// <summary>
        /// 获取cd触发器指定参数
        /// </summary>
        /// <param name="triggerIndex"></param>
        /// <param name="argIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public float GetCdTriggerArg(int triggerIndex, int argIndex)
        {
            if (triggerIndex >= cdTriggers.Count)
                throw new ArgumentOutOfRangeException($"设置cd触发器时， triggerIndex {triggerIndex} 越界 , 检查cd触发器类型个数配置 actionId {Id}");

            var trigger = cdTriggers[triggerIndex];
            var argCount = trigger.GetValidArgsCount();
            if (argIndex >= argCount)
                throw new ArgumentOutOfRangeException($"设置cd触发器时， argIndex {argIndex} 越界 , 检查cd触发器参数个数配置 actionId {Id}");

            return trigger.GetCurArg(argIndex);
        }




        public void Update(IUpdateable value)
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// 恢复cd到原始值
        ///// </summary>
        //public void ResetCdTriggerDuration()
        //{
        //    foreach (var trigger in cdTriggers)
        //    {

        //        if (trigger is TriggerTime)
        //        {
        //            var triggerTime = trigger as TriggerTime;
        //            triggerTime.ResetArgs();
        //        }
        //    }
        //}

        #endregion

    }




}