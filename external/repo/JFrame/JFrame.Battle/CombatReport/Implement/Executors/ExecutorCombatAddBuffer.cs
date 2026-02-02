using System;

namespace JFramework
{

    /// <summary>
    ///  type=5 参数：0 执行周期 1: bufferId, 2: foldCount 3:duration 4:概率
    /// </summary>
    public class ExecutorCombatAddBuffer : ExecutorCombatNormal
    {
        Utility utilty = new Utility();
        public ExecutorCombatAddBuffer(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }

        public override int GetValidArgsCount()
        {
            return 5;
        }

        protected int GetBuffIdArg()
        {
            return (int)GetCurArg(1);
        }


        protected int GetBuffFoldArg()
        {
            return (int)GetCurArg(2);
        }

        protected int GetBuffDurationArg()
        {
            return (int)GetCurArg(3);
        }

        protected float GetRandomArg()
        {
            return GetCurArg(4);
        }


        protected override double GetExecutorValue()
        {
            return GetBuffIdArg();
        }



        protected override void DoHit(CombatUnit target, CombatExtraData extraData)
        {
            if (!utilty.RandomHit(GetRandomArg() * 100))
                return;

            var clone = extraData.Clone() as CombatExtraData;
            var buffer = context.CombatBufferFactory.CreateBuffer(GetBuffIdArg(), clone, GetBuffFoldArg());
            //buffer.SetCurFoldCount(GetBuffFoldArg());
            buffer.SetDuration(GetBuffDurationArg());
            //buffer.OnAttach(target);
            target.AddBuffer(buffer);
        }

        protected override void SetValueType(CombatExtraData data)
        {
            data.ValueType = CombatValueType.None;
        }
    }
}