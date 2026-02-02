
using Stateless;
using System;
using System.Collections.Generic;


namespace JFramework
{
    /// <summary>
    /// 游戏状态机-> Login , Game 等等 
    /// </summary>
    public abstract class BaseSMSync<TContext, TState, TTrigger> where TState : BaseStateSync<TContext>
    {

        public class SMConfig
        {
            public TState state;
            public Dictionary<TTrigger, TState> dicPermit;
        }

        /// <summary>
        /// 状态机
        /// </summary>
        protected StateMachine<TState, TTrigger> machine;

        /// <summary>
        /// 
        /// </summary>
        protected TContext context;

        /// <summary>
        /// 所有状态列表
        /// </summary>
        List<TState> states;

        /// <summary>
        /// 初始化状态机
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="Exception"></exception>
        public void Initialize(TContext context)
        {
            this.context = context;

            //从子类获取所有状态实例
            states = GetAllStates();

            if (states == null || states.Count == 0)
                throw new Exception("状态机状态列表为空 " + GetType().ToString());

            //Debug.Log("before StateMachine create " );

            machine = new StateMachine<TState, TTrigger>(states[0]);

            //Debug.Log("StateMachine success " + machine.ToString()); 

            //获取所有状态配置
            var configs = GetConfigs();
            var keys = configs.Keys;
            foreach (var key in keys) {
                var config = configs[key];
                var state = config.state;

                //配置状态
                var cfg = machine.Configure(state);
                cfg = cfg.OnEntry( () => {  OnEnter(state); });
                cfg = cfg.OnExit( () => {  OnExit(state); });

                var triggerKeys = config.dicPermit.Keys;
                foreach (var triggerKey in triggerKeys)
                {
                    var targetState = config.dicPermit[triggerKey];
                    cfg = cfg.Permit(triggerKey, targetState);
                }
            }
        }

        /// <summary>
        /// 进入状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private void OnEnter(TState state)
        {
            state.OnEnter(context);
        }

        /// <summary>
        /// 离开状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private void OnExit(TState state)
        {
            state.OnExit();
        }

        /// <summary>
        /// 获取当前state
        /// </summary>
        /// <returns></returns>
        public TState GetCurState()
        {
            return machine.State;
        }

        /// <summary>
        /// 获取所有状态配置
        /// </summary>
        /// <returns></returns>
        protected abstract Dictionary<string, SMConfig> GetConfigs();

        /// <summary>
        /// 获取所有状态
        /// </summary>
        /// <returns></returns>
        protected abstract List<TState> GetAllStates();

    }
}
