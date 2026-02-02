using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    ///  type = 4 参数：0 执行周期 1=attrId, 2=倍率
    /// </summary>
    public class ExecutorCombatChangeAttribute : ExecutorCombatNormal
    {
        protected string uid = "ExecutorCombatChangeAttribute";

        protected List<KeyValuePair<CombatUnit, double>> targets = new List<KeyValuePair<CombatUnit, double>>();

        public override int GetValidArgsCount()
        {
            return 3;
        }

        protected float GetAttrIdArg()
        {
            return (int)GetCurArg(1);
        }

        protected virtual float GetRateArg()
        {
            return GetCurArg(2);
        }

        public ExecutorCombatChangeAttribute(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }

        protected override double GetExecutorValue()
        {
            return GetRateArg();
        }


        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {
            //var itemAttr = target.GetAttribute((CombatAttribute)GetAttrIdArg());
            //var finalValue = data.Value * itemAttr.CurValue;
            //uid = data.Action.Uid;

            //target.AddExtraValue((CombatAttribute)GetAttrIdArg(), uid, finalValue);
            uid = data.Action.Uid;
            data.Value = GetRateArg() * data.FoldCount;
            var finalValue = target.OnAttrChanged(data, (CombatAttribute)GetAttrIdArg());

            targets.Add(new KeyValuePair<CombatUnit, double>(target, finalValue));
        }

        public override void OnStop()
        {
            base.OnStop();

            foreach (var item in targets)
            {
                var target = item.Key;
                var value = item.Value;
                target.MinusExtraValue((CombatAttribute)GetAttrIdArg(), uid, value);
            }

            targets.Clear();
        }

        protected override void SetValueType(CombatExtraData data)
        {
            data.ValueType = CombatValueType.None;
        }
    }
}