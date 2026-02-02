using System;
using System.Diagnostics;

namespace JFramework
{
    /// <summary>
    /// 时间触发器：时间到了触发1次 arg = CD周期 type = 1
    /// </summary>
    public class CDTimeTrigger : BaseBattleTrigger
    {

        public CDTimeTrigger(IPVPBattleManager pVPBattleManager, float[] args, float delay = 0f) : base( pVPBattleManager, args, delay) { }


        /// <summary>
        /// 获取周期
        /// </summary>
        /// <returns></returns>
        public float GetDuration()
        {
            return args[0];
        }

        /// <summary>
        /// 延迟完成
        /// </summary>
        protected override void OnDelayCompleteEveryFrame(CombatFrame frame)
        {
            base.OnDelayCompleteEveryFrame(frame);

            //更新cd
            if (delta >= GetDuration() && GetEnable())
            {
                if (!IsOn())
                {
                    
                    NotifyTriggerOn(this, new object[] { true });
                    //delta = 0f;
                }
                SetOn(true);
            }
            else
            {
                SetOn(false);
            }

        }
    }
}