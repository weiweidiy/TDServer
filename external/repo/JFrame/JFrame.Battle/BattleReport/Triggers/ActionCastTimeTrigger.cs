using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 15
    /// </summary>
    public class ActionCastTimeTrigger : ActionCastTrigger
    {
        float duration;
        bool casted;
        IBattleAction action;
        public ActionCastTimeTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0) : base(battleManager, args, delay)
        {
            if (args.Length < 2)
                throw new Exception("ActionCastTimeTrigger 需要至少2个参数");

            duration = args[0];
            List<float> ids = new List<float>();
            for(int i = 1; i < args.Length; i++)
            {
                ids.Add(args[i]);
            }
            
            targetAciontIds = ids.ToArray();
        }

        protected override void Action_onStartCast(IBattleAction action, List<IBattleUnit> targets, float duration)
        {
            casted = true;
            this.action = action;
        }

        protected override void OnDelayCompleteEveryFrame(CombatFrame frame)
        {
            base.OnDelayCompleteEveryFrame(frame);

            if (!casted)
                return;

            //更新cd
            if (delta >= duration && GetEnable())
            {
                if (!IsOn())
                {
                    NotifyTriggerOn(this, new object[] { action , null,null });
                }
                delta = 0f;
                SetOn(true);
            }
            else
            {
                SetOn(false);
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