namespace JFramework
{
    /// <summary>
    /// 加血执行器type = 3：参数：0 执行周期 , 1 : 加成
    /// </summary>
    public class ExecutorCombatHeal : ExecutorCombatNormal
    {
        public ExecutorCombatHeal(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }

        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {
            target.OnHeal(data);
        }

        protected override double GetExecutorValue()
        {
            return  GetRateArg();
        }

        /// <summary>
        /// 获取加成参数
        /// </summary>
        /// <returns></returns>
        protected float GetRateArg()
        {

            return GetCurArg(1);
        }

        public override int GetValidArgsCount()
        {
            return 2;
        }

        protected override void SetValueType(CombatExtraData data)
        {
            data.ValueType = CombatValueType.Heal;
        }
    
    }
}