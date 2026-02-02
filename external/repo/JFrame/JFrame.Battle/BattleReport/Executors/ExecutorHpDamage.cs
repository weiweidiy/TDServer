
using System;

namespace JFramework
{



    /// <summary>
    /// 伤害效果 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：伤害倍率  5: 自己的属性类型， 6：自身属性百分比  type = 4, 
    /// </summary>
    public class ExecutorHpDamage : ExecutorDamage
    {
        CombatAttribute attrType;
        float rate;
        public ExecutorHpDamage(FormulaManager formulaManager, float[] args) : base(formulaManager, args) 
        {
            if (args != null && args.Length >= 6)
            {
                attrType = (CombatAttribute)args[4];
                rate = (float)args[5];
            }
            else
            {
                throw new System.Exception(this.GetType().ToString() + " 参数数量不对");
            }
        }

        public override float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
        {
            var value = target.MaxHP * arg;
            int max = 0;

            switch(attrType)
            {
                case CombatAttribute.MaxHP:
                    {
                        max = (int)(caster.MaxHP * rate);
                    }
                    break;
                case CombatAttribute.ATK:
                    {
                        max = (int)(caster.Atk * rate);
                    }
                    break;
                //case CombatAttribute.MaxHP:
                //    {
                //        max =(int)(caster.HP * rate);
                //    }
                //    break;
                default:
                    throw new Exception("没有实现pvp属性类型 " + attrType);
            }

            return Math.Min(max, value);
        }
    }
}


