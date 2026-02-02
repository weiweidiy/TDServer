namespace JFramework
{
    /// <summary>
    /// 加血执行器type = 6：参数：0 执行周期 , 1 : 加成 2: 间隔  3： 次数
    /// </summary>
    public class ExecutorCombatContinuousHeal : ExecutorCombatHeal
    {
        float delta = 0f;

        public ExecutorCombatContinuousHeal(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }

        public override int GetValidArgsCount()
        {
            return 4;
        }
        protected float GetIntervalArg()
        {
            return GetCurArg(2);
        }

        protected override float GetCountArg()
        {
            return GetCurArg(3);
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
            if (delta >= GetIntervalArg())
            {
                delta = 0f;
                Hit();
            }
        }

        public override void Reset()
        {
            base.Reset();
            delta = 0f;
        }

    }
}