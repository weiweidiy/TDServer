using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 主动动作，会排队释放
    /// </summary>
    public class ActiveAction : BaseAction
    {
        public ActiveAction(string UID, int id, ActionType type, float duration, IBattleTrigger conditionTrigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors, IBattleTrigger cdTrigger, OldActionSM sm) : base(UID, id, type, duration, conditionTrigger, finder, exutors, cdTrigger, sm)
        {
        }

        public override ActionMode Mode => ActionMode.Active;
    }
}