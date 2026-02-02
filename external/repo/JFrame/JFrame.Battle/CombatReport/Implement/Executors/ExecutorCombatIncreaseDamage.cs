namespace JFramework
{
    /// <summary>
    /// type 8 : 普通傷害執行器 只能打1次 type = 1 參數：0：执行時間 1：傷害加成 2：类型 3: 递增倍率 4 递增最大次数
    /// </summary>
    public class ExecutorCombatIncreaseDamage : ExecutorCombatDamage
    {
        int increaseCount = 0;
        public ExecutorCombatIncreaseDamage(CombatBaseFinder combinFinder, CombatBaseFormula formulua) : base(combinFinder, formulua)
        {
        }


        public override int GetValidArgsCount()
        {
            return 5;
        }

        public float GetIncreaseRate()
        {
            return GetCurArg(3);
        }

        public float GetIncreaseMaxCount()
        {
            return GetCurArg(4);
        }

        protected override double GetExecutorValue()
        {
            var result = base.GetExecutorValue() * (1 + GetIncreaseRate() * increaseCount);
            if (increaseCount <= GetIncreaseMaxCount())
                increaseCount++;
            return result;
        }

        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {
            base.DoHit(target, data);
        }



    }
}