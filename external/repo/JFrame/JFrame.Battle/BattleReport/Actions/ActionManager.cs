using System.Collections.Generic;
using System;
using System.Diagnostics;
using JFramework.BattleReportSystem;


namespace JFramework
{
    public enum ActionType
    {
        All = 0,
        Normal,
        Skill,
    }

    /// <summary>
    /// 动作模式，Active, Passive
    /// </summary>
    public enum ActionMode
    {
        Active = 1, //会占用施放线程
        Passive, //条件满足立即触发
    }

    /// <summary>
    /// 动作管理器
    /// </summary>
    public class ActionManager : ListContainer<IBattleAction>, IActionManager
    {
        public event Action<IBattleAction, List<IBattleUnit>, float> onStartCast;

        public event Action<List<IBattleAction>, float> onInterrupt;

        public event Action<IBattleAction, float> onStartCD;

        /// <summary>
        /// 即将命中目标
        /// </summary>
        public event Action<IBattleAction, IBattleUnit, ExecuteInfo> onHittingTarget;


        public bool IsBusy { get; private set; }

        float curDuration = 0f;

        float deltaTime = 0f;

        IBattleUnit owner;

        public void Initialize(IBattleUnit owner)
        {
            this.owner = owner;

            var actions = GetAll();
            if (actions != null)
            {
                foreach (var action in actions)
                {
                    action.OnAttach(owner);
                }
            }

        }

        /// <summary>
        /// 添加到管理器
        /// </summary>
        /// <param name="member"></param>
        public override void Add(IBattleAction member)
        {
            base.Add(member);

            member.onCanCast += Member_onCanCast;
            member.onStartCast += Member_onStartCast;
            member.onStartCD += Member_onStartCD;
            member.onHittingTarget += Member_onHittingTarget;
        }




        /// <summary>
        /// 从管理器移除
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public override bool Remove(string uid)
        {
            var member = base.Get(uid);
            if (member != null)
            {
                member.onCanCast -= Member_onCanCast;
                member.onStartCast -= Member_onStartCast;
                //member.onCdComplete -= Member_onCdComplete;
                return base.Remove(uid);
            }
            else
                return false;
        }

        /// <summary>
        /// 更新帧
        /// </summary>
        /// <param name="frame"></param>
        public void Update(CombatFrame frame)
        {
            var actions = GetAll();
            foreach (var action in actions)
            {
                action.Update(frame);
            }

            UpdateDuration(frame);
        }

        /// <summary>
        /// 更新释放时间
        /// </summary>
        /// <param name="frame"></param>
        public void UpdateDuration(CombatFrame frame)
        {
            if (IsBusy)
            {
                deltaTime += frame.DeltaTime;

                if (deltaTime >= curDuration)
                {
                    IsBusy = false;
                    deltaTime = 0f;
                }
            }
        }


        /// <summary>
        /// 技能能释放了(每一帧会一直调用）
        /// </summary>
        /// <param name="action"></param>
        private void Member_onCanCast(IBattleAction action)
        {
            //如果正在释放其他主动技能，则返回，否则立即释放
            if (!IsBusy && action.Mode == ActionMode.Active)
            {
                curDuration = action.Cast();
                IsBusy = true;
                return;
            }
            
            ////被动技能直接释放
            //if(action.Mode == ActionMode.Passive)
            //{
            //    action.Cast();
            //    return;
            //}
        }

        /// <summary>
        /// 技能已经释放了
        /// </summary>
        /// <param name="action"></param>
        /// <param name="targets"></param>
        private void Member_onStartCast(IBattleAction action, List<IBattleUnit> targets, float duration)
        {
            onStartCast?.Invoke(action, targets, duration);
        }

        /// <summary>
        /// 进入CD了
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void Member_onStartCD(IBattleAction action, float cd)
        {
            onStartCD?.Invoke(action,cd);
        }

        /// <summary>
        /// 即将命中
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        private void Member_onHittingTarget(IBattleAction arg1, IBattleUnit arg2, ExecuteInfo arg3)
        {
            onHittingTarget?.Invoke(arg1, arg2, arg3);
        }


        /// <summary>
        /// 角色死亡了
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OnDead()
        {
            var actions = GetAll();
            if (actions != null)
            {
                foreach (var a in actions)
                {
                    a.SetDead(true);
                }
            }

            IsBusy = false;

            curDuration = 0f;

            deltaTime = 0f;
        }



        /// <summary>
        /// 根据actionType获取action对象
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public IBattleAction GetActionByType(ActionType actionType)
        {
            var actions = GetAll();
            if (actions != null)
            {
                foreach(var a in actions)
                {
                    if(a.Type == actionType)
                        return a;
                }
            }

            return null;
        }

        /// <summary>
        /// 根据actionType获取所有action对象
        /// </summary>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public virtual List<IBattleAction> GetActionsByType(ActionType actionType)
        {
            var result = new List<IBattleAction>();

            var actions = GetAll();
            if (actions != null)
            {
                if (actionType == ActionType.All)
                    return actions;

                foreach (var a in actions)
                {
                    if (a.Type == actionType)
                        result.Add(a);
                }
            }

            return result;
        }

        /// <summary>
        /// 根据动作ID获取动作
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public IBattleAction GetAction(int actionId)
        {
            var actions = GetAll();
            if (actions != null)
            {
                foreach (var a in actions)
                {
                    if (a.Id == actionId)
                        return a;
                }
            }

            return null;
        }

        /// <summary>
        /// 打断技能释放
        /// </summary>
        /// <param name="actions"></param>
        /// <exception cref="NotImplementedException"></exception>
        public List<IBattleAction> Interrupt(List<IBattleAction> actions)
        {
            if (actions == null)
                throw new ArgumentNullException("打断的技能列表不能为null");

            var result = new List<IBattleAction>();

            foreach(var action in actions)
            {
                if(action.IsExecuting())
                {
                    action.Interrupt(); //打断，立即进入冷却
                    result.Add(action);
                }                 
            }

            return result;
        }

        /// <summary>
        /// 眩晕时候调用，打断技能
        /// </summary>
        public void OnStunning(ActionType actionType, float duration)
        {
            var actions = GetActionsByType(actionType);
            var result = Interrupt(actions);

            foreach (var action in actions)
            {
                action.SetEnable(false);
            }

            //to do:通知被打断的技能None·
            onInterrupt?.Invoke(result, duration);
        }

        /// <summary>
        /// 恢复眩晕
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void OnResumeFromStunning(ActionType actionType)
        {
            var actions = GetActionsByType(actionType);
            foreach(var action in actions)
            {
                action.SetEnable(true);
            }
        }

        public void OnSilence()
        {
            throw new NotImplementedException();
        }

        IBattleAction[] IActionManager.GetAll()
        {
            return GetAll().ToArray();
        }
    }
}