

//using System.Collections.Generic;

//namespace JFrame
//{

//    /// <summary>// 还没生效
//    /// 攻击力提升 参数 1：持续时间 , 参数2 ：攻击力提升数值
//    /// </summary>
//    public class BufferAttackUp : DurationBuffer
//    {
//        int value;

//        public BufferAttackUp(IBattleUnit caster, bool isBuff, int buffType,string UID, int id, int foldCount, float[] args, IBattleTrigger trigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors) : base(caster, isBuff, buffType, UID, id, foldCount, args, trigger, finder, exutors)
//        {
//            if (args.Length < 2)
//                throw new System.Exception("BufferAttackUp 参数不能少于2个");
//        }

//        public virtual float GetValue()
//        {
//            return Args[1] * FoldCount;
//        }

//        public override void OnAttach(IBattleUnit unit)
//        {
//            base.OnAttach(unit);
//            value = unit.AtkUpgrade((int)GetValue());
//        }

//        public override void OnDettach()
//        {
//            base.OnDettach();

//            //如果直接减，会有问题
//            Owner.AtkReduce(value);
//        }

//    }
//}


