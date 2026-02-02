using System;
using System.Collections.Generic;

namespace JFramework
{

    public abstract class BaseBattleTrigger : IBattleTrigger
    {
        public event Action<IBattleTrigger, object[]> onTriggerOn;

        protected void NotifyTriggerOn(IBattleTrigger trigger, object[] arg)
        {
            onTriggerOn?.Invoke(trigger, arg);

        }

        /// <summary>
        /// 拥有者
        /// </summary>
        public IAttachOwner Owner { get; private set; }

        /// <summary>
        /// 触发器类型
        /// </summary>
        public virtual BattleTriggerType TriggerType { get => BattleTriggerType.Normal; }

        /// <summary>
        /// 战斗管理器
        /// </summary>
        protected IPVPBattleManager battleManager;

        /// <summary>
        /// 执行的可变更参数
        /// </summary>
        protected float[] args;

        /// <summary>
        /// 原始参数
        /// </summary>
        public float[] OriginalArgs;

        /// <summary>
        /// 延迟触发
        /// </summary>
        protected float delay;
        /// <summary>
        /// 是否已经延迟过了
        /// </summary>
        protected bool delayed;

        /// <summary>
        /// 临时变量，记录流逝时间
        /// </summary>
        protected float delta;

        /// <summary>
        /// 触发状态
        /// </summary>
        private bool isOn;

        /// <summary>
        /// 是否可用
        /// </summary>
        bool isEnable = true;

        public BaseBattleTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0)
        {
            this.battleManager = battleManager;
            this.args = args;
            this.delay = delay;
            this.delayed = delay == 0f; //如果延迟为0，视为已经延迟过了
            this.isOn = false;
            OriginalArgs = new float[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                OriginalArgs[i] = args[i];
            }
        }



        /// <summary>
        /// 更新帧
        /// </summary>
        /// <param name="frame"></param>
        public virtual void Update(CombatFrame frame)
        {
            if (!GetEnable())
                return;

            delta += frame.DeltaTime;

            if (!delayed)
            {
                if (delta - delay > 0f)
                {
                    delta -= delay;
                    delayed = true;
                }
                return;
            }

            OnDelayCompleteEveryFrame(frame);
        }

        protected virtual void OnDelayCompleteEveryFrame(CombatFrame frame) { }


        /// <summary>
        /// 影响是否触发，如果设置为false , 则不会触发
        /// </summary>
        /// <param name="isOn"></param>
        public void SetEnable(bool isOn) => this.isEnable = isOn;
        public virtual bool GetEnable() => isEnable;



        /// <summary>
        /// 重新启动
        /// </summary>
        public virtual void Restart()
        {
            delta = 0f;
            isOn = false;
        }

        /// <summary>
        /// 是否生效
        /// </summary>
        /// <returns></returns>
        public bool IsOn()
        {
            return isOn;
        }

        /// <summary>
        /// 设置无效
        /// </summary>
        public void SetOn(bool isOn)
        {
            this.isOn = isOn;
        }

        /// <summary>
        /// 获取CD
        /// </summary>
        /// <returns></returns>
        public float[] GetArgs()
        {
            return args;
        }

        /// <summary>
        /// 设置cd
        /// </summary>
        /// <param name="cd"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void SetArgs(float[] args)
        {
            if (args.Length != this.args.Length)
                throw new Exception("SetArgs时参数列表长度不一致 " + this.GetType().Name);

            for (int i = 0; i < args.Length; i++)
            {
                this.args[i] = args[i];
            }

        }

        public virtual void OnAttach(IAttachOwner target)
        {
            Owner = target;
        }

        public virtual void OnDetach()
        {
            //throw new NotImplementedException();
        }

        public virtual object GetExtraArg()
        {
            return null;
        }

        public float[] GetOriginalArgs()
        {
            return OriginalArgs;
        }

