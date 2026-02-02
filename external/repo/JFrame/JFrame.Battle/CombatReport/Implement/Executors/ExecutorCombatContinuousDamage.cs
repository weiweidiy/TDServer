namespace JFramework
{
    /// <summary>
    /// 连续傷害执行器，可以打多次 type = 2 參數0：持續時間 1：傷害加成  2: 类型   3：間隔  4：次數
    /// </summary>
    public class ExecutorCombatContinuousDamage : ExecutorCombatDamage
    {
        CombatUnit primaryTarget = null;

        float delta = 0f;

        public override int GetValidArgsCount()
        {
            return 5;
        }
        protected float GetIntervalArg()
        {
            return GetCurArg(3);
        }

        protected override float GetCountArg()
        {
            return GetCurArg(4);
        }


        public ExecutorCombatContinuousDamage(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }

        protected override void OnUpdate(CombatFrame frame)
        {
           // base.Update(frame);

            if (!isExecuting)
                return;

            if (count >= GetCountArg())
            {
                isExecuting = false;
                return;
            }
                
            delta += frame.DeltaTime;
            if (delta >= GetIntervalArg()) //每次间隔都要重新找一次目标（executorfinders)
            {
                delta = 0f;
                
                var pTarget = Hit();

                if (primaryTarget == null)
                    primaryTarget = pTarget;
                else
                {
                    if(pTarget != null && primaryTarget != pTarget)
                    {
                        //切换目标了
                        primaryTarget = pTarget;
                        extraData.Target = primaryTarget;
                        extraData.ShootCount = (int)GetCountArg() - count;
                        extraData.Caster.OnShootTargetChanged(extraData);
                    }
                }
            }
        }



        public override void Reset() //进入执行状态会重置
        {
            base.Reset();
            delta = 0f;
        }

        public override void OnExitState()
        {
            base.OnExitState();
        }
    }
}