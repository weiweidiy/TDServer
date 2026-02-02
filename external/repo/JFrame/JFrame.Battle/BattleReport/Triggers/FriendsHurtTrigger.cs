using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 非满血友军
    /// </summary>
    public class FriendsHurtTrigger : BaseBattleTrigger
    {
        public FriendsHurtTrigger(IPVPBattleManager pvpBattleManager, float[] arg, float delay = 0) : base(pvpBattleManager, arg, delay)
        {
            if (arg.Length < 1)
                throw new Exception("FriendsHurtTrigger 参数不对，需要1个参数");
        }

        protected override void OnDelayCompleteEveryFrame(CombatFrame frame)
        {
            base.OnDelayCompleteEveryFrame(frame);

            var targets = FindTargets();
            if (targets.Count > 0)
            {
                SetOn(true);
            }
            else
            {
                SetOn(false);
            }
        }

        /// <summary>
        /// 触发条件单位数量
        /// </summary>
        /// <returns></returns>
        int GetTriggerHurtCount()
        {
            return (int)args[0];
        }

        public List<IBattleUnit> FindTargets()
        {
            var result = new List<IBattleUnit>();
            var owner = Owner as IBattleAction;
            if (owner == null)
                throw new Exception("attach owner 转换失败 ");

            var units = battleManager.GetUnits(battleManager.GetFriendTeam(owner.Owner));

            //debug
            foreach (var unit in units)
            {
                if (unit.IsAlive() && !unit.IsHpFull() && result.Count < GetTriggerHurtCount())
                {
                    result.Add(unit);
                }
            }

            return result;
        }



    }
}

///// <summary>
///// 己方顺序寻找一个非满血的单位 arg:非满血个数 type=4
///// </summary>
//public class FriendsHurtTrigger : BaseBattleTrigger
//{
//    public FriendsHurtTrigger(IPVPBattleManager pvpBattleManager, float[] arg, float delay = 0) : base(pvpBattleManager, arg, delay)
//    {

//    }

//    protected override void OnDelayCompleteEveryFrame()
//    {
//        base.OnDelayCompleteEveryFrame();

//        var targets = FindTargets();
//        if(targets.Count > 0 )
//        {
//            SetOn(true);
//        }
//        else
//        {
//            SetOn(false);
//        }
//    }

//    /// <summary>
//    /// 触发条件单位数量
//    /// </summary>
//    /// <returns></returns>
//    int GetTriggerHurtCount()
//    {
//        return (int)args[0];
//    }

//    public List<IBattleUnit> FindTargets()
//    {
//        var result = new List<IBattleUnit>();

//        var units = battleManager.GetUnits(battleManager.GetFriendTeam(Owner.Owner));

//        //debug
//        foreach (var unit in units)
//        {
//            if (unit.IsAlive() && !unit.IsHpFull() && result.Count < GetTriggerHurtCount())
//            {
//                result.Add(unit);
//            }
//        }

//        return result;
//    }



//}
