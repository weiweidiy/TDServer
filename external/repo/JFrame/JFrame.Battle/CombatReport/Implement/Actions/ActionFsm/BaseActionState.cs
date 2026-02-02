using System;

namespace JFramework
{
    /// <summary>
    /// 技能状态基类
    /// </summary>
    public abstract class BaseActionState : BaseStateSync<CombatAction>
    {
        public override void OnExit()
        {
            base.OnExit();

            Console.WriteLine(GetType().Name + " OnExit");
        }

        protected override void OnEnter()
        {
            base.OnEnter();

            Console.WriteLine(GetType().Name + " OnEnter");
        }

        public virtual void Update(CombatFrame frame)
        {
            
        }
    }
}
