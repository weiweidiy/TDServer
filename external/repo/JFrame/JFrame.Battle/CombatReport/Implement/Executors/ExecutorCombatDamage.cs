namespace JFramework
{

    /// <summary>
    /// type 1 普通傷害執行器 只能打1次 type = 1 參數：0：执行時間 1：傷害加成 2: 类型（0：无  1：普通伤害， 100：灼烧伤害 ）
    /// </summary>
    public class ExecutorCombatDamage : ExecutorCombatNormal
    {
        public ExecutorCombatDamage(CombatBaseFinder combinFinder, CombatBaseFormula formulua) : base(combinFinder, formulua)
        {
        }

        public override int GetValidArgsCount()
        {
            return 3;
        }

        protected float GetRateArg()
        {
            return GetCurArg(1);
        }

        protected int GetValueType()
        {
            return (int)GetCurArg(2);
        }



        protected override double GetExecutorValue()
        {
            return GetRateArg();
        }

        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {

            //var bullet = new CombatBullet(this, target , data); 
            //Owner.AddBullet(bullet);

            target.OnDamage(data);
            StealHp(data);
        }

        /// <summary>
        /// 吸血
        /// </summary>
        /// <param name="data"></param>
        protected void StealHp(CombatExtraData data)
        {
            var hpSteal = (double)data.Caster.GetAttributeCurValue(CombatAttribute.HpSteal);
            var d = new CombatExtraData();
            d.Caster = data.Caster;
            d.Target = data.Caster;
            d.Value = data.Value * hpSteal;
            d.Owner = data.Owner;
            d.Action = data.Action;

            if (d.Value < 1)
                return;

            //d.Target = data.Caster;
            data.Caster.OnHeal(d);
        }


        protected override void SetValueType(CombatExtraData data)
        {
            data.ValueType = (CombatValueType)GetValueType();
        }
    }
}