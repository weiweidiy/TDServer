//using System.Collections.Generic;

//namespace JFrame
//{
//    /// <summary>
//    /// 缴械 参数 1： 持续时间
//    /// </summary>
//    public class DebufferDisarm : DurationBuffer
//    {
//        public DebufferDisarm(IBattleUnit caster, bool isBuff, string UID, int id, int foldCount, float[] args, IBattleTrigger trigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors) : base(caster, isBuff, UID, id, foldCount, args, trigger, finder, exutors)
//        {
//        }

//        public override void OnAttach(IBattleUnit unit)
//        {
//            base.OnAttach(unit);

//            unit.OnStunning(ActionType.Normal, GetDuration());
//        }

//        public override void OnDettach()
//        {
//            base.OnDettach();

//            Owner.OnResumeFromStunning(ActionType.Normal);
//        }

//    }
//}




