using System;
using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// 普通的一次行动逻辑 (瞬时动作类型)
    /// </summary>
    public abstract class BaseAction : IBattleAction, IUpdateable
    {
        #region 委托
        /// <summary>
        /// 可以触发了
        /// </summary>
        public event Action<IBattleAction> onCanCast;

        /// <summary>
        /// 开始释放了
        /// </summary>
        public event Action<IBattleAction, List<IBattleUnit>, float> onStartCast;

        /// <summary>
        /// 开始CD了
        /// </summary>
        public event Action<IBattleAction, float> onStartCD;

        /// <summary>
        /// 即将命中目标
        /// </summary>
        public event Action<IBattleAction, IBattleUnit, ExecuteInfo> onHittingTarget;

        /// <summary>
        /// 已经命中
        /// </summary>
        public event Action<IBattleAction, IBattleUnit, ExecuteInfo, IBattleUnit> onHittedComplete;

        public void NotifyCanCast()
        {
            onCanCast?.Invoke(this);
        }

        public void NotifyStartCast(List<IBattleUnit> targets, float duration)
        {
            onStartCast?.Invoke(this, targets, duration);
        }

        public void NotifyStartCD(float cd)
        {
            onStartCD?.Invoke(this, cd);
        }
        #endregion

        #region 属性字段
        /// <summary>
        /// 动作名称
        /// </summary>
        public string Name => nameof(BaseAction);

        /// <summary>
        /// 动作ID
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 动作类型 主动 排队触发：被动：各自触发
        /// </summary>
        public virtual ActionType Type { get; private set; }

        /// <summary>
        /// 技能模式
        /// </summary>
        public abstract ActionMode Mode { get; }

        /// <summary>
        /// 拥有者
        /// </summary>
        public IBattleUnit Owner { get; private set; }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public string Uid { get; private set; }

        /// <summary>
        /// 是否持续时间
        /// </summary>
        float castDuration;

        /// <summary>
        /// 条件触发器
        /// </summary>
        public IBattleTrigger ConditionTrigger { get; private set; }

        /// <summary>
        /// 冷却触发器
        /// </summary>
        public IBattleTrigger cdTrigger { get; private set; }

        /// <summary>
        /// 目标搜索器
        /// </summary>
        public IBattleTargetFinder finder { get; private set; }

        /// <summary>
        /// 效果执行器
        /// </summary>
        public List<IBattleExecutor> exeutors { get; private set; }



        /// <summary>
        /// 状态机
        /// </summary>
        OldActionSM sm;

        #endregion

        /// <summary>
        /// 常规动作逻辑，触发器触发->搜索敌人->执行效果
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trigger"></param>
        /// <param name="finder"></param>
        /// <param name="exutor"></param>
        public BaseAction(string UID, int id, ActionType type, float duration, IBattleTrigger trigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors, IBattleTrigger cdTrigger, OldActionSM sm)
        {
            this.Type = type;
            this.castDuration = duration;
            this.Uid = UID;
            this.Id = id;
            this.ConditionTrigger = trigger;
            this.finder = finder;
            this.exeutors = exutors;
            this.cdTrigger = cdTrigger;

            this.sm = sm;
            this.sm.Initialize(this);

            if (exeutors != null)
            {
                foreach (var executor in exeutors)
                {
                    executor.onHittingTarget += Executor_onHittingTarget;
                    executor.onHittedComplete += Executor_onHittedComplete;
                }
            }

        }



        /// <summary>
        /// 附加到单位上
        /// </summary>
        /// <param name="owner"></param>
        public void OnAttach(IBattleUnit owner)
        {
            Owner = owner;


            if (finder != null)
                finder.OnAttach(this);

            if (cdTrigger != null)
                cdTrigger.OnAttach(this);

            if (exeutors != null)
            {
                foreach (var executor in exeutors)
                {
                    executor.OnAttach(this);
                }
            }

            if (ConditionTrigger != null)
            {
                ConditionTrigger.onTriggerOn += ConditionTrigger_onTriggerOn; //通过事件触发会直接触发finder和执行器，不会等待下一帧
                ConditionTrigger.OnAttach(this);
            }

            sm.SwitchToStandby();
        }

        /// <summary>
        /// 直接独立执行
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <exception cref="Exception"></exception>
        
        private void ConditionTrigger_onTriggerOn(IBattleTrigger arg1, object[] arg2)
        {
            if (GetCurState() == nameof(ActionCding))
                return;

            var targets = finder.FindTargets(arg2);
            if (targets == null || targets.Count == 0)
            {
                ConditionTrigger.Restart();
                return;
            }

            EnterCD();

            foreach (var e in exeutors)
            {
                // to do: ibattleaction接口参数要替换成iattachowner
                e.Hit(Owner, this, targets, arg2);
                //if (Id == 940420)
                //    Debug.LogError("940420反射");
                //打完了
            }



            //ConditionTrigger.Restart();
        }

        /// <summary>
        /// 即将命中目标
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Executor_onHittingTarget(IBattleUnit target, ExecuteInfo obj)
        {
            onHittingTarget?.Invoke(this, target, obj);
        }

        /// <summary>
        /// 已经命中目标
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="info"></param>
        /// <param name="target"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Executor_onHittedComplete(IBattleUnit caster, ExecuteInfo info, IBattleUnit target)
        {
            onHittedComplete?.Invoke(this, caster, info, target);
        }

        /// <summary>
        /// 更新帧
        /// </summary>
        /// <param name="frame"></param>
        public void Update(CombatFrame frame)
        {
            sm.Update(frame);
        }

        #region 状态切换
        /// <summary>
        /// 进入待机状态
        /// </summary>
        public void Standby()
        {
            sm.SwitchToStandby();
        }


        /// <summary>
        /// 向指定目标释放
        /// </summary>
        /// <param name="units"></param>
        /// <exception cref="System.NotImplementedException"></exception>
        public float Cast()
        {
            sm.SwitchToExecuting();
            return GetCastDuration();
        }

        /// <summary>
        /// 进入CD
        /// </summary>
        public void EnterCD()
        {
            sm.SwitchToCding();
        }
        #endregion


        /// <summary>
        /// 是否cd完成
        /// </summary>
        /// <returns></returns>
        public bool IsCDComplete()
        {
            return cdTrigger.IsOn();
        }

        /// <summary>
        /// 动作是否在执行中
        /// </summary>
        /// <returns></returns>
        public bool IsExecuting()
        {
            foreach (var executor in exeutors)
            {
                if (executor.Executing) return true;
            }
            return false;
        }

        /// <summary>
        /// 是否满足释放条件
        /// </summary>
        /// <returns></returns>
        public bool CanCast()
        {
            return ConditionTrigger.IsOn();
        }

        /// <summary>
        /// 设置是否是死亡状态（有些触发器要在死亡状态生效）
        /// </summary>
        /// <param name="dead"></param>
        public void SetDead(bool dead)
        {
            if (!dead)
            {
                sm.SwitchToCding(); //直接进入cd
            }
            else
            {
                sm.SwitchToDead();
            }
        }

        /// <summary>
        /// 搜索目标
        /// </summary>
        /// <returns></returns>
        public List<IBattleUnit> FindTargets(object[] args)
        {
            return finder.FindTargets(args);
        }

        /// <summary>
        /// 准备执行效果
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="targets"></param>
        public void ReadyToExecute(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets)
        {
            foreach (var e in exeutors)
            {
                e.ReadyToExecute(caster, this, targets);
            }
        }

        /// <summary>
        /// 获取释放周期
        /// </summary>
        /// <returns></returns>
        public float GetCastDuration()
        {
            return castDuration;
        }

        /// <summary>
        /// 获取当前状态
        /// </summary>
        /// <returns></returns>
        public string GetCurState()
        {
            return sm.GetCurState();
        }

        /// <summary>
        /// 冷却触发器
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IBattleTrigger GetCDTrigger()
        {
            return cdTrigger;
        }

        /// <summary>
        /// 打断
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Interrupt()
        {
            //进入CD状态
            if (GetCurState() == nameof(ActionCding))
                return;

            EnterCD();
        }

        /// <summary>
        /// 设置动作是否可用
        /// </summary>
        /// <param name="enable"></param>
        public void SetEnable(bool enable)
        {
            ConditionTrigger.SetEnable(enable);
        }

        public float GetFoldCount()
        {
            return 1f;
        }

        public float GetDuration()
        {
            return float.MaxValue;
        }

        public void SetValid(bool valid)
        {

        }

        public void SetConditionTriggerArgs(float[] args)
        {
            if (ConditionTrigger != null)
                ConditionTrigger.SetArgs(args);
        }

        public void SetFinderArgs(float[] args)
        {
            if (finder != null)
                finder.SetArgs(args);
        }

        public void SetExecutorArgs(float[] args)
        {
            if (exeutors != null)
            {
                foreach (var executor in exeutors)
                {
                    executor.SetArgs(args);
                }
            }
        }

        public void SetCdArgs(float[] args)
        {
            if (cdTrigger != null)
                cdTrigger.SetArgs(args);
        }

        public void Update(IUpdateable value)
        {
            throw new NotImplementedException();
        }
    }
}