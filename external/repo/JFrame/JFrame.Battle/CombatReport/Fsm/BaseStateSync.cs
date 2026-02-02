
using System;
using System.Diagnostics;

namespace JFramework
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseStateSync<TContext>
    {
        /// <summary>
        /// 上下文
        /// </summary>
        protected TContext context;

        /// <summary>
        /// 状态机名字
        /// </summary>
        public string Name => GetType().Name;

        /// <summary>
        /// 状态进入时调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void OnEnter(TContext context)
        {
            this.context = context;
  
            OnEnter(); 
        }

        /// <summary>
        /// 子类实现
        /// </summary>
        /// <returns></returns>
        protected virtual void OnEnter() { AddListeners(); }

        /// <summary>
        /// 状态退出时调用
        /// </summary>
        /// <returns></returns>
        public virtual void OnExit()
        {
            RemoveListeners();
        }

        /// <summary>
        /// 事件监听器，在状态进入时调用，子类重写
        /// </summary>
        public virtual void AddListeners() { }
        public virtual void RemoveListeners() { }
    }
}
