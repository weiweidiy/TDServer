//using System.Collections.Generic;

//namespace JFrame
//{

//    /// <summary>
//    /// 护盾 参数 1： 持续时间， 参数2： 抵抗次数
//    /// </summary>
//    public class BufferShield : DurationBuffer
//    {
//        int amount;
//        public BufferShield(IBattleUnit caster, bool isBuff, string UID, int id, int foldCount, float[] args, IBattleTrigger trigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors) : base(caster, isBuff, UID, id, foldCount, args, trigger, finder, exutors)
//        {
//            if (args.Length < 2)
//                throw new System.Exception("BufferShield 参数不能少于2个");
//        }


//        public override void OnAttach(IBattleUnit unit)
//        {
//            base.OnAttach(unit);

//            amount = (int)GetValue();

//            unit.onDamaging += Unit_onDamaging;
//        }

//        public override void OnDettach()
//        {
//            base.OnDettach();

//            Owner.onDamaging -= Unit_onDamaging;
//        }


//        private void Unit_onDamaging(IBattleUnit caster, IBattleAction action, IBattleUnit hittee, ExecuteInfo value)
//        {
//            if(amount > 0)
//            {
//                amount--;
//                value.Value = 0;
//                value.IsGuard = true;
//            }
//        }

//        public virtual float GetValue()
//        {
//            return Args[1] * FoldCount;
//        }

//        ///// <summary>
//        ///// 是否有效
//        ///// </summary>
//        ///// <returns></returns>
//        //public override bool IsValid()
//        //{
//        //    return base.IsValid() && amount != 0;
//        //}


//    }
//}




/////// <summary>
/////// Id
/////// </summary>
////public int Id { get; private set; }

/////// <summary>
/////// 唯一id
/////// </summary>
////public string Uid { get; private set; }

/////// <summary>
/////// buff类型
/////// </summary>
////public BufferTriggerType BufferType { get; private set; }

/////// <summary>
/////// 是否是BUFF
/////// </summary>
////public bool IsBuff { get; protected set; }

/////// <summary>
/////// buff参数
/////// </summary>
////public float Arg { get; set; }

/////// <summary>
/////// 层数
/////// </summary>
////public int FoldCount { get; set; }

/////// <summary>
/////// 剩余周期
/////// </summary>
////public float RestDuration { get; protected set; }

/////// <summary>
/////// buffer是否生效
/////// </summary>
////bool isValid;

/////// <summary>
/////// 生命周期
/////// </summary>
////protected float duration;

/////// <summary>
/////// 拥有者
/////// </summary>
////protected IBattleUnit owner;

/////// <summary>
/////// 最大值
/////// </summary>
////protected int maxValue;

////public Buffer(int id, string uid, BufferTriggerType bufferType, float arg, float duration, int fold, int maxValue, IBattleUnit owner = null)
////{
////    Id = id;
////    Uid = uid;
////    BufferType = bufferType;
////    Arg = arg;
////    FoldCount = fold;
////    this.owner = owner;
////    this.duration = duration;
////    this.maxValue = maxValue;

////    OnValid();

////    if (duration != -1)
////    {
////        //tweenerDuration = DotweenManager.Ins.DOTweenDelay(duration, 1, () =>
////        //{
////        //    if (isValid)
////        //        OnInValid();

////        //    onBuffInValid?.Invoke(this);
////        //});
////    }
////}

/////// <summary>
/////// 重置周期
/////// </summary>
////public void Reset(float duration)
////{
////    this.duration = duration;
////    //tweenerDuration?.Kill();
////    if (duration != -1)
////    {
////        isValid = true;
////        //tweenerDuration = DotweenManager.Ins.DOTweenDelay(duration, 1, () =>
////        //{
////        //    if (isValid)
////        //        OnInValid();

////        //    onBuffInValid?.Invoke(this);
////        //});
////    }

////}


/////// <summary>
/////// buff生效（周期开始）
/////// </summary>
////protected virtual void OnValid()
////{
////    isValid = true;
////}

/////// <summary>
/////// buff失效（周期到了）
/////// </summary>
////protected virtual void OnInValid()
////{
////    isValid = false;
////}

/////// <summary>
/////// 值加成
/////// </summary>
/////// <param name="value"></param>
/////// <returns></returns>
////public abstract int Buff(int value);

/////// <summary>
/////// 值加成
/////// </summary>
/////// <param name="value"></param>
/////// <returns></returns>
////public abstract float Buff(float value);

////public virtual void Release()
////{
////    //tweenerDuration?.Kill();

////    if (isValid)
////        OnInValid();
////}