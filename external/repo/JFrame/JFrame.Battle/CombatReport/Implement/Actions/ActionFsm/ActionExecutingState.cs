using System;
using System.Diagnostics;

namespace JFramework
{
    /// <summary>
    /// 执行中状态
    /// </summary>
    public class ActionExecutingState : BaseActionState
    {
        //public event Action onExecutingComplete;
        float executingDuration;


        float deltaTime = 0f;

        protected override void OnEnter()
        {
            base.OnEnter();
            context.ResetExecutors();
            context.ResetDelayTrigger();
            context.NotifyStartExecuting();
            executingDuration = context.GetExecutingDuration();

            var hasTarget = context.ExtraData.Target != null ? true : context.ExtraData.Targets.Count > 0;

            if (hasTarget)
            {
                var bulletSpeed = context.BulletSpeed;
                if (bulletSpeed > 0f)
                {
                    //設置delaytrigger新參數
                    var startPos = context.ExtraData.Caster.GetPosition();
                    var target = context.ExtraData.Target != null ? context.ExtraData.Target : context.ExtraData.Targets[0];
                    var endPos = target.GetPosition();
                    var p = endPos - startPos;
                    var distance = p.Magnitude();
                    var duration = distance / bulletSpeed;

                    var executor = context.GetExecutor(0) as ExecutorCombatSingleThreadDamage;
                    if (executor == null)
                        context.SetDelayTriggerOriginArgs(new float[] { duration });
                    else
                    {
                        //修改子彈的delta
                        //if (context.Id == 121)
                        //    executor.context.Logger?.Log("duration =" + duration + " frame: " + executor.context.CombatManager.Frame.CurFrame);
                        executor.SetDelay(duration);
                    }
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void Update(CombatFrame frame)
        {
            base.Update(frame);

            context.UpdateDelayTrigger(frame);
            if (context.IsDelayTriggerOn())
            {
                context.DoExecutors();
                context.UpdateExecutors(frame);

                deltaTime += frame.DeltaTime;

                if (deltaTime >= executingDuration)
                {
                    deltaTime = 0f;
                    var cdDuration = context.SwitchToCd();
                }
            }
        }


    }
}
