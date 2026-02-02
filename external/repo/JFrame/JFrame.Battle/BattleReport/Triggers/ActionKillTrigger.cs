using static System.Collections.Specialized.BitVector32;

namespace JFramework
{
    /// <summary>
    /// type 12
    /// </summary>
    public class ActionKillTrigger : ActionHittedTrigger
    {

        public ActionKillTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay) {
        }


        protected override void Action_onHittedComplete(IBattleAction action, IBattleUnit caster,ExecuteInfo info, IBattleUnit target)
        {
            if(!target.IsAlive())
            {
                NotifyTriggerOn(this, new object[] { action, target, info });
                SetOn(true);
            }

        }

    }
}