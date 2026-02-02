

using System.Collections.Generic;
using System;

namespace JFramework
{
    /// <summary>
    /// type 13
    /// </summary>
    public class ActionHittingTrigger : ActionCastTrigger
    {
        //protected int targetAciontId;

        //protected List<IBattleAction> targetActions = new List<IBattleAction>();
        public ActionHittingTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if (args.Length < 1)
                throw new Exception("KillTrigger 需要至少1个参数");

            //targetAciontId = (int)args[0];
        }

        //public override void OnAttach(IAttachOwner owner)
        //{
        //    base.OnAttach(owner);

        //    var o = owner as IBattleAction;
        //    if (o == null)
        //        throw new Exception("attach owner 转换失败 ");

        //    if (targetAciontId == 0) //所有技能
        //    {
        //        var actions = o.Owner.GetActions();
        //        if (actions == null)
        //            throw new Exception("没有找到目标 action " + targetAciontId);

        //        foreach (var action in actions)
        //        {
        //            action.onHittingTarget += Action_onHittingComplete;
        //            targetActions.Add(action);
        //        }
        //    }
        //    else
        //    {
        //        var action = o.Owner.GetAction(targetAciontId);

        //        if (action == null)
        //            throw new Exception("没有找到目标 action " + targetAciontId);

        //        action.onHittingTarget += Action_onHittingComplete;
        //        targetActions.Add(action);
        //    }

        //}

        //public override void OnDetach()
        //{
        //    base.OnDetach();

        //    if (targetActions != null)
        //    {
        //        foreach (var action in targetActions)
        //        {
        //            action.onHittingTarget -= Action_onHittingComplete;
        //        }
        //    }

        //}

        protected virtual void Action_onHittingComplete(IBattleAction action, IBattleUnit target, ExecuteInfo info)
        {
            NotifyTriggerOn(this, new object[] { action , target, info});
            SetOn(true);

        }

        protected override void AddListner(IBattleAction action)
        {
            action.onHittingTarget += Action_onHittingComplete;
        }

        protected override void RemoveListner(IBattleAction action)
        {
            action.onHittingTarget -= Action_onHittingComplete;
        }
    }
}