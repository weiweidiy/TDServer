namespace JFramework
{
    /// <summary>
    /// 待机状态（冷却已经完成）
    /// </summary>
    public class ActionStandby : ActionState
    {
        public override string Name => nameof(ActionStandby);

        public override void OnEnter(BaseAction context)
        {
            base.OnEnter(context); //要先调用

            if(NeedUpdate())
                context.ConditionTrigger.Restart();
        }


        public override void OnExit()
        {
            //Debug.Log("BatlleReady OnExit");
            base.OnExit();
        }

        public override void Update(CombatFrame frame)
        {
            base.Update(frame);

            if (NeedUpdate())
                context.ConditionTrigger.Update(frame);

            if (NeedUpdate() && context.CanCast())
            {
                //通知动作管理器，能释放
                context.NotifyCanCast();
            }
        }

        bool NeedUpdate()
        {
            return context.ConditionTrigger != null && (context.ConditionTrigger.TriggerType == BattleTriggerType.Normal || context.ConditionTrigger.TriggerType == BattleTriggerType.All);
        }
    }
}