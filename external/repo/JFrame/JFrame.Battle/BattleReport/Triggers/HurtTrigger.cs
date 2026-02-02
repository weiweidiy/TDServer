using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 6  立即触发型，自己受到伤害 0: 触发概率， 1： 0自己，1队友
    /// </summary>
    public class HurtTrigger : BaseBattleTrigger
    {
        float hitRate;
        int team = -1;
        List<IBattleUnit> targets = new List<IBattleUnit>();

        public HurtTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if(args.Length < 2)
            {
                throw new System.Exception("HurtTrigger 需要2个参数");
            }

            hitRate = args[0];
            team = (int)args[1];
        }


        public override void OnAttach(IAttachOwner target)
        {
            base.OnAttach(target);

            if(team == 0)
            {
                Owner.Owner.onDamaging += Owner_onDamaging;
                targets.Add(Owner.Owner);
            }
            else
            {
                var friends = FindTargets();
                foreach (var f in friends)
                {
                    f.onDamaging += Owner_onDamaging;
                    targets.Add(f);
                }
            }

        }

        public override void OnDetach()
        {
            base.OnDetach();

            foreach(var t in targets)
            {
                t.onDamaging -= Owner_onDamaging;
            }
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
                if (unit.IsAlive() && !unit.Equals(Owner.Owner))
                {
                    result.Add(unit);
                }
            }

            return result;
        }


        private void Owner_onDamaging(IBattleUnit caster, IBattleAction action, IBattleUnit target, ExecuteInfo Info)
        {
            var r = new Random().NextDouble();
            if (r < hitRate)
            {
                NotifyTriggerOn(this, new object[] { action, target, Info });
            }
            
        }
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