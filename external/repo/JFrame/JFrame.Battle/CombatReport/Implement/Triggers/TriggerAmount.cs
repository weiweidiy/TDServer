using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 直接触发 type = 2 参数0： 次数 
    /// </summary>
    public class TriggerAmount : CombatBaseTrigger
    {
        int count = 0;
        public TriggerAmount(List<CombatBaseFinder> finders) : base(finders)
        {
        }

        public override int GetValidArgsCount()
        {
            return 1;
        }

        /// <summary>
        /// 退出当前状态
        /// </summary>
        public override void OnExitState()
        {
            base.OnExitState();

            count++;
        }

        protected int GetAmountArg()
        {
            return (int)GetCurArg(0);
        }

        protected override void OnUpdate(CombatFrame frame)
        {
            base.OnUpdate(frame);

            if (count < GetAmountArg())
            {
                if (finders != null && finders.Count > 0)
                {
                    foreach(var finder in finders)
                    {
                        var targets = finder.FindTargets(ExtraData); //获取目标
                        targets = Filter(targets);
                        if (targets != null && targets.Count > 0)
                        {
                            _extraData.Targets = targets;
                            SetOn(true);
                        }
                    }
                }
                else
                    SetOn(true);
            }

        }
    }
}