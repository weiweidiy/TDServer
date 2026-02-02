using JFramework.Common;
using System;
using System.Collections;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// action 组件基类 0:攻擊距離 1:查找數量
    /// </summary>
    public abstract class BaseActionComponent : IArgsable, ICombatAttachable<CombatAction>, ICombatUpdatable
    {
        CombatAction _owner;
        public virtual CombatAction Owner => _owner;

        /// <summary>
        /// 原始参数
        /// </summary>
        float[] originArgs; //0:攻擊距離

        /// <summary>
        /// 当前参数值
        /// </summary>
        protected float[] curArgs;

        /// <summary>
        /// 上下文
        /// </summary>
        public CombatContext context;

        /// <summary>
        /// 是否启动了
        /// </summary>
        bool isStart; 

        /// <summary>
        /// 初始化组件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args"></param>
        public void Initialize(CombatContext context, float[] args)
        {
            if (args.Length < GetValidArgsCount())
                throw new ArgumentException($"参数数量不正确{GetType().ToString()} 需要 {GetValidArgsCount()} 实际{args.Length}");

            this.context = context;
            originArgs = args;
            SetCurArgs(originArgs);
        }

        public float[] GetCurArgs()
        {
            return curArgs;
        }

        public float[] GetOriginArgs()
        {
            return originArgs;
        }

        public void ResetArgs()
        {
            if (originArgs.Length != curArgs.Length)
                throw new Exception("原始参数列表长度和当前参数列表长度不一致，无法重置！" + GetType().Name);

            Array.Copy(originArgs, curArgs, originArgs.Length);
        }

        public void SetCurArgs(float[] args)
        {
            if (args == null)
                throw new ArgumentNullException("参数列表为空！" + GetType().Name);

            if (curArgs == null)
                curArgs = new float[args.Length];

            if (curArgs.Length != args.Length)
                throw new Exception("原始参数列表长度和当前参数列表长度不一致，无法赋值！" + GetType().Name);

            Array.Copy(args, curArgs, args.Length);
        }

        public void SetOrginArgs(float[] args)
        {
            originArgs = args;
            SetCurArgs(originArgs);
        }

        public void SetCurArg(int index, float arg)
        {
            if (curArgs == null || curArgs.Length <= index)
                throw new ArgumentOutOfRangeException("设置参数时索引越界！" + GetType().Name);

            curArgs[index] = arg;
        }

        public float GetCurArg(int index)
        {
            if (curArgs == null || curArgs.Length <= index)
                throw new ArgumentOutOfRangeException("获取参数时索引越界！" + GetType().Name);

            return curArgs[index];
        }

        public void Update(CombatFrame frame) {
            if (isStart)
                OnUpdate(frame);
        }

        protected abstract void OnUpdate(CombatFrame frame);


        public virtual void OnAttach(CombatAction target)
        {
            _owner = target;
        }

        public virtual void OnDetach()
        {
            _owner = null;
        }

        /// <summary>
        /// 所有组件都完成了初始化了,只启动1次
        /// </summary>
        public virtual void OnStart() {

            isStart = true;
        }



        /// <summary>
        /// 停止组件的效果
        /// </summary>
        public virtual void OnStop()
        {
            isStart = false;
        }

        public virtual void OnEnterState()
        {

        }

        /// <summary>
        /// 退出当前状态
        /// </summary>
        public virtual void OnExitState()
        {

        }

        public abstract int GetValidArgsCount();
    }

}