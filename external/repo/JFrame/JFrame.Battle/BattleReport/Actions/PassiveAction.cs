using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 被动技能
    /// </summary>
    public class PassiveAction : BaseAction
    {
        public PassiveAction(string UID, int id, ActionType type, float duration, IBattleTrigger conditionTrigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors, IBattleTrigger cdTrigger, OldActionSM sm) : base(UID, id, type, duration, conditionTrigger, finder, exutors, cdTrigger, sm)
        {
        }

        public override ActionMode Mode => ActionMode.Passive;
    }
}