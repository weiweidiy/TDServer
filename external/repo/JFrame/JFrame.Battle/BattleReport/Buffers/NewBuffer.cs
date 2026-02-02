using System;
using System.Collections.Generic;

namespace JFramework
{

    //public abstract class NewBuffer : INewBuffer
    //{
    //    public event Action<IBuffer> onCast;

    //    protected void NotifyOnCast(IBuffer buffer)
    //    {
    //        onCast?.Invoke(buffer);
    //    }

    //    /// <summary>
    //    /// Id
    //    /// </summary>
    //    public int Id { get; private set; }

    //    /// <summary>
    //    /// 唯一ID
    //    /// </summary>
    //    public virtual string Uid { get; private set; }

    //    /// <summary>
    //    /// 叠加层数
    //    /// </summary>
    //    public int FoldCount { get; private set; }

    //    /// <summary>
    //    /// 参数列表
    //    /// </summary>
    //    public float[] Args { get; set; }

    //    ///// <summary>
    //    ///// 目标对象
    //    ///// </summary>
    //    public IAttachOwner Owner { get; private set; }

    //    /// <summary>
    //    /// 释放者
    //    /// </summary>
    //    protected IBattleUnit Caster;


    //    /// <summary>
    //    /// 条件触发器
    //    /// </summary>
    //    public IBattleTrigger ConditionTrigger { get; private set; }

    //    /// <summary>
    //    /// 目标搜索器
    //    /// </summary>
    //    public IBattleTargetFinder finder { get; private set; }

    //    /// <summary>
    //    /// 效果执行器
    //    /// </summary>
    //    public List<IBattleExecutor> exeutors { get; private set; }



    //    public NewBuffer(IBattleUnit caster, string UID, int id, int foldCount, IBattleTrigger trigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors)
    //    {
    //        Id = id;
    //        this.Uid = UID;
    //        this.FoldCount = foldCount;
    //        this.Caster = caster;
    //        this.ConditionTrigger = trigger;
    //        this.finder = finder;
    //        this.exeutors = exutors;

    //        if (exeutors != null)
    //        {
    //            foreach (var executor in exeutors)
    //            {
    //                executor.onHittingTarget += Executor_onHittingTarget;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// buffer 执行效果命中
    //    /// </summary>
    //    /// <param name="unit"></param>
    //    /// <param name="info"></param>
    //    private void Executor_onHittingTarget(IBattleUnit unit, ExecuteInfo info)
    //    {

    //    }

    //    public float Cast()
    //    {
    //        //释放
    //        var targets = finder.FindTargets();
    //        if (targets == null || targets.Count == 0)
    //            throw new Exception("buff cast 没有找到有效目标 " + Id);

    //        foreach(var e in exeutors)
    //        {
    //            // to do: ibattleaction接口参数要替换成iattachowner
    //            e.ReadyToExecute(Caster, null, targets);
    //        }

    //        ConditionTrigger.SetOn(false);

    //        return 0f;
    //    }

    //    public bool CanCast()
    //    {
    //        return ConditionTrigger.IsOn();
    //    }


    //    /// <summary>
    //    /// buffer是否有效（时间到了，或者数值消耗完了等等）
    //    /// </summary>
    //    public abstract bool IsValid();



    //    public void OnAttach(IAttachOwner target)
    //    {
    //        this.Owner = target;

    //        if (ConditionTrigger != null)
    //        {
    //            ConditionTrigger.OnAttach(this);
    //        }
                

    //        if (finder != null)
    //            finder.OnAttach(this);

    //        if (exeutors != null)
    //        {
    //            foreach (var executor in exeutors)
    //            {
    //                executor.OnAttach(this);
    //            }
    //        }
    //    }

    //    public void OnDetach()
    //    {
    //        //throw new NotImplementedException();
    //    }

    //    /// <summary>
    //    /// 更新帧
    //    /// </summary>
    //    public virtual void Update(BattleFrame frame)
    //    {
    //        if (ConditionTrigger != null)
    //            ConditionTrigger.Update(frame);
    //    }

    //    /// <summary>
    //    /// 添加层数
    //    /// </summary>
    //    /// <param name="foldCount"></param>
    //    public void AddFoldCount(int foldCount)
    //    {
    //        FoldCount += foldCount;
    //    }


    //}
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