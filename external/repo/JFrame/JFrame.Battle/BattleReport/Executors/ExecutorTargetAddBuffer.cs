
using System;
using System.Collections.Generic;

namespace JFramework
{
  


    /// <summary>
    /// 1：执行段数，2：延迟执行 3: 段数间隔 4：buffId 5:层数 6:概率0-1 type = 2
    /// </summary>
    public class ExecutorTargetAddBuffer : ExecutorNormal
    {
        protected int bufferId;
        protected int foldCount;
        protected float rate;//添加概率

        /// <summary>
        /// 第四个参数是bufferID, 第5个参数是buffer值
        /// </summary>
        /// <param name="args"></param>
        public ExecutorTargetAddBuffer(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
            if (args != null && args.Length >= 6)
            {
                bufferId = (int)args[3];
                foldCount = (int)args[4];
                rate = (float)args[5];
            }
            else
            {
                throw new System.Exception("添加buffer executor 参数数量不对 ");
            }
        }

        /// <summary>
        /// 命中，开始添加buffer
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="target"></param>
        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] args = null)
        {
            foreach (IBattleUnit target in targets)
            {
                if (!HitRate())
                    continue;

                //如果是减溢，则进行抵抗
                var owner = Owner as IBattleAction;
                if (owner == null)
                    throw new Exception("attach owner 转换失败");

                if (!owner.Owner.IsBuffer(bufferId) && HitAnti(caster, action, target))
                {
                    //通知抵抗
                    target.OnDebuffAnti(caster, action, bufferId);
                    continue;
                }


                AddBuff(caster, action, target);
            }

        }

        protected virtual void AddBuff(IBattleUnit caster, IBattleAction action, IBattleUnit target)
        {
            //添加buff
            target.AddBuffer(caster, bufferId, foldCount);
        }

        /// <summary>
        /// 是否命中
        /// </summary>
        /// <returns></returns>
        protected bool HitRate()
        {
            var r = new Random().NextDouble();
            return r < rate;
        }

        /// <summary>
        /// 是否抵抗
        /// </summary>
        /// <returns></returns>
        protected bool HitAnti(IBattleUnit caster, IBattleAction action, IBattleUnit target)
        {
            return formulaManager.IsDebuffAnti(caster, action, target);
        }
    }
}

///// <summary>
///// 1：执行段数，2：延迟执行 3: 段数间隔 4：buffId 5:层数 6:概率0-1 type = 2
///// </summary>
//public class ExecutorTargetAddBuffer : BaseExecutor
//{
//    protected int bufferId;
//    protected int foldCount;
//    protected float rate;//添加概率

//    /// <summary>
//    /// 第四个参数是bufferID, 第5个参数是buffer值
//    /// </summary>
//    /// <param name="args"></param>
//    public ExecutorTargetAddBuffer(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
//    {
//        if (args != null && args.Length >= 6)
//        {
//            bufferId = (int)args[3];
//            foldCount = (int)args[4];
//            rate = (float)args[5];
//        }
//        else
//        {
//            throw new System.Exception("添加buffer executor 参数数量不对 ");
//        }
//    }

//    /// <summary>
//    /// 命中，开始添加buffer
//    /// </summary>
//    /// <param name="caster"></param>
//    /// <param name="action"></param>
//    /// <param name="target"></param>
//    public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets)
//    {
//        foreach(IBattleUnit target in targets)
//        {
//            if (!HitRate())
//                continue;

//            //如果是减溢，则进行抵抗


//            if (!Owner.Owner.IsBuffer(bufferId) &&   HitAnti(caster, action, target))
//            {
//                //通知抵抗
//                target.OnDebuffAnti(caster, action, bufferId);
//                continue;
//            }


//            AddBuff( caster,  action, target);
//        }

//    }

//    protected virtual void AddBuff(IBattleUnit caster, IBattleAction action, IBattleUnit target)
//    {
//        //添加buff
//        target.AddBuffer(caster, bufferId, foldCount);
//    }

//    /// <summary>
//    /// 是否命中
//    /// </summary>
//    /// <returns></returns>
//    protected bool HitRate()
//    {
//        var r = new Random().NextDouble();
//        return r < rate;
//    }

//    /// <summary>
//    /// 是否抵抗
//    /// </summary>
//    /// <returns></returns>
//    protected bool HitAnti(IBattleUnit caster, IBattleAction action, IBattleUnit target)
//    {     
//        return formulaManager.IsDebuffAnti(caster, action, target); 
//    }
//}


