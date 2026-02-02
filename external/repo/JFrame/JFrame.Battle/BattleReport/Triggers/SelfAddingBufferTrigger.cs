using System;

namespace JFramework
{
    /// <summary>
    /// 自己添加buff前触发
    /// </summary>
    public class SelfAddingBufferTrigger : BaseBattleTrigger
    {
        int buffType;
        public SelfAddingBufferTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if (args.Length != 1)
                throw new Exception("FriednsAddedBufferTrigger 需要1个参数");

            buffType = (int)args[0];

        }

        public override void OnAttach(IAttachOwner target)
        {
            base.OnAttach(target);

            target.Owner.onBufferAdding += Owner_onBufferAdding;
        }


        public override void OnDetach()
        {
            base.OnDetach();

            Owner.Owner.onBufferAdding -= Owner_onBufferAdding;
        }

        private void Owner_onBufferAdding(IBattleUnit target, int buffId, ExecuteInfo info)
        {
            NotifyTriggerOn(this, new object[] {Owner, target, info });
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