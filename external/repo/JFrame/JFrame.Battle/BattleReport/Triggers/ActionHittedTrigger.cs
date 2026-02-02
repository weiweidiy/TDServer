

using System.Collections.Generic;
using System;

namespace JFramework
{

    /// <summary>
    /// type 14
    /// </summary>
    public class ActionHittedTrigger : ActionCastTrigger
    {
        //protected int targetAciontId;

        //protected List<IBattleAction> targetActions = new List<IBattleAction>();
        public ActionHittedTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
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

        //    if(targetAciontId == 0) //所有技能
        //    {
        //        var actions = o.Owner.GetActions();
        //        if (actions == null)
        //            throw new Exception("没有找到目标 action " + targetAciontId);

        //        foreach (var action in actions)
        //        {
        //            action.onHittedComplete += Action_onHittedComplete;
        //            targetActions.Add(action);
        //        }
        //    }
        //    else
        //    {
        //        var action = o.Owner.GetAction(targetAciontId);

        //        if (action == null)
        //            throw new Exception("没有找到目标 action " + targetAciontId);

        //        action.onHittedComplete += Action_onHittedComplete;
        //        targetActions.Add(action);
        //    }
            
        //}

        //public override void OnDetach()
        //{
        //    base.OnDetach();

        //    if (targetActions != null)
        //    {
        //        foreach(var action in targetActions)
        //        {
        //            action.onHittedComplete -= Action_onHittedComplete;
        //        }
        //    }
                
        //}

        protected virtual void Action_onHittedComplete(IBattleAction action, IBattleUnit caster, ExecuteInfo info, IBattleUnit target)
        {
            NotifyTriggerOn(this, new object[] { action , target  , info});
            SetOn(true);

        }

        protected override void AddListner(IBattleAction action)
        {
            action.onHittedComplete += Action_onHittedComplete;
        }

        protected override void RemoveListner(IBattleAction action)
        {
            action.onHittedComplete -= Action_onHittedComplete;
        }
    }
}