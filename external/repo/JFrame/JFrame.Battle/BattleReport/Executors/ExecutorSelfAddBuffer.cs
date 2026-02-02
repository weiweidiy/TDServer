using System;
using System.Collections.Generic;

namespace JFramework
{


    /// <summary>
    /// 强制给自己添加BUFF，比如可以攻击敌人，并给自己添加BUFF 1：执行段数，2：延迟执行 3: 段数间隔 4：buffId 5:层数 type = 6
    /// </summary>
    public class ExecutorSelfAddBuffer : ExecutorTargetAddBuffer
    {
        public ExecutorSelfAddBuffer(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
        }

        /// <summary>
        /// 命中，开始添加buffer
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="target"></param>
        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] arg = null)
        {
            if (!HitRate())
                return;

            //to do:如果是减溢，则进行抵抗

            //添加buff
            caster.AddBuffer(caster, bufferId, foldCount);

        }
    }
}

///// <summary>
///// 强制给自己添加BUFF，比如可以攻击敌人，并给自己添加BUFF 1：执行段数，2：延迟执行 3: 段数间隔 4：buffId 5:层数 type = 6
///// </summary>
//public class ExecutorSelfAddBuffer : ExecutorTargetAddBuffer
//{
//    public ExecutorSelfAddBuffer(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
//    {
//    }

//    /// <summary>
//    /// 命中，开始添加buffer
//    /// </summary>
//    /// <param name="caster"></param>
//    /// <param name="action"></param>
//    /// <param name="target"></param>
//    public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets)
//    {
//        if (!HitRate())
//            return;

//        //to do:如果是减溢，则进行抵抗

//        //添加buff
//        caster.AddBuffer(caster, bufferId, foldCount);

//    }
//}
