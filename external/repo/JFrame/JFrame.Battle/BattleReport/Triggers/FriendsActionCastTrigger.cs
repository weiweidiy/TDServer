using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework
{

    /// <summary>
    /// 友军释放普通或技能时 type = 11
    /// </summary>
    public class FriendsActionCastTrigger : BaseBattleTrigger
    {
        ActionType actionType;

        int team; //1 友军，2 敌军

        List<IBattleAction> targetActions = new List<IBattleAction>();
        public FriendsActionCastTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if (args.Length < 2)
                throw new Exception("ActionCastTrigger 需要2个参数");

            actionType = (ActionType)args[0];
            team = (int)args[1];
        }

        public override void OnAttach(IAttachOwner owner)
        {
            base.OnAttach(owner);

            var o = owner as IBattleAction;
            if (o == null)
                throw new Exception("attach owner 转换失败 ");

            

            var units = battleManager.GetUnits(  team == 1?  battleManager.GetFriendTeam(owner.Owner) : battleManager.GetOppoTeam(owner.Owner));

            foreach( var unit in units)
            {
                if (unit == owner.Owner)
                    continue;

                var targetActions = unit.GetActions(actionType);

                if (targetActions == null || targetActions.Length == 0)
                    throw new Exception("没有找到目标 action " + actionType);

                foreach (var targetAction in targetActions)
                {
                    targetAction.onStartCast += Action_onStartCast;

                    this.targetActions.Add(targetAction);
                }
            }

            
        }


        public override void OnDetach()
        {
            foreach(var targetAction in targetActions )
            {
                targetAction.onStartCast -= Action_onStartCast;
            }
        }

        private void Action_onStartCast(IBattleAction action, List<IBattleUnit> targets, float duration)
        {
            IBattleUnit target = null;
            if(targets != null && targets.Count > 0)
                target = targets[0];

            NotifyTriggerOn(this, new object[] { action, target, new ExecuteInfo() { } });
            SetOn(true);
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