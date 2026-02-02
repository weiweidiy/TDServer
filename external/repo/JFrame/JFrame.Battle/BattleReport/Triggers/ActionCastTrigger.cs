using System;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace JFramework
{

    /// <summary>
    /// 自己释放技能时， type = 7
    /// </summary>
    public class ActionCastTrigger : BaseBattleTrigger 
    {
        protected float[] targetAciontIds;

        protected List<IBattleAction> targetActions = new List<IBattleAction>();
        public ActionCastTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if (args.Length < 1)
                throw new Exception("ActionCastTrigger 需要1个参数");

            targetAciontIds = args;
        }

        public override void OnAttach(IAttachOwner owner)
        {
            base.OnAttach(owner);

            //var o = owner as IBattleAction;
            //if (o == null)
            //    throw new Exception("attach owner 转换失败 ");

            bool found = false;
            foreach (var actionId in targetAciontIds)
            {
                var targetAction = owner.Owner.GetAction((int)actionId);

                if (targetAction == null)
                {
                    continue;
                    //throw new Exception("没有找到目标 action " + targetAciontIds);
                }

                AddListner(targetAction);
                targetActions.Add(targetAction);
                found = true;
            }

            if (!found)
                throw new Exception(owner.Id + " 没有找到目标 action ");
        }

        protected virtual void AddListner(IBattleAction action)
        {
            action.onStartCast += Action_onStartCast;
        }

        protected virtual void RemoveListner(IBattleAction action)
        {
            action.onStartCast -= Action_onStartCast;
        }

        public override void OnDetach()
        {
            base.OnDetach();

            foreach (var action in targetActions)
                RemoveListner(action);
        }

        protected virtual void Action_onStartCast(IBattleAction action, List<IBattleUnit> targets, float duration)
        {
            if (targets == null || targets.Count == 0)
                return;

            NotifyTriggerOn(this,new object[] { action, targets[0], null });
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