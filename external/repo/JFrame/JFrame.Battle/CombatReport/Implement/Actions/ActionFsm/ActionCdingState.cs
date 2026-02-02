namespace JFramework
{
    /// <summary>
    /// 冷却状态
    /// </summary>
    public class ActionCdingState : BaseActionState
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            context.ResetCdTriggers();
            context.NotifyStartCD();
            context.EnterCdTriggers(); //進入狀態
        }

        public override void OnExit()
        {
            base.OnExit();

            context.ExitCdTriggers(); //推出狀態
        }

        public override void Update(CombatFrame frame)
        {
            base.Update(frame);

            context.UpdateCdTriggers(frame);
            context.UpdateExecutors(frame); //执行器会继续执行

            if (context.IsCdTriggerOn())
            {
                context.SwitchToTrigging();
            }
        }


    }


    public class ActionReadyState : BaseActionState
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            context.ResetReadyCdTrigger();
            context.NotifyStartCD();
            context.EnterReadyCdTriggers(); //進入狀態
        }

        public override void OnExit()
        {
            base.OnExit();

            context.ExitReadyCdTriggers(); //推出狀態
        }

        public override void Update(CombatFrame frame)
        {
            base.Update(frame);

            context.UpdateReadyCdTrigger(frame);
         
            if (context.IsReadyCdTriggerOn())
            {
                context.SwitchToTrigging();
            }
        }
    }
}
