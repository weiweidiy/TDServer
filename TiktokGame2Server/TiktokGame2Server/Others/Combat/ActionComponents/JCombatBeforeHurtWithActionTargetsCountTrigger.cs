using Game.Share;
using JFramework.Game;
using TiktokGame2Server.Others;

namespace TiktokGame2Server.Others
{
    /// <summary>
    /// 命中前触发器，带兵种类型过滤
    /// </summary>
    public class JCombatBeforeHurtWithActionTargetsCountTrigger : JCombatBeforeHurtTrigger
    {
        public JCombatBeforeHurtWithActionTargetsCountTrigger(float[] args, IJCombatTargetsFinder finder) : base(args, finder)
        {
        }

        protected override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 1;
        }

        int GetActionTargetsCount()
        {
            return (int)GetArg(0);
        }

        protected override void OnBeforeHurt(IJCombatTargetable targetable, IJCombatDamageData data, IJCombatCasterUnit caster, IJCombatExecutorExecuteArgs casterExecuteArgs)
        {
            var trgetUnit = targetable as IJCombatCasterTargetableUnit;
            if (trgetUnit == null)
            {
                throw new System.Exception("JCombatBeforeHurtWithSoldierTypeTrigger can only be used with IJCombatCasterTargetableUnit targets.");
            }

            var targets = casterExecuteArgs.TargetUnits;
            if (targets == null || targets.Count == 0)
            {
                return;
            }
            if (targets.Count >= GetActionTargetsCount())
            {
                executeArgs.DamageData = data;
                executeArgs.TargetUnits = new List<IJCombatCasterTargetableUnit> { trgetUnit };
                TriggerOn(executeArgs);
            }
        }
    }

}
