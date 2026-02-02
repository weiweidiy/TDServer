using System;

namespace JFramework
{

    public abstract class ActionState
    {
        /// <summary>
        /// 状态机
        /// </summary>
        public OldActionSM Fsm { get; set; }

        /// <summary>
        /// 子类实现，状态名字
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// 上下文
        /// </summary>
        protected BaseAction context;


        /// <summary>
        /// 状态进入时调用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual void OnEnter(BaseAction context)
        {
            this.context = context;
            //Debug.Log(this.GetType().ToString() + "  onEnter " + GetHashCode());
            AddListeners();
            
        }

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
        public virtual void AddListeners()
        {

        }


        public virtual void RemoveListeners()
        {
  
        }

        /// <summary>
        /// 状态机更新帧
        /// </summary>
        /// <param name="frame"></param>
        public virtual void Update(CombatFrame frame)
        {
            
        }
    }
}