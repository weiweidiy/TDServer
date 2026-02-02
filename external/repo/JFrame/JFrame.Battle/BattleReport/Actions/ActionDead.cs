namespace JFramework
{
    public class ActionDead : ActionState
    {
        public override string Name => nameof(ActionDead);

        public override void OnEnter(BaseAction context)
        {
            base.OnEnter(context); //要先调用

            foreach (var e in context.exeutors)
            {
                e.Reset();
            }

            if (NeedUpdate())
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
            return context.ConditionTrigger != null && (context.ConditionTrigger.TriggerType == BattleTriggerType.AfterDead || context.ConditionTrigger.TriggerType == BattleTriggerType.All);
        }
    }
}