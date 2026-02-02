using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;

namespace JFramework
{
    public abstract class Buffer : IBuffer
    {

        public event Action<IBuffer> onCast;

        protected void NotifyOnCast(IBuffer buffer)
        {
            onCast?.Invoke(buffer);
        }

        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 唯一ID
        /// </summary>
        public virtual string Uid { get; private set; }

        /// <summary>
        /// 叠加层数
        /// </summary>
        public int FoldCount { get; private set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public float[] Args { get; set; }

        /// <summary>
        /// 目标对象
        /// </summary>
        public virtual IBattleUnit Owner { get; private set; }

        /// <summary>
        /// 释放者
        /// </summary>
        protected IBattleUnit Caster { get; private set; }

        /// <summary>
        /// 条件触发器
        /// </summary>
        public IBattleTrigger ConditionTrigger { get; private set; }

        /// <summary>
        /// 目标搜索器
        /// </summary>
        public IBattleTargetFinder finder { get; private set; }

        /// <summary>
        /// 效果执行器
        /// </summary>
        public List<IBattleExecutor> exeutors { get; private set; }

        public string Name => throw new NotImplementedException();

        protected bool isValid;

        bool isBuff;

        ActionMode actionMode;

        public Buffer(IBattleUnit caster, bool isBuff, int buffType, string UID, int id, int foldCount, float[] args,  IBattleTrigger trigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors)
        {
            Id = id;
            this.Uid = UID;
            this.Args = args;
            this.FoldCount = foldCount;
            this.ConditionTrigger = trigger;
            this.finder = finder;
            this.exeutors = exutors;
            this.Caster = caster;
            this.isBuff = isBuff;
            actionMode = (ActionMode)buffType;

            if (exeutors != null)
            {
                foreach (var executor in exeutors)
                {
                    executor.onHittingTarget += Executor_onHittingTarget;
                }
            }

            isValid = true;
        }

        /// <summary>
        /// 被添加上时
        /// </summary>
        public virtual void OnAttach(IBattleUnit target)
        {
            this.Owner = target;

            if (finder != null)
                finder.OnAttach(this);

            if (exeutors != null)
            {
                foreach (var executor in exeutors)
                {
                    executor.OnAttach(this);
                }
            }

            if (ConditionTrigger != null)
            {
                ConditionTrigger.OnAttach(this);
                ConditionTrigger.onTriggerOn += ConditionTrigger_onTriggerOn; //通过事件触发会直接触发finder和执行器，不会等待下一帧
            }
        }



        /// <summary>
        /// 被移除时
        /// </summary>
        public virtual void OnDettach()
        {
            if (ConditionTrigger != null)
            {
                ConditionTrigger.OnDetach();
            }


            if (finder != null)
                finder.OnDetach();

            if (exeutors != null)
            {
                foreach (var executor in exeutors)
                {
                    executor.OnDetach();
                }
            }
        }

        /// <summary>
        /// 触发器触发了（一半是需要立即执行的）
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void ConditionTrigger_onTriggerOn(IBattleTrigger arg1, object[] arg2)
        {
            var targets = finder.FindTargets(arg2);
            if (targets == null || targets.Count == 0)
            {
                ConditionTrigger.Restart();
                return;
            }

            foreach (var e in exeutors)
            {
                // to do: ibattleaction接口参数要替换成iattachowner
                //e.ReadyToExecute(Caster, null, targets, arg2);
                e.Hit(Caster, null, targets, arg2);
                e.Executing = false;
            }

            ConditionTrigger.Restart();
        }


        /// <summary>
        /// buffer 执行效果命中
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="info"></param>
        private void Executor_onHittingTarget(IBattleUnit unit, ExecuteInfo info)
        {
            
        }

        /// <summary>
        /// 设置是否有效
        /// </summary>
        /// <param name="valid"></param>
        public void SetValid(bool valid)
        {
            isValid = valid;
        }

        /// <summary>
        /// 释放
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public float Cast()
        {
            //释放
            var targets = finder.FindTargets(null);
            if (targets == null || targets.Count == 0)
                throw new Exception("buff cast 没有找到有效目标 " + Id);

            foreach (var e in exeutors)
            {
                // to do: ibattleaction接口参数要替换成iattachowner
                e.ReadyToExecute(Caster, null, targets);
            }

            ConditionTrigger.SetOn(false);

            return 0f;
        }

        /// <summary>
        /// 是否可以释放
        /// </summary>
        /// <returns></returns>
        public bool CanCast()
        {
            return ConditionTrigger.IsOn();
        }

        /// <summary>
        /// buffer是否有效（时间到了，或者数值消耗完了等等）
        /// </summary>
        public virtual bool IsValid()
        {
            return isValid;
        }

    
        /// <summary>
        /// 更新帧
        /// </summary>
        public virtual void Update(CombatFrame frame)
        {
            if (ConditionTrigger != null)
                ConditionTrigger.Update(frame);

            if(exeutors != null)
            {
                foreach (var e in exeutors)
                {
                    if(e.Executing && actionMode == ActionMode.Active)
                    {
                        e.Update(frame);
                    }
                }
            }

        }

        /// <summary>
        /// 添加层数
        /// </summary>
        /// <param name="foldCount"></param>
        public virtual void AddFoldCount(int foldCount)
        {
            FoldCount += foldCount;

            if (ConditionTrigger != null)
                ConditionTrigger.OnUpdate();
        }

        /// <summary>
        /// 获取层数
        /// </summary>
        /// <returns></returns>
        public float GetFoldCount()
        {
            return FoldCount;
        }

        /// <summary>
        /// 获取周期
        /// </summary>
        /// <returns></returns>
        public abstract float GetDuration();

        /// <summary>
        /// 设置条件触发器参数
        /// </summary>
        /// <param name="args"></param>
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
            //if (cdTrigger != null)
            //    cdTrigger.SetArgs(args);
        }

