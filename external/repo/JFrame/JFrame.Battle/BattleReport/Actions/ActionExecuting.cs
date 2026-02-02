using System.Reflection;
using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 释放中
    /// </summary>
    public class ActionExecuting : ActionState
    {
        public override string Name => nameof(ActionExecuting);

        float deltaTime = 0f;

        float duration = 1f; //临时，从action中获取

        public void OnEnter(BaseAction context, List<IBattleUnit> targets)
        {
            OnEnter(context);

            //获取目标 to do:不用再找，通过standby
            if(targets == null)
                 targets = context.FindTargets(null);

            if ((targets == null || targets.Count == 0) && context.Mode == ActionMode.Active)
            {
                return;
                //throw new Exception("释放时，没有找到目标 " + context.Id);
            }


            duration = context.GetCastDuration();

            //重置动作
            foreach (var e in context.exeutors)
            {
                e.Reset();
            }

            //准备执行效果
            context.ReadyToExecute(context.Owner, context, targets);
            //通知开始触发
            context.NotifyStartCast(targets, duration);

        }


        public override void OnExit()
        {
            //Debug.Log("BatlleReady OnExit");
            base.OnExit();


        }

        public override void Update(CombatFrame frame)
        {
            base.Update(frame);

            foreach(var e in context.exeutors)
            {
                e.Update(frame);
            }

            deltaTime += frame.DeltaTime;

            if (deltaTime >= duration)
            {
                deltaTime = 0f;
                context.EnterCD();
            }
        }
    }
}