using System;

namespace JFramework
{
    /// <summary>
    /// 动态HP触发器 type  19
    /// </summary>
    public class DynamicHpTrigger : BaseBattleTrigger
    {
        float baseValue;
        float xValue;
        float kValue;
        int curLevel;
        int preDamage;

        public DynamicHpTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if (args.Length < 3)
                throw new System.Exception("DynamicHpTrigger 参数数量不对");

            baseValue = args[0];
            xValue = args[1];
            kValue = args[2];
        }


        public override void OnAttach(IAttachOwner target)
        {
            base.OnAttach(target);
            curLevel = (int)battleManager.GetExtraData();

            NotifyTriggerOn(this, new object[] { Owner, Owner.Owner, new ExecuteInfo() { Value = curLevel } });
        }

        /// <summary>
        /// 延迟完成
        /// </summary>
        protected override void OnDelayCompleteEveryFrame(CombatFrame frame)
        {
            base.OnDelayCompleteEveryFrame(frame);

            ///战斗开始受到的所有伤害值总和
            var allDamage = GetAllDamage();
            var curHp = GetCurHp(curLevel);
            if (allDamage - preDamage >= curHp)
            {
                curLevel++;
                preDamage += (int)curHp;
                NotifyTriggerOn(this, new object[] { Owner, Owner.Owner, new ExecuteInfo() { Value = curLevel } });
            }
        }

        /// <summary>
        /// 获取当前等级生命值
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        double GetCurHp(int level)
        {
            return baseValue + xValue * Math.Pow(level, kValue);
        }

        /// <summary>
        /// 本次战斗生命损失值
        /// </summary>
        /// <returns></returns>
        int GetAllDamage()
        {
            return Owner.Owner.MaxHP - Owner.Owner.HP;
        }

        ///// <summary>
        ///// 获取
        ///// </summary>
        ///// <param name="level"></param>
        ///// <returns></returns>
        //double GetAllDamage(int level)
        //{
        //    double result = 0;

        //    if (level == 0)
        //    {
        //        return GetCurHp(level);
        //    }     
        //    else
        //    {
        //        result += GetAllDamage(level - 1);
        //        return result;
        //    }
                
        //}

    }
}

//public class NoneTrigger : BaseBattleTrigger
//{
//    public NoneTrigger(IPVPBattleManager pVPBattleManager, float[] duration, float delay = 0f) : base(pVPBattleManager, duration, delay) { }



//    /// <summary>
//    /// 延迟完成
//    /// </summary>
//    protected override void OnDelayCompleteEveryFrame()
//    {
//        base.OnDelayCompleteEveryFrame();

//        SetOn(true);
//    }
//}