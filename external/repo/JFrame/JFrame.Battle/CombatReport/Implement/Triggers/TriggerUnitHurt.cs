using System.Collections.Generic;
using System.Reflection;

namespace JFramework
{
    /// <summary>
    /// type 4  参数0: 概率  参数1：目标（0：受击者  1：攻击者） 参数2：反击伤害触发（0=不触发 1=触发） 参数3：累计伤害量 参数4：暴击筛选（0=不过滤 ， 1=过滤）
    /// </summary>
    public class TriggerUnitHurt : CombatBaseTrigger
    {
        List<CombatUnit> unitList = new List<CombatUnit>();

        Utility utility = new Utility();

        double damageAmout = 0;

        public TriggerUnitHurt(List<CombatBaseFinder> finders) : base(finders)
        {
        }

        public override int GetValidArgsCount()
        {
            return 5;
        }

        protected float GetRandomArg()
        {
            return GetCurArg(0);
        }

        protected int GetTargetType()
        {
            return (int)GetCurArg(1);
        }

        protected int GetTriggerType()
        {
            return (int)GetCurArg(2);
        }

        protected int GetDamageAmount()
        {
            return (int)GetCurArg(3);
        }

        protected bool GetIsCri()
        {
            return GetCurArg(4) == 1;
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            unitList.Clear();
            damageAmout = 0;

            if (finders != null && finders.Count > 0)
            {
                var finder = finders[0];

                var targets = finder.FindTargets(ExtraData); //获取目标
                targets = Filter(targets);
                if (targets != null && targets.Count > 0)
                {
                    foreach (var target in targets)
                    {
                        target.onDamaged += Target_onDamaging;
                        unitList.Add(target);
                    }
                }
            }
            else
            {
                ExtraData.Owner.onDamaged += Target_onDamaging;
                unitList.Add(ExtraData.Owner);

            }

        }


        public override void OnExitState()
        {
            base.OnExitState();

            foreach (var target in unitList)
            {
                target.onDamaged -= Target_onDamaging;
            }
        }

        private void Target_onDamaging(CombatExtraData data)
        {
            if (!utility.RandomHit(GetRandomArg() * 100))
                return;

            if (finders != null && finders.Count > 1)
            {
                var finder = finders[1];
                var targets = finder.FindTargets(ExtraData);
                ExtraData.Targets = targets;
            }
            else
            {
                var targetType = GetTargetType();
                if (targetType == 0) //受伤的作为目标
                    ExtraData.Targets = unitList;
                else
                {
                    //发起者作为目标（反射伤害用）
                    ExtraData.Targets = new List<CombatUnit>() { data.Caster };
                    ExtraData.Target = ExtraData.Targets[0];
                    ExtraData.ExtraArg = data.Value; //受到的伤害
                }
            }

            //是否触发反击
            if (GetTriggerType() == 0 && data.ValueType == CombatValueType.TurnBackDamage)
                return;

            //过滤非暴击
            if (GetIsCri() && !data.IsCri)
                return;

            damageAmout += data.Value;

            if (damageAmout >= GetDamageAmount())
            {
                SetOn(true);
                damageAmout = 0;
            }


        }


    }
}