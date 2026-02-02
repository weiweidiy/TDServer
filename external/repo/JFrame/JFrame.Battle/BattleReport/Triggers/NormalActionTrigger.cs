using JFramework;

namespace JFramework
{
    /// <summary>
    /// 普通攻击时候触发，type = 5
    /// </summary>
    public class NormalActionTrigger : BaseBattleTrigger
    {
        public NormalActionTrigger(IPVPBattleManager battleManager, float[] arg, float delay = 0) : base(battleManager, arg, delay)
        {

        }

        public override void OnAttach(IAttachOwner owner)
        {
            base.OnAttach(owner);

            var o = owner as IBattleAction;

            o.onStartCast += Action_onStartCast;
        }

        private void Action_onStartCast(IBattleAction arg1, System.Collections.Generic.List<IBattleUnit> arg2, float arg3)
        {
            SetOn(true);
        }
    }
}


///// <summary>
///// 普通攻击时候触发，type = 5
///// </summary>
//public class NormalActionTrigger : BaseBattleTrigger
//{
//    public NormalActionTrigger(IPVPBattleManager battleManager, float[] arg, float delay = 0) : base(battleManager, arg, delay)
//    {

//    }

//    public override void OnAttach(IBattleAction action)
//    {
//        base.OnAttach(action);

//        action.onStartCast += Action_onStartCast;
//    }

//    private void Action_onStartCast(IBattleAction arg1, System.Collections.Generic.List<IBattleUnit> arg2, float arg3)
//    {
//        SetOn(true);
//    }
//}