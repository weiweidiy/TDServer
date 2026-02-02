using System;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace JFramework
{
    /// <summary>
    /// type 20:非自己死亡时触发
    /// </summary>
    public class OtherDeathTrigger : BaseBattleTrigger
    {
        int team = -1;
        float hitRate = 0f;
        List<IBattleUnit> targets = new List<IBattleUnit>();

        public OtherDeathTrigger(IPVPBattleManager pvpBattleManager, float[] arg, float delay = 0) : base(pvpBattleManager, arg, delay)
        {
            if (arg.Length < 2)
                throw new Exception("OtherDeathTrigger 参数不对");

            team = (int)arg[0];
            hitRate = arg[1];
        }

        public override void OnAttach(IAttachOwner target)
        {
            base.OnAttach(target);

            var targets = FindTargets(team);
            foreach (var f in targets)
            {
                f.onDead += F_onDead;
                this.targets.Add(f);
            }

        }

        private void F_onDead(IBattleUnit cast, IBattleAction action, IBattleUnit target)
        {
            var r = new Random().NextDouble();
            if (r < hitRate)
            {
                NotifyTriggerOn(this, new object[] { action, target, null });
                SetOn(true);
            }
        }

        public override void OnDetach()
        {
            base.OnDetach();

            foreach (var t in targets)
            {
                t.onDead -= F_onDead;
            }
        }

        public List<IBattleUnit> FindTargets(int team)
        {
            var result = new List<IBattleUnit>();
            var owner = Owner as IBattleAction;
            if (owner == null)
                throw new Exception("attach owner 转换失败 ");


            List<IBattleUnit> units = null;
            
            if(team == 0)
                units= battleManager.GetUnits(battleManager.GetFriendTeam(owner.Owner));
            else
                units = battleManager.GetUnits(battleManager.GetOppoTeam(owner.Owner));

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
    }
}

