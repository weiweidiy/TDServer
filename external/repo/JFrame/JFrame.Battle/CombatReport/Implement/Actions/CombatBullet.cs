using System;
using System.Diagnostics;

namespace JFramework
{
    public class CombatBullet : IUnique, IUpdateable, ICombatUpdatable
    {
        public string Uid { get; private set; }

        CombatBaseExecutor executor;

        float delay = 0f;

        float delta = 0f;

        CombatUnit target;

        CombatExtraData data;

        int i = 0;

        public CombatBullet(CombatBaseExecutor executor, CombatUnit target, CombatExtraData data, float delay)
        {
            this.executor = executor;
            Uid = Guid.NewGuid().ToString();
            this.delay = delay;
            this.target = target;
            this.data = data;
        }
        public void Update(CombatFrame frame)
        {
            delta += frame.DeltaTime;

            //if (executor.Owner.Id == 121)
            //{
            //    i++;
            //    executor.context.Logger?.Log("更新 " + i + " / " + frame.CurFrame +  " " + GetHashCode() );
            //}
            if (delta >= delay)
            {
                //if (executor.Owner.Id == 121)
                //    executor.context.Logger?.Log("delta " + delta + " frame.DeltaTime" + frame.DeltaTime + " delay " + delay + "  OnDamage frame: " + executor.context.CombatManager.Frame.CurFrame);
                target.OnDamage(data);

                //生效
                //if (executor.context.Logger != null)
                //    executor.context.Logger.Log("bullet update");

                executor.Owner.RemvoeBullet(this);

                delta = 0f;
            }

        }

        public void Update(IUpdateable value)
        {

        }


    }

}