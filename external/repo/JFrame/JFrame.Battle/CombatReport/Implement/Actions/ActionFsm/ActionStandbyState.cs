namespace JFramework
{
    /// <summary>
    /// 等待触发状态
    /// </summary>
    public class ActionStandbyState : BaseActionState
    {
        protected override void OnEnter()
        {
            base.OnEnter();

            context.ResetConiditionTriggers();
            context.EnterConditionTriggers();
        }

        public override void OnExit()
        {
            base.OnExit();

            context.ExitConditionTriggers();
        }

        public override void Update(CombatFrame frame)
        {
            base.Update(frame);
            
            context.UpdateConditionTriggers(frame);

            if(context.IsConditionTriggerOn())
            {
                context.NotifyTriggerOn(); //允许每一帧都通知
            }
        }


    }
}