        public bool IsBuff()
        {
            return isBuff;
        }
    }
}




///// <summary>
///// Id
///// </summary>
//public int Id { get; private set; }

///// <summary>
///// 唯一id
///// </summary>
//public string Uid { get; private set; }

///// <summary>
///// buff类型
///// </summary>
//public BufferTriggerType BufferType { get; private set; }

///// <summary>
///// 是否是BUFF
///// </summary>
//public bool IsBuff { get; protected set; }

///// <summary>
///// buff参数
///// </summary>
//public float Arg { get; set; }

///// <summary>
///// 层数
///// </summary>
//public int FoldCount { get; set; }

///// <summary>
///// 剩余周期
///// </summary>
//public float RestDuration { get; protected set; }

///// <summary>
///// buffer是否生效
///// </summary>
//bool isValid;

///// <summary>
///// 生命周期
///// </summary>
//protected float duration;

///// <summary>
///// 拥有者
///// </summary>
//protected IBattleUnit owner;

///// <summary>
///// 最大值
///// </summary>
//protected int maxValue;

//public Buffer(int id, string uid, BufferTriggerType bufferType, float arg, float duration, int fold, int maxValue, IBattleUnit owner = null)
//{
//    Id = id;
//    Uid = uid;
//    BufferType = bufferType;
//    Arg = arg;
//    FoldCount = fold;
//    this.owner = owner;
//    this.duration = duration;
//    this.maxValue = maxValue;

//    OnValid();

//    if (duration != -1)
//    {
//        //tweenerDuration = DotweenManager.Ins.DOTweenDelay(duration, 1, () =>
//        //{
//        //    if (isValid)
//        //        OnInValid();

//        //    onBuffInValid?.Invoke(this);
//        //});
//    }
//}

///// <summary>
///// 重置周期
///// </summary>
//public void Reset(float duration)
//{
//    this.duration = duration;
//    //tweenerDuration?.Kill();
//    if (duration != -1)
//    {
//        isValid = true;
//        //tweenerDuration = DotweenManager.Ins.DOTweenDelay(duration, 1, () =>
//        //{
//        //    if (isValid)
//        //        OnInValid();

//        //    onBuffInValid?.Invoke(this);
//        //});
//    }

//}


///// <summary>
///// buff生效（周期开始）
///// </summary>
//protected virtual void OnValid()
//{
//    isValid = true;
//}

///// <summary>
///// buff失效（周期到了）
///// </summary>
//protected virtual void OnInValid()
//{
//    isValid = false;
//}

///// <summary>
///// 值加成
///// </summary>
///// <param name="value"></param>
///// <returns></returns>
//public abstract int Buff(int value);

///// <summary>
///// 值加成
///// </summary>
///// <param name="value"></param>
///// <returns></returns>
//public abstract float Buff(float value);

//public virtual void Release()
//{
//    //tweenerDuration?.Kill();

//    if (isValid)
//        OnInValid();
//}