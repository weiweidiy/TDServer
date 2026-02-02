using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 16
    /// </summary>
    public class ActionCastHittedTrigger : ActionCastTrigger
    {
        bool casted;
        //IBattleAction action;
        public ActionCastHittedTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
        }

        public override void OnAttach(IAttachOwner owner)
        {
            base.OnAttach(owner);

            owner.Owner.onDamaged += Owner_onDamaged;
        }


        public override void OnDetach()
        {
            base.OnDetach();

            Owner.Owner.onDamaged -= Owner_onDamaged;
        }


        private void Owner_onDamaged(IBattleUnit caster, IBattleAction action, IBattleUnit target, ExecuteInfo info)
        {
            if (!casted)
                return;

            NotifyTriggerOn(this, new object[] { action, target, info });
            SetOn(true);
        }


        protected override void Action_onStartCast(IBattleAction action, List<IBattleUnit> targets, float duration)
        {
            casted = true;
            //this.action = action;
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