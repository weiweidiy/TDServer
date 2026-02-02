

namespace JFramework
{
    /// <summary>
    /// 次数冷却：arg = 可触发次数 type = 2
    /// </summary>
    public class AmountTrigger : BaseBattleTrigger
    {
        int amount = 0;

        public override BattleTriggerType TriggerType => BattleTriggerType.All;

        public AmountTrigger(IPVPBattleManager pVPBattleManager, float[] amount, float delay = 0f) : base(pVPBattleManager, amount, delay) { }


        public override void Restart()
        {
            base.Restart();

            amount++;
        }

        int GetAmount()
        {
            return (int)args[0];
        }

        /// <summary>
        /// 延迟完成（每一帧调用1次）
        /// </summary>
        protected override void OnDelayCompleteEveryFrame(CombatFrame frame)
        {
            base.OnDelayCompleteEveryFrame(frame);

            //如果当前次数小于使用次数，则通知
            if(amount < GetAmount())
            {
                SetOn(true);
            }
            else
            {
                SetOn(false);
            }
        }



    }
}