        public virtual void OnUpdate()
        {
            
        }
    }
    //public abstract class BaseBattleTrigger : IBattleTrigger
    //{
    //    //public event Action onTrigger;

    //    /// <summary>
    //    /// 拥有者
    //    /// </summary>
    //    public IBattleAction Owner { get; private set; }

    //    /// <summary>
    //    /// 触发器类型
    //    /// </summary>
    //    public virtual BattleTriggerType TriggerType { get => BattleTriggerType.Normal; }

    //    /// <summary>
    //    /// 战斗管理器
    //    /// </summary>
    //    protected IPVPBattleManager battleManager;

    //    /// <summary>
    //    /// 参数
    //    /// </summary>
    //    protected float[] args;

    //    /// <summary>
    //    /// 延迟触发
    //    /// </summary>
    //    protected float delay;
    //    /// <summary>
    //    /// 是否已经延迟过了
    //    /// </summary>
    //    protected bool delayed;

    //    /// <summary>
    //    /// 临时变量，记录流逝时间
    //    /// </summary>
    //    protected float delta;

    //    /// <summary>
    //    /// 触发状态
    //    /// </summary>
    //    bool isOn;

    //    /// <summary>
    //    /// 是否可用
    //    /// </summary>
    //    bool isEnable = true;


    //    public BaseBattleTrigger(IPVPBattleManager battleManager, float[] args, float delay = 0)
    //    {
    //        this.battleManager = battleManager;
    //        this.args = args;
    //        this.delay = delay;
    //        this.delayed = delay == 0f; //如果延迟为0，视为已经延迟过了
    //        this.isOn = false;
    //    }



    //    /// <summary>
    //    /// 更新帧
    //    /// </summary>
    //    /// <param name="frame"></param>
    //    public virtual void Update(BattleFrame frame)
    //    {
    //        if (!GetEnable())
    //            return;

    //        delta += frame.DeltaTime;

    //        if (!delayed)
    //        {
    //            if (delta - delay > 0f)
    //            {
    //                delta -= delay;
    //                delayed = true;
    //            }
    //            return;
    //        }

    //        OnDelayCompleteEveryFrame();
    //    }

    //    protected virtual void OnDelayCompleteEveryFrame() { }


    //    /// <summary>
    //    /// 影响是否触发，如果设置为false , 则不会触发
    //    /// </summary>
    //    /// <param name="isOn"></param>
    //    public void SetEnable(bool isOn) => this.isEnable = isOn;
    //    public virtual bool GetEnable() => isEnable;

    //    //public void NotifyOnTrigger()
    //    //{
    //    //    if (GetEnable())
    //    //    {
    //    //        isValid = true;
    //    //        //onTrigger?.Invoke();
    //    //    }

    //    //}

    //    public virtual void OnAttach(IBattleAction action)
    //    {
    //        Owner = action;
    //    }

    //    /// <summary>
    //    /// 重新启动
    //    /// </summary>
    //    public virtual void Restart()
    //    {
    //        //SetEnable(true);
    //        delta = 0f;
    //        isOn = false;
    //    }

    //    /// <summary>
    //    /// 是否生效
    //    /// </summary>
    //    /// <returns></returns>
    //    public bool IsOn()
    //    {
    //        return isOn;
    //    }

    //    /// <summary>
    //    /// 设置无效
    //    /// </summary>
    //    public void SetOn(bool isOn)
    //    {
    //        this.isOn = isOn;
    //    }

    //    /// <summary>
    //    /// 获取CD
    //    /// </summary>
    //    /// <returns></returns>
    //    public float[] GetArgs()
    //    {
    //        return args;
    //    }

    //    /// <summary>
    //    /// 设置cd
    //    /// </summary>
    //    /// <param name="cd"></param>
    //    /// <exception cref="NotImplementedException"></exception>
    //    public void SetArgs(float[] args)
    //    {
    //        this.args = args;
    //    }
    //}
}