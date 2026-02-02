using System;

namespace JFramework
{
    ///// <summary>
    ///// 触发器工厂
    ///// </summary>
    //public class TriggerFactory
    //{
    //    public IBattleTrigger Create(PVPBattleManager pvpBattleManager, int triggerType, float arg, float delay = 0)
    //    {
    //        switch (triggerType)
    //        {
    //            case 1: //无
    //                return new NoneTrigger(pvpBattleManager, new float[1] { arg }, delay);
    //            case 2: //自身死亡触发
    //                return new DeathTrigger(pvpBattleManager, new float[1] { arg }, delay);
    //            case 3: //战斗开始触发
    //                return new BattleStartTrigger(pvpBattleManager, new float[1] { arg }, delay);
    //            case 4: //己方有非满血成员
    //                return new FriendsHurtTrigger(pvpBattleManager, new float[1] { arg }, delay);
    //            case 5: //指定action释放
    //                return new ActionCastTrigger(pvpBattleManager, new float[1] { arg }, delay);
    //            default:
    //                throw new Exception(triggerType + " 技能未实现的 ConditionTrigger type " + triggerType);
    //        }
    //    }
    //}

    public class TriggerFactory
    {
        public IBattleTrigger Create(PVPBattleManager pvpBattleManager, int triggerType, float[] args, float delay = 0)
        {
            switch (triggerType)
            {
                case 1: //无
                    return new NoneTrigger(pvpBattleManager, args, delay);
                case 2: //自身死亡触发
                    return new DeathTrigger(pvpBattleManager, args, delay);
                case 3: //战斗开始触发
                    return new BattleStartTrigger(pvpBattleManager, args, delay);
                case 4: //己方有非满血成员
                    return new FriendsHurtTrigger(pvpBattleManager, args, delay);
                case 5:
                    return new AttachTrigger(pvpBattleManager, args, delay);
                case 6:
                    return new HurtTrigger(pvpBattleManager, args, delay);
                case 7: //指定action释放
                    return new ActionCastTrigger(pvpBattleManager, args, delay);
                case 8:
                    return new CDTimeTrigger(pvpBattleManager, args, delay);
                case 9:
                    return new AmountTrigger(pvpBattleManager, args, delay);
                case 11: //友军释放动作，不包括自己
                    return new FriendsActionCastTrigger(pvpBattleManager, args, delay);
                case 12:
                    return new ActionKillTrigger(pvpBattleManager, args, delay);
                case 13:
                    return new ActionHittingTrigger(pvpBattleManager, args, delay);
                case 14:
                    return new ActionHittedTrigger(pvpBattleManager, args, delay);
                case 15:
                    return new ActionCastTimeTrigger(pvpBattleManager, args, delay);
                case 16:
                    return new ActionCastHittedTrigger(pvpBattleManager, args, delay);
                case 17:
                    return new FriednsAddedBufferTrigger(pvpBattleManager, args, delay);
                case 18:
                    return new SelfAddingBufferTrigger(pvpBattleManager, args, delay);
                case 19:
                    return new DynamicHpTrigger(pvpBattleManager, args, delay);
                case 20:
                    return new OtherDeathTrigger(pvpBattleManager, args, delay);
                case 21:
                    return new FriendsRemoveBufferTrigger(pvpBattleManager, args, delay);
                default:
                    throw new Exception(triggerType + " 技能未实现的 ConditionTrigger type " + triggerType);
            }
        }
    }
}