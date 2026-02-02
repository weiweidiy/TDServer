using JFramework.BattleReportSystem;
using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 管理所有action to do:可以增加线程组概念，允许某些技能使用一个线程组
    /// </summary>
    public class CombatActionManager : UpdateableContainer<CombatAction>, ICombatUpdatable 
    {
        public event Action<CombatExtraData> onTriggerOn; //满足触发条件
        public event Action<CombatExtraData> onStartExecuting; //开始释放
        public event Action<CombatExtraData> onStartCD;
        public event Action<CombatExtraData> onHittingTargets; //动作命中之前（单次所有目标）
        public event Action<CombatExtraData> onTargetsHittedComplete; //动作命中之后（所有目标）
        public event Action<CombatExtraData> onHittingTarget; //动作命中之前（单目标）
        public event Action<CombatExtraData> onTargetHittedComplete; //动作命中之后（单目标）
        public event Action<CombatExtraData> onCdChanged;

        bool isBusy;
        float curDuration = 0f;
        float deltaTime = 0f;

        public void Initialize(IActionOwner actionOwner)
        {
            //Release();

            foreach (var action in GetAll())
            {
                action.onTriggerOn += Action_onTriggerOn;
                action.onStartExecuting += Action_onStartExecuting;
                action.onStartCD += Action_onStartCD;
                action.onHittingTargets += Action_onHittingTargets;
                action.onTargetsHittedComplete += Action_onTargetsHittedComplete;
                action.onHittingTarget += Action_onHittingTarget;
                action.onTargetHittedComplete += Action_onTargetHittedComplete;
                action.onCDChanged += Action_onCDChanged;

                //把unit上的extraData 传递给 action
                action.ExtraData = actionOwner.ExtraData.Clone() as CombatExtraData;
                //action.ExtraData.Owner = actionOwner;
            }
        }



        public void AddActions(List<CombatAction> actions)
        {
            AddRange(actions);

            foreach(var action in actions)
            {
                action.ActionManager = this;
            }
        }

        public void Release()
        {
            Clear();
        }


        public void SetExtraData(CombatExtraData extraData)
        {
            foreach(var action in GetAll())
            {
                action.ExtraData = extraData.Clone() as CombatExtraData;
            }
        }


        public void Start()
        {
            foreach (var action in GetAll())
            {
                action.Start();
                //切换到不可用状态
                action.SwitchToDisable();
                action.SwitchToReady();

                //action.SwitchToTrigging();
            }
        }

        public void Stop()
        {
            foreach (var action in GetAll())
            {
                action.Stop();
                //切换到不可用状态
                action.SwitchToDisable();
            }
        }


        //public void SetAllActive(bool active)
        //{
        //    if (active)
        //        Start();
        //    else
        //        Stop();
        //    //foreach (var action in GetAll())
        //    //{
        //    //    if (active)
        //    //        action.SwitchToCd();
        //    //    else
        //    //        action.SwitchToDisable();
        //    //}
        //}

        /// <summary>
        /// 逻辑帧
        /// </summary>
        /// <param name="frame"></param>
        public void Update(CombatFrame frame)
        {
            
            foreach (var action in GetAll())
            {
                action.Update(frame);
            }

            UpdateDuration(frame);

            //更新等待添加删除更新的item
            UpdateWaitingItems();
        }

  

        /// <summary>
        /// 更新释放时间
        /// </summary>
        /// <param name="frame"></param>
        public void UpdateDuration(CombatFrame frame)
        {
            if (isBusy)
            {
                deltaTime += frame.DeltaTime;

                if (deltaTime >= curDuration)
                {
                    isBusy = false;
                    deltaTime = 0f;
                }
            }
        }


        #region action事件
        private void Action_onTargetHittedComplete(CombatExtraData extraData)
        {
            onTargetHittedComplete?.Invoke(extraData);
        }

        private void Action_onHittingTarget(CombatExtraData extraData)
        {
            onHittingTarget?.Invoke(extraData);
        }

        private void Action_onTargetsHittedComplete(CombatExtraData extraData)
        {
            onTargetsHittedComplete?.Invoke(extraData);
        }

        private void Action_onHittingTargets(CombatExtraData extraData)
        {
            onHittingTargets?.Invoke(extraData);
        }

        private void Action_onStartCD(CombatExtraData extraData)
        {
            onStartCD?.Invoke(extraData);
        }
        private void Action_onCDChanged(CombatExtraData extraData)
        {
            onCdChanged?.Invoke(extraData);
        }

        /// <summary>
        /// 满足触发条件了
        /// </summary>
        /// <param name="extraData"></param>
        private void Action_onTriggerOn(CombatExtraData extraData)
        {
            if (extraData.Action.Mode == ActionMode.Active)
                onTriggerOn?.Invoke(extraData);

            if (extraData.Action.Mode == ActionMode.Passive)
            {
                extraData.Action.SwitchToExecuting();
                return;
            }
                

            if (!isBusy && extraData.Action.Mode == ActionMode.Active)
            {
                curDuration = extraData.Action.SwitchToExecuting();
                isBusy = true;
                return;
            }

        }

        /// <summary>
        /// 开始释放了
        /// </summary>
        /// <param name="extraData"></param>
        private void Action_onStartExecuting(CombatExtraData extraData)
        {
            onStartExecuting?.Invoke(extraData);
        }
        #endregion

    }

}