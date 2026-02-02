
using System;

namespace JFramework
{
    public class ExecutorDanamicAttrChange : ExecutorChangeAttr
    {
        float baseValue;
        float xValue;
        float kValue;

        public ExecutorDanamicAttrChange(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
            if (args != null && args.Length >= 7)
            {
                attrType = (CombatAttribute)((int)args[3]);
                baseValue = args[4];
                xValue = args[5];
                kValue = args[6];
            }
            else
            {
                throw new System.Exception(this.GetType().ToString() + " 参数数量不对");
            }
        }

        protected override float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target, object[] args = null)
        {
            ExecuteInfo info = args[2] as ExecuteInfo;
            var curLevel = info.Value;

            float attrValue = 0;
            switch (attrType)
            {
                case CombatAttribute.ATK:
                    {
                        var curValue = target.Atk;
                        var finalValue = baseValue + xValue * Math.Pow(curLevel, kValue);
                        attrValue = (int)finalValue - curValue;
                    }
                    break;
                //case PVPAttribute.HP:
                //    {
                //        //attrValue = target.HP;
                //    }
                //    break;
                //case PVPAttribute.MaxHP:
                //    {
                //        //attrValue = target.MaxHP;
                //    }
                //    break;
                //case PVPAttribute.AtkSpeed:
                //    {
                //        //attrValue = target.AtkSpeed;
                //    }
                //    break;
                //case PVPAttribute.Critical:
                //    {
                //        //attrValue = target.Critical;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.CriticalDamage:
                //    {
                //        //attrValue = target.CriticalDamage;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.CriticalDamageResist:
                //    {
                //        //attrValue = target.CriticalDamageResist;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.DamageEnhance:
                //    {
                //        //attrValue = target.DamageEnhance;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.DamageReduce:
                //    {

                //        //attrValue = target.DamageReduce;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.SkillDamageEnhance:
                //    {
                //        //attrValue = target.SkillDamageEnhance;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }

                //case PVPAttribute.SkillDamageReduce:
                //    {
                //        //attrValue = target.SkillDamageReduce;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.Block:
                //    {
                //        //attrValue = target.Block;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.Puncture:
                //    {
                //        //attrValue = target.Puncture;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.ControlHit:
                //    {
                //        //attrValue = target.ControlHit;
                //       // return attrValue + arg * Owner.GetFoldCount();
                //    }
                //case PVPAttribute.ControlResistance:
                //    {
                //        //attrValue = target.ControlResistance;
                //        //return attrValue + arg * Owner.GetFoldCount();
                //    }
                default:
                    throw new Exception("没有实现pvp属性 " + attrType);

            }

            return attrValue;
            //return attrValue * arg * Owner.GetFoldCount();
        }
    }

}


///// <summary>
///// 伤害效果 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：伤害倍率  type = 1
///// </summary>
//public class ExecutorDamage : BaseExecutor
//{ 
//    /// <summary>
//    /// 伤害倍率
//    /// </summary>
//    protected float arg = 1f;


//    /// <summary>
//    /// 伤害效果，1：执行段数，2：延迟执行 3: 段数间隔 4：伤害倍率
//    /// </summary>
//    /// <param name="args"></param>
//    public ExecutorDamage(FormulaManager formulaManager, float[] args):base(formulaManager, args)
//    {
//        if (args != null && args.Length >= 4)
//        {
//            arg = args[3];
//        }
//        else
//        {
//            throw new System.Exception( this.GetType().ToString() + " 参数数量不对 缺少伤害倍率参数" );
//        }
//    }

//    /// <summary>
//    /// 执行命中
//    /// </summary>
//    /// <param name="caster"></param>
//    /// <param name="targets"></param>
//    /// <param name="reporter"></param>
//    public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets)
//    {
//        foreach(var target in targets) {

//            bool isCri = false;
//            bool isBlock = false;
//            var baseValue = (int)GetValue(caster, action, target);

//            int dmg = 0;
//            var owner = Owner as IBattleAction;

//            if(owner.Type == ActionType.Normal) //普通攻击
//                dmg = formulaManager.GetNormalDamageValue(baseValue, caster, action, target, out isCri, out isBlock);
//            else
//                dmg = formulaManager.GetSkillDamageValue(baseValue, caster, action, target, out isCri);

//            var info = new ExecuteInfo() { Value = (int)dmg, IsCri = isCri, IsBlock = isBlock };

//            //广播，可以改变这个值
//            NotifyHitTarget(target,info);

//            target.OnDamage(caster, action, info);

//            OnTargetHit(caster, action, target, info);
//        }

//    }

//    protected virtual void OnTargetHit(IBattleUnit caster, IBattleAction action, IBattleUnit target, ExecuteInfo info) { }

//    /// <summary>
//    /// 获取执行效果的值
//    /// </summary>
//    /// <param name="caster"></param>
//    /// <param name="action"></param>
//    /// <param name="actionArg">动作加成值</param>
//    /// <param name="target"></param>
//    /// <param name="isCri"></param>
//    /// <param name="isBlock"></param>
//    /// <returns></returns>
//    public virtual float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
//    {
//        return caster.Atk * arg;

//    }
//}
