using System;

namespace JFramework
{
    //public interface INewBuffer : IUnique , IAttachOwner , IAttachable
    //{
    //    /// <summary>
    //    /// 触发效果了
    //    /// </summary>
    //    event Action<IBuffer> onCast;

    //    /// <summary>
    //    /// Id
    //    /// </summary>
    //    int Id { get; }

    //    /// <summary>
    //    /// 叠加层数
    //    /// </summary>
    //    int FoldCount { get; }

    //    /// <summary>
    //    /// 添加buffer层数
    //    /// </summary>
    //    /// <param name="foldCount"></param>
    //    void AddFoldCount(int foldCount);

    //    /// <summary>
    //    /// 参数列表
    //    /// </summary>
    //    float[] Args { get; set; }

    //    /// <summary>
    //    /// buffer是否有效（时间到了，或者数值消耗完了等等）
    //    /// </summary>
    //    bool IsValid();

    //    /// <summary>
    //    /// 更新帧
    //    /// </summary>
    //    void Update(BattleFrame frame);

    //    /// <summary>
    //    /// 触发实际效果
    //    /// </summary>
    //    /// <returns></returns>
    //    float Cast();

    //    /// <summary>
    //    /// 是否能释放
    //    /// </summary>
    //    /// <returns></returns>
    //    bool CanCast();
    //}






    public interface IBuffer :  IUnique , IAttachOwner
    {
        /// <summary>
        /// 触发效果了
        /// </summary>
        event Action<IBuffer> onCast;

        ///// <summary>
        ///// Id
        ///// </summary>
        // int Id { get;  }

        bool IsBuff();

        /// <summary>
        /// 叠加层数
        /// </summary>
         int FoldCount { get;  }

        /// <summary>
        /// 添加buffer层数
        /// </summary>
        /// <param name="foldCount"></param>
         void AddFoldCount(int foldCount);

        /// <summary>
        /// 参数列表
        /// </summary>
         float[] Args { get; set; }

        /// <summary>
        /// buffer是否有效（时间到了，或者数值消耗完了等等）
        /// </summary>
        bool IsValid();

        /// <summary>
        /// 被添加上时
        /// </summary>
        void OnAttach(IBattleUnit unit);

        /// <summary>
        /// 被移除时
        /// </summary>
        void OnDettach();

        /// <summary>
        /// 更新帧
        /// </summary>
        void Update(CombatFrame frame);

        /// <summary>
        /// 触发实际效果
        /// </summary>
        /// <returns></returns>
        float Cast();

        /// <summary>
        /// 是否能释放
        /// </summary>
        /// <returns></returns>
        bool CanCast();
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