using System;
using System.Collections.Generic;
using System.Drawing;
using static System.Collections.Specialized.BitVector32;

namespace JFramework
{


    /// <summary>
    /// 执行器基础类： 可以延迟执行，多段执行
    /// </summary>
    public abstract class ExecutorNormal : ExecutorBase
    {
        //public event Action<IBattleUnit, ExecuteInfo> onHittingTarget;

        //protected void NotifyHitTarget(IBattleUnit target, ExecuteInfo info)
        //{
        //    onHittingTarget?.Invoke(target, info);
        //}


        //protected FormulaManager formulaManager;

        //protected float[] args;

        /// <summary>
        /// 参数：1：执行段数，2：延迟执行 3: 段数间隔
        /// </summary>
        /// <param name="args"></param>
        public ExecutorNormal(FormulaManager formulaManager, float[] args):base(formulaManager, args) 
        {

            //this.formulaManager = formulaManager;
            //this.args = args;

            if (args != null && args.Length >= 3)
            {
                count = args[0];
                delay = args[1];
                interval = args[2];
            }
            else
            {
                count = 1;
                delay = 0;
                interval = 0.25f;
            }

            delayed = delay == 0f;
        }

        ///// <summary>
        ///// 是否激活
        ///// </summary>
        //public bool Active { get; private set; }

        //public virtual IAttachOwner Owner { get; private set; }

        /// <summary>
        /// 攻击次数
        /// </summary>
        float count;

        /// <summary>
        /// 攻击间隔
        /// </summary>
        float interval;

        /// <summary>
        /// 延迟命中
        /// </summary>
        float delay;

        /// <summary>
        /// 是否已经延迟过了
        /// </summary>
        bool delayed;
        /// <summary>
        /// 临时变量
        /// </summary>
        float delta;

        /// <summary>
        /// 临时计数
        /// </summary>
        int tempCount;

        /// <summary>
        /// 执行对象相关缓存
        /// </summary>
        IBattleUnit caster;
        IBattleAction action;
        List<IBattleUnit> targets;
        object[] triggerArg;


        /// <summary>
        /// 更新帧
        /// </summary>
        /// <param name="frame"></param>
        public override void Update(CombatFrame frame)
        {
            if (!Executing)
                return;

            delta += frame.DeltaTime;

            if (!delayed)
            {
                if (delta - delay >= 0f)
                {
                    delta -= delay;
                    delayed = true;
                }
                return;
            }

            if (delta < interval)
                return;

            delta = 0f;
            //延迟完成了
            Hit(caster, action, targets, triggerArg);

            tempCount++;

            if (tempCount >= count)
            {
                Executing = false;
                delayed = false;
                tempCount = 0;
            }
        }

        ///// <summary>
        ///// 命中效果，子类实现
        ///// </summary>
        ///// <param name="caster"></param>
        ///// <param name="action"></param>
        ///// <param name="targets"></param>
        //public abstract void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object triggerArg = null);

        /// <summary>
        /// 准备释放
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="target"></param>
        /// <exception cref="Exception"></exception>
        public override void ReadyToExecute(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] triggerArgs = null)
        {
            if (Executing)
                throw new Exception("执行器正在执行中，无法再次执行" + this.GetType().ToString());

            //激活
            Executing = true;

            this.caster = caster;
            this.action = action;
            this.targets = targets;
            triggerArg = triggerArgs;
        }

        //public virtual void OnAttach(IAttachOwner action)
        //{
        //    Owner = action;
        //}

        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            delayed = delay == 0f;
            tempCount = 0;
            delta = 0;
        }

        //public virtual void OnDetach()
        //{
        //    //throw new NotImplementedException();
        //}

        //public float[] GetArgs()
        //{
        //    return args;
        //}

        public override void SetArgs(float[] args)
        {
            base.SetArgs(args);

            if (args != null && args.Length >= 3)
            {
                count = args[0];
                delay = args[1];
                interval = args[2];
            }
        }
    }

}


///// <summary>
///// 执行器基础类： 可以延迟执行，多段执行
///// </summary>
//public abstract class BaseExecutor : IBattleExecutor
//{
//    public event Action<IBattleUnit, ExecuteInfo> onHittingTarget;

//    protected void NotifyHitTarget(IBattleUnit target, ExecuteInfo info)
//    {
//        onHittingTarget?.Invoke(target, info);
//    }


//    protected FormulaManager formulaManager;

//    /// <summary>
//    /// 参数：1：执行段数，2：延迟执行 3: 段数间隔
//    /// </summary>
//    /// <param name="args"></param>
//    public BaseExecutor(FormulaManager formulaManager, float[] args) {

//        this.formulaManager = formulaManager;

//        if (args != null && args.Length >= 3)
//        {
//            count = args[0];
//            delay = args[1];
//            interval = args[2];
//        }
//        else
//        {
//            count = 1;
//            delay = 0;
//            interval = 0.25f;
//        }

//        delayed = delay == 0f;
//    }

//    /// <summary>
//    /// 是否激活
//    /// </summary>
//    public bool Active { get; private set; }

//    public virtual IBattleAction Owner { get; private set; }

//    /// <summary>
//    /// 攻击次数
//    /// </summary>
//    float count;

//    /// <summary>
//    /// 攻击间隔
//    /// </summary>
//    float interval;

//    /// <summary>
//    /// 延迟命中
//    /// </summary>
//    float delay;

//    /// <summary>
//    /// 是否已经延迟过了
//    /// </summary>
//    bool delayed;
//    /// <summary>
//    /// 临时变量
//    /// </summary>
//    float delta;

//    /// <summary>
//    /// 临时计数
//    /// </summary>
//    int tempCount;

//    /// <summary>
//    /// 执行对象相关缓存
//    /// </summary>
//    IBattleUnit caster;
//    IBattleAction action;
//    List<IBattleUnit> targets;


//    /// <summary>
//    /// 更新帧
//    /// </summary>
//    /// <param name="frame"></param>
//    public void Update(BattleFrame frame)
//    {
//        if (!Active)
//            return;

//        delta += frame.DeltaTime;

//        if (!delayed)
//        {
//            if (delta - delay >= 0f)
//            {
//                delta -= delay;
//                delayed = true;
//            }
//            return;
//        }

//        if (delta < interval)
//            return;

//        delta = 0f;
//        //延迟完成了
//        Hit(caster, action, targets);

//        tempCount++;

//        if (tempCount >= count)
//        {
//            Active = false;
//            delayed = false;
//            tempCount = 0;
//        }
//    }

//    /// <summary>
//    /// 命中效果，子类实现
//    /// </summary>
//    /// <param name="caster"></param>
//    /// <param name="action"></param>
//    /// <param name="target"></param>
//    public abstract void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> target);

//    /// <summary>
//    /// 准备释放
//    /// </summary>
//    /// <param name="caster"></param>
//    /// <param name="action"></param>
//    /// <param name="target"></param>
//    /// <exception cref="Exception"></exception>
//    public void ReadyToExecute(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets)
//    {
//        if (Active)
//            throw new Exception("执行器正在执行中，无法再次执行" + this.GetType().ToString());

//        //激活
//        Active = true;

//        this.caster = caster;
//        this.action = action;
//        this.targets = targets;
//    }

//    public virtual void OnAttach(IBattleAction action)
//    {
//        Owner = action;
//    }

//    /// <summary>
//    /// 重置
//    /// </summary>
//    public void Reset()
//    {
//        Active = false;
//        delayed = delay == 0f;
//        tempCount = 0;
//        delta = 0;
//    }

//}