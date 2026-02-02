
using System;
using System.Collections.Generic;

namespace JFramework
{
  




    /// <summary>
    /// 发动者自身百分x的生命为数值，添加给目标， 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：自身HP比率  type = 5
    /// </summary>
    public class ExecutorMaxHpUp : ExecutorDamage
    {
        int hpValue;

        public ExecutorMaxHpUp(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
        }

        public override void OnAttach(IAttachOwner owner)
        {
            base.OnAttach(owner);

            var o = owner as IBattleAction;
            if (o == null)
                throw new Exception("attach owner 转换失败 ");

            hpValue = (int)(o.Owner.MaxHP * arg);
        }

        /// <summary>
        /// 获取需要增加的生命上限
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public override float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
        {
            return hpValue;
        }


        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] arg = null)
        {
            foreach (var target in targets)
            {
                var info = new ExecuteInfo() { Value = (int)GetValue(caster, action, target) };
                //广播，可以改变这个值
                NotifyHittingTarget(target, info);

                target.OnMaxHpUp(caster, action, info);
                //target.MaxHPUpgrade((int)GetValue(caster, action , target));
            }

        }


    }
}


///// <summary>
///// 发动者自身百分x的生命为数值，添加给目标， 参数  1：执行段数，2：延迟执行 3: 段数间隔  4 ：自身HP比率  type = 5
///// </summary>
//public class ExecutorMaxHpUp : ExecutorDamage
//{
//    int hpValue;

//    public ExecutorMaxHpUp(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
//    {
//    }

//    public override void OnAttach(IBattleAction action)
//    {
//        base.OnAttach(action);

//        hpValue = (int)(action.Owner.MaxHP * arg);
//    }

//    /// <summary>
//    /// 获取需要增加的生命上限
//    /// </summary>
//    /// <param name="caster"></param>
//    /// <param name="action"></param>
//    /// <param name="target"></param>
//    /// <returns></returns>
//    public override float GetValue(IBattleUnit caster, IBattleAction action, IBattleUnit target)
//    {
//        return hpValue;
//    }


//    public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets)
//    {
//        foreach(var target in targets)
//        {
//            var info = new ExecuteInfo() { Value = (int)GetValue(caster, action, target) };
//            //广播，可以改变这个值
//            NotifyHitTarget(target, info);

//            target.OnMaxHpUp(caster, action, info);
//            //target.MaxHPUpgrade((int)GetValue(caster, action , target));
//        }

//    }


//}

