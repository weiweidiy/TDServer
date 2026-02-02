using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 21
    /// </summary>
    public class FriendsRemoveBufferTrigger : BaseBattleTrigger
    {
        int buffId;
        List<IBattleUnit> unitList = new List<IBattleUnit>();
        public FriendsRemoveBufferTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if (args.Length != 1)
                throw new Exception("FriendsRemoveBufferTrigger 需要1个参数");

            buffId = (int)args[0];
        }

        public override void OnAttach(IAttachOwner target)
        {
            base.OnAttach(target);

            var units = battleManager.GetUnits(battleManager.GetFriendTeam(Owner.Owner));

            foreach (var unit in units)
            {
                if (unit == Owner.Owner)
                    continue;

                unit.onBufferRemoved += Unit_onBufferRemoved;
                unitList.Add(unit);

            }
        }
        public override void OnDetach()
        {
            base.OnDetach();

            foreach (var unit in unitList)
            {
                unit.onBufferRemoved -= Unit_onBufferRemoved;
            }
        }

        private void Unit_onBufferRemoved(IBattleUnit owner, IBuffer buffer)
        {
            if (buffer.Id == buffId)
            {
                NotifyTriggerOn(this, new object[] { Owner, owner, null });
                SetOn(true);
            }
        }
    }
}

///// <summary>
///// 动作释放时触发 参数1：目标actionID type = 5
///// </summary>
//public class ActionCastTrigger : BaseBattleTrigger
//{
//    int targetAciontId;
//    public ActionCastTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
//    {
//        if (args.Length < 1)
//            throw new Exception("ActionCastTrigger 需要1个参数");

//        targetAciontId = (int)args[0];
//    }

//    public override void OnAttach(IBattleAction action)
//    {
//        base.OnAttach(action);

//        var targetAction = action.Owner.GetAction(targetAciontId);

//        if (targetAction == null)
//            throw new Exception("没有找到目标 action " + targetAciontId);

//        targetAction.onStartCast += Action_onStartCast;
//    }

//    private void Action_onStartCast(IBattleAction action, List<IBattleUnit> targets, float duration)
//    {
//        SetOn(true);
//    }

//}