namespace JFramework
{
    /// <summary>
    ///  type=7 参数0： 执行周期 ， 参数1: bufftype(0=buffer，1=debuffer, 2=所有) , 参数2： 概率
    /// </summary>
    public class ExecutorCombatRemoveBuffer : ExecutorCombatNormal
    {
        Utility utilty = new Utility();
        public ExecutorCombatRemoveBuffer(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }

        public override int GetValidArgsCount()
        {
            return 3;
        }

        protected int GetBufferTypeArg()
        {
            return (int)GetCurArg(1);
        }

        protected float GetRandomArg()
        {
            return GetCurArg(2);
        }

        protected override double GetExecutorValue()
        {
            return 0;
        }


        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {
            if (!utilty.RandomHit(GetRandomArg() * 100))
                return;

            var clone = extraData.Clone() as CombatExtraData;

            //var buffer = context.CombatBufferFactory.CreateBuffer(GetBuffIdArg(), clone);
            //buffer.SetCurFoldCount(GetBuffFoldArg());
            //buffer.SetDuration(GetBuffDurationArg());
            //buffer.OnAttach(target);
            //target.AddBuffer(buffer);

            //var buffer = target.get
        }

        protected override void SetValueType(CombatExtraData data)
        {
            data.ValueType = CombatValueType.None;
        }
    }
}