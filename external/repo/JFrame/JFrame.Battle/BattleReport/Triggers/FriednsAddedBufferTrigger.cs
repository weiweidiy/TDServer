using System;
using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// type 17 友军被添加了某个BUFF时触发
    /// </summary>
    public class FriednsAddedBufferTrigger : BaseBattleTrigger
    {
        int buffId;

        List<IBattleUnit> unitList = new List<IBattleUnit>();
        public FriednsAddedBufferTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if (args.Length != 1)
                throw new Exception("FriednsAddedBufferTrigger 需要1个参数");

            buffId = (int)args[0];
        }

        public override void OnAttach(IAttachOwner target)
        {
            base.OnAttach(target);

            //var o = owner as IBattleAction;
            //if (o == null)
            //    throw new Exception("attach owner 转换失败 ");

            var units = battleManager.GetUnits(battleManager.GetFriendTeam(Owner.Owner));

            foreach (var unit in units)
            {
                if (unit == Owner.Owner)
                    continue;

                unit.onBufferAdded += Owner_onBufferAdded;
                unitList.Add(unit);
            }
        }

        public override void OnDetach()
        {
            base.OnDetach();
            
            foreach(var unit in unitList)
            {
                unit.onBufferAdded -= Owner_onBufferAdded;
            }
        }

        private void Owner_onBufferAdded(IBattleUnit unit, IBuffer buffer)
        {
            if(buffer.Id == buffId)
            {
                NotifyTriggerOn(this, new object[] { Owner, unit, null });
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