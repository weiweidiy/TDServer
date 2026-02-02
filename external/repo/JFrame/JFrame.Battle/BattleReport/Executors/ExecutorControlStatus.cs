using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 伤害效果 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：状态类型 1 眩晕， 2缴械 type
    /// </summary>
    public class ExecutorControlStatus : ExecutorNormal
    {
        int arg;

        List<IBattleUnit> controlTargets = new List<IBattleUnit>();
        public ExecutorControlStatus(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
            if (args != null && args.Length == 4)
            {
                arg = (int)args[3];
            }
            else
            {
                throw new System.Exception(this.GetType().ToString() + " 参数数量不对");
            }
        }

        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] argsObject = null)
        {
            foreach (var target in targets)
            {
                switch (arg)
                {
                    case 1: //眩晕
                        target.OnStunning(ActionType.All, Owner.GetDuration());
                        break;
                    case 2:
                        target.OnStunning(ActionType.Normal, Owner.GetDuration());
                        break;
                    case 3:
                        target.OnStunning(ActionType.Skill, Owner.GetDuration());
                        break;
                    default:
                        throw new Exception("没有实现控制类型 " + arg);
                }

                controlTargets.Add(target);
            }
        }

        public override void OnDetach()
        {
            base.OnDetach();

            foreach (var target in controlTargets)
            {
                switch (arg)
                {
                    case 1:
                        target.OnResumeFromStunning(ActionType.All);
                        break;
                    case 2:
                        target.OnResumeFromStunning(ActionType.Normal);
                        break;
                    case 3:
                        target.OnResumeFromStunning(ActionType.Skill);
                        break;
                    default:
                        throw new Exception("没有实现控制类型" + arg);
                }
            }
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
