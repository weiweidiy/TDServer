
using System;
using System.Collections.Generic;



namespace JFramework
{


    /// <summary>
    /// 伤害效果 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：伤害倍率  type = 1
    /// </summary>
    public class ExecutorDamage : ExecutorNormal
    {
        /// <summary>
        /// 伤害倍率
        /// </summary>
        protected float arg = 1f;


        /// <summary>
        /// 伤害效果，1：执行段数，2：延迟执行 3: 段数间隔 4：伤害倍率
        /// </summary>
        /// <param name="args"></param>
        public ExecutorDamage(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
            if (args != null && args.Length >= 4)
            {
                arg = args[3];
            }
            else
            {
                throw new System.Exception(this.GetType().ToString() + " 参数数量不对 缺少伤害倍率参数");
            }
        }

        /// <summary>
        /// 执行命中
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="targets"></param>
        /// <param name="reporter"></param>
        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] args = null)
        {
            foreach (var target in targets)
            {

                bool isCri = false;
                bool isBlock = false;
                var baseValue = (int)GetValue(caster, action, target);

                int dmg = 0;
                var owner = Owner as IBattleAction;
                //if (owner == null)
                //    throw new Exception("attach owner 转换失败 ");

                if (owner != null)
                {
                    if (owner.Type == ActionType.Normal) //普通攻击
                        dmg = formulaManager.GetNormalDamageValue(baseValue, caster, action, target, out isCri, out isBlock);
                    else
                        dmg = formulaManager.GetSkillDamageValue(baseValue, caster, action, target, out isCri);
                }
                else
                {

                    dmg = baseValue;
                }

                var info = new ExecuteInfo() { Value = (int)dmg, IsCri = isCri, IsBlock = isBlock, Source = caster };

                //广播，可以改变这个值
                NotifyHittingTarget(target, info);

                target.OnDamage(caster, action, info);

                OnTargetHit(caster, action, target, info);

                NotifyHitedTarget(caster, info, target);
            }

        }

        protected virtual void OnTargetHit(IBattleUnit caster, IBattleAction action, IBattleUnit target, ExecuteInfo info) { }

        /// <summary>
        /// 获取执行效果的值
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="actionArg">动作加成值</param>
        /// <param name="target"></param>
        /// <param name="isCri"></param>
        /// <param name="isBlock"></param>
        /// <returns></returns>
        public virtual float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
        {
            return caster.Atk * arg;

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
