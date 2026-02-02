using JFramework.BattleReportSystem;
using JFramework.Common;
using System;
using System.Collections.Generic;

namespace JFramework
{
    public class CombatBuffer : BaseCombatBuffer
    {
        float delta;

        protected float duration;
        int curFoldCount;

        CombatActionManager actionManager = new CombatActionManager();

        public void Initialize(CombatBufferInfo bufferInfo, List<CombatAction> actions, int foldCount)
        {
            FoldType = bufferInfo.foldType;
            BufferType = bufferInfo.bufferType;
            MaxFoldCount = bufferInfo.foldMaxCount;
            Uid = Guid.NewGuid().ToString();
            Id = bufferInfo.id;
            SetCurFoldCount(foldCount);
            if (actions != null)
            {
                actionManager.AddRange(actions);
                actionManager.Initialize(this); //透传数据初始化了
                actionManager.onStartExecuting += ActionManager_onStartExecuting;
            }
        }


        public override void Update(CombatFrame frame)
        {
            delta += frame.DeltaTime;
            if (delta >= duration)
            {
                delta = 0f;
                Expired = true;
            }
            else
            {
                actionManager.Update(frame);
            }
        }


        public override void SetDuration(float duration) => this.duration = duration;
        public override float GetDuration() => duration;

        public override void SetCurFoldCount(int foldCount)
        {
            curFoldCount = foldCount;
            _extraData.FoldCount = curFoldCount;
        }
        public override int GetCurFoldCount() => curFoldCount;

        #region action接口
        /// <summary>
        /// 添加action
        /// </summary>
        /// <param name="action"></param>
        public override void AddAction(CombatAction action) => actionManager.AddItem(action);

        /// <summary>
        /// 移除action
        /// </summary>
        /// <param name="action"></param>
        public override void RemoveAction(CombatAction action) => actionManager.RemoveItem(action);

        /// <summary>
        /// 更新action
        /// </summary>
        /// <param name="action"></param>
        public override void UpdateAction(CombatAction action) => actionManager.UpdateItem(action);

        /// <summary>
        /// 獲取所有action
        /// </summary>
        /// <returns></returns>
        public override List<CombatAction> GetActions() => actionManager.GetAll();


        private void ActionManager_onItemUpdated(CombatAction obj)
        {
            //throw new NotImplementedException();
        }

        private void ActionManager_onItemRemoved(CombatAction obj)
        {
            //throw new NotImplementedException();
        }

        private void ActionManager_onItemAdded(List<CombatAction> obj)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 触发了
        /// </summary>
        /// <param name="extraData"></param>
        private void ActionManager_onTriggerOn(CombatExtraData extraData)
        {

        }

        /// <summary>
        /// action进入cd了
        /// </summary>
        /// <param name="extraData"></param>
        private void ActionManager_onStartCD(CombatExtraData extraData)
        { }

        /// <summary>
        /// action开始释放了
        /// </summary>
        /// <param name="extraData"></param>
        private void ActionManager_onStartExecuting(CombatExtraData extraData)
        {
            NotifyBufferExecuting(extraData);
        }

        public override void OnAttach(CombatUnit target)
        {
            base.OnAttach(target);

            actionManager.SetExtraData(_extraData);
            actionManager.Start();
            delta = 0;
        }

        public override void OnDetach()
        {
            base.OnDetach();

            actionManager.Stop();

        }

        public override void Update(IUpdateable value)
        {
            var combatbuffer = value as CombatBuffer;

            SetCurFoldCount(combatbuffer.GetCurFoldCount());
        }
        #endregion
    }

}