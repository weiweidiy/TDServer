using System;
using System.Drawing;

namespace JFramework
{

    /// <summary>
    /// 自己死亡触发
    /// </summary>
    public class DeathTrigger : BaseBattleTrigger
    {
        bool hited;

        int count;
        int tempCount;
        public DeathTrigger(IPVPBattleManager pvpBattleManager, float[] arg, float delay = 0) : base(pvpBattleManager, arg, delay)
        {
            if (arg.Length < 1)
                throw new Exception("DeathTrigger 参数不对");

            count = (int)arg[0];
        }

        public override BattleTriggerType TriggerType => BattleTriggerType.AfterDead;

        public override void Restart()
        {
            base.Restart();
            hited = false;
        }

        protected override void OnDelayCompleteEveryFrame(CombatFrame frame)
        {
            base.OnDelayCompleteEveryFrame(frame);

            var owner = Owner as IBattleAction;
            if (owner == null)
                throw new Exception("attach owner 转换失败 ");

            if (!owner.Owner.IsAlive() && !hited && tempCount < count)
            {
                SetOn(true);
                hited = true;
                tempCount++;
            }

        }

    }
}

///// <summary>
///// 本体死亡触发，只能1次  arg : 没用  type = 2
///// </summary>
//public class DeathTrigger : BaseBattleTrigger
//{
//    bool hited;

//    public DeathTrigger( IPVPBattleManager pvpBattleManager, float[] arg, float delay = 0):base(pvpBattleManager, arg, delay)
//    { }

//    public override BattleTriggerType TriggerType => BattleTriggerType.AfterDead;




//    protected override void OnDelayCompleteEveryFrame()
//    {
//        base.OnDelayCompleteEveryFrame();

//        if (!Owner.Owner.IsAlive() && !hited)
//        {
//            SetOn(true);
//            hited = true;
//        }

//    }

//}