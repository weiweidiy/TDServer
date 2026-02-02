
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace JFramework
{

    /// <summary>
    /// 改变属性执行器：参数 4：属性id  参数5：改变百分比
    /// </summary>
    public class ExecutorChangeAttr : ExecutorNormal
    {
        protected CombatAttribute attrType;
        protected float arg;

        //真正改变的值
        protected float valueChanged;

        /// <summary>
        /// 发生属性改变的目标列表
        /// </summary>
        protected List<IBattleUnit> changedTargets = new List<IBattleUnit>();
        public ExecutorChangeAttr(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
            if (args != null && args.Length >= 5)
            {
                attrType = (CombatAttribute)((int)args[3]);
                arg = args[4];
            }
            else
            {
                throw new System.Exception(this.GetType().ToString() + " 参数数量不对");
            }
        }

        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] args = null)
        {

            foreach (var target in targets)
            {


                var value = GetValue(caster, action, target, args);

                if (value > 0)
                    valueChanged += UpgradeValue(target, Math.Abs(value));
                else
                    valueChanged -= ReduceValue(target, Math.Abs(value));

                changedTargets.Add(target);
            }
        }

        public override void OnDetach()
        {
            base.OnDetach();

            foreach (var t in changedTargets)
            {
                //var value = GetValue(null, null, t);
                if (valueChanged > 0)
                    ReduceValue(t, valueChanged);
                else
                    UpgradeValue(t, valueChanged);
            }
        }

        protected float UpgradeValue(IBattleUnit target, float value)
        {
            switch (attrType)
            {
                case CombatAttribute.ATK:
                    {
                        return target.AtkUpgrade((int)value);
                    }
                case CombatAttribute.MaxHP: return 0;
                //case CombatAttribute.MaxHP:
                //    {
                //        return target.MaxHPUpgrade((int)value);
                //    }
                //case CombatAttribute.AtkSpeed: return 0;
                //case CombatAttribute.Critical:
                //    {
                //        return target.CriUpgrade(value);
                //    }
                //case CombatAttribute.CriticalDamage:
                //    {
                //        return target.CriticalDamageUpgrade(value);
                //    }
                //case CombatAttribute.CriticalDamageResist:
                //    {
                //        return target.CriticalDamageResistUpgrade(value);
                //    }
                //case CombatAttribute.ControlHit:
                //    {
                //        return target.ControlHitUpgrade(value);
                //    }
                //case CombatAttribute.ControlResistance:
                //    {
                //        return target.ControlResistanceUpgrade(value);
                //    }
                //case CombatAttribute.DamageEnhance:
                //    {
                //        return target.DamageEnhanceUpgrade(value);
                //    }
                //case CombatAttribute.DamageReduce:
                //    {
                //        return target.DamageReduceUpgrade(value);
                //    }
                //case CombatAttribute.SkillDamageEnhance:
                //    {
                //        return target.SkillDamageEnhanceUpgrade(value);
                //    }
                //case CombatAttribute.SkillDamageReduce:
                //    {
                //        return target.SkillDamageReduceUpgrade(value);
                //    }
                //case CombatAttribute.Block:
                //    {
                //        return target.BlockUpgrade(value);
                //    }
                //case CombatAttribute.Puncture:
                //    {
                //        return target.PunctureUpgrade(value);
                //    }
                default:
                    throw new Exception("没有实现pvp属性 " + attrType);
            }
        }

        protected float ReduceValue(IBattleUnit target, float value)
        {
            switch (attrType)
            {
                case CombatAttribute.ATK:
                    {
                        return target.AtkReduce((int)value);
                    }
                case CombatAttribute.MaxHP: return 0;
                //case CombatAttribute.MaxHP:
                //    {
                //        return target.MaxHPReduce((int)value);
                //    }
                //case CombatAttribute.AtkSpeed: return 0;
                //case CombatAttribute.Critical:
                //    {
                //        return target.CriReduce(value);
                //    }
                //case CombatAttribute.CriticalDamage:
                //    {
                //        return target.CriticalDamageReduce(value);
                //    }
                //case CombatAttribute.CriticalDamageResist:
                //    {
                //        return target.CriticalDamageResistReduce(value);
                //    }
                //case CombatAttribute.ControlHit:
                //    {
                //        return target.ControlHitReduce(value);
                //    }
                //case CombatAttribute.ControlResistance:
                //    {
                //        return target.ControlResistanceReduce(value);
                //    }
                //case CombatAttribute.DamageEnhance:
                //    {
                //        return target.DamageEnhanceReduce(value);
                //    }
                //case CombatAttribute.DamageReduce:
                //    {
                //        return target.DamageReduceReduce(value);
                //    }
                //case CombatAttribute.SkillDamageEnhance:
                //    {
                //        return target.SkillDamageEnhanceReduce(value);
                //    }
                //case CombatAttribute.SkillDamageReduce:
                //    {
                //        return target.SkillDamageReduceReduce(value);
                //    }
                //case CombatAttribute.Block:
                //    {
                //        return target.BlockReduce(value);
                //    }
                //case CombatAttribute.Puncture:
                //    {
                //        return target.PunctureReduce(value);
                //    }
                default:
                    throw new Exception("没有实现pvp属性 " + attrType);
            }
        }

        protected virtual float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target, object[] args = null)
        {
            float attrValue = 0;
            //switch (attrType)
            //{
            //    case CombatAttribute.ATK:
            //        {
            //            attrValue = target.Atk;
            //        }
            //        break;
            //    case CombatAttribute.MaxHP:
            //        {
            //            attrValue = target.HP;
            //        }
            //        break;
            //    case CombatAttribute.MaxHP:
            //        {
            //            attrValue = target.MaxHP;
            //        }
            //        break;
            //    case CombatAttribute.AtkSpeed:
            //        {
            //            attrValue = target.AtkSpeed;
            //        }
            //        break;
            //    case CombatAttribute.Critical:
            //        {
            //            attrValue = target.Critical;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.CriticalDamage:
            //        {
            //            attrValue = target.CriticalDamage;
            //            return  arg* Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.CriticalDamageResist:
            //        {
            //            attrValue = target.CriticalDamageResist;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.DamageEnhance:
            //        {
            //            attrValue = target.DamageEnhance;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.DamageReduce:
            //        {

            //            attrValue = target.DamageReduce;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.SkillDamageEnhance:
            //        {
            //            attrValue = target.SkillDamageEnhance;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }

            //    case CombatAttribute.SkillDamageReduce:
            //        {
            //            attrValue = target.SkillDamageReduce;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.Block:
            //        {
            //            attrValue = target.Block;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.Puncture:
            //        {
            //            attrValue = target.Puncture;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.ControlHit:
            //        {
            //            attrValue = target.ControlHit;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    case CombatAttribute.ControlResistance:
            //        {
            //            attrValue = target.ControlResistance;
            //            return arg * Owner.GetFoldCount() - valueChanged;
            //        }
            //    default:
            //        throw new Exception("没有实现pvp属性 " + attrType);

            //}

            return attrValue * arg * Owner.GetFoldCount();
        }

        //protected virtual float CalcCD(float originValue)
        //{
        //    return originValue / (1 + arg * Owner.GetFoldCount());
        //}
        //float AtkSpeedUpgrade()
        //{
        //    var result = 0f;
        //    var actions = Owner.Owner.GetActions();
        //    foreach (var action in actions)
        //    {
        //        if (action.Type == ActionType.Normal) //普通攻击
        //        {
        //            var cdTrigger = action.GetCDTrigger();
        //            var cdTimeTrigger = cdTrigger as CDTimeTrigger;
        //            if (cdTimeTrigger != null)
        //            {
        //                var args = cdTimeTrigger.GetArgs();
        //                var originValue = args[0];

        //                var cd = CalcCD(originValue);
        //                result = originValue - cd;
        //                args[0] = cd;
        //                cdTimeTrigger.SetArgs(args);

        //                //Debug.LogError(target.Name + "OnAttach new cd " + cdTimeTrigger.GetArgs()[0]);
        //            }
        //        }
        //    }
        //    return result;
        //}
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
