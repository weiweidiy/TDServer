using System;
using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// 觸發器基類
    /// </summary>
    public abstract class CombatBaseTrigger : BaseActionComponent, ICombatTrigger, IActionOwner
    {
        /// <summary>
        /// 透傳參數
        /// </summary>
        protected CombatExtraData _extraData;
        public CombatExtraData ExtraData
        {
            get => _extraData; 
            set
            {
                _extraData = value;
            }
        }
     

        bool isOn;

        /// <summary>
        /// 查找器
        /// </summary>
        protected List<CombatBaseFinder> finders;

        public CombatBaseTrigger(List<CombatBaseFinder> finders)
        {
            this.finders = finders;
        }

        /// <summary>
        /// 查詢是否觸發
        /// </summary>
        /// <returns></returns>
        public bool IsOn()
        {
            return isOn;
        }

        /// <summary>
        /// 重置為未觸發
        /// </summary>
        public virtual void Reset()
        {
            SetOn(false);
        }

        /// <summary>
        /// 設置觸發狀態
        /// </summary>
        /// <param name="on"></param>
        public void SetOn(bool on)
        {
            isOn = on;
        }

        /// <summary>
        /// 設置透傳參數的源單位
        /// </summary>
        /// <param name="target"></param>
        public override void OnAttach(CombatAction target)
        {
            base.OnAttach(target);
            if(finders != null)
            {
                foreach(var finder in finders)
                {
                    finder.OnAttach(target);
                }
            }
        }

        protected override void OnUpdate(CombatFrame frame)
        {
        }

        protected virtual List<CombatUnit> Filter(List<CombatUnit> list)
        {
            for(int i = list.Count - 1; i >= 0; i--)
            {
                var unit = list[i];
                if (ExtraData != null && ExtraData.Owner != null && !ExtraData.Owner.IsMainTarget() && !unit.IsMainTarget() && unit.GetParentInfo().uid != ExtraData.Owner.GetParentInfo().uid) //如果自己是子单位，则过滤掉不是父单位的单位
                    list.RemoveAt(i);
            }

            return list;
        }
    }
}