using JFramework.BattleReportSystem;
using System;
using System.Collections.Generic;

namespace JFramework
{

    public abstract class BaseCombatBuffer : ICombatUpdatable, IUnique, IActionOwner, ICombatAttachable<CombatUnit>, IUpdateable//, IUpdate<BaseCombatBuffer>
    {
        public event Action<CombatExtraData> onBufferExecuting;
        protected void NotifyBufferExecuting(CombatExtraData extraData)
        {
            onBufferExecuting?.Invoke(extraData);
        }

        public virtual string Uid { get; set; }

        public virtual int Id { get; set; }

        /// <summary>
        /// 是否过期
        /// </summary>
        public virtual bool Expired { get; set; }

        public virtual CombatBufferFoldType FoldType { get; set; }

        /// <summary>
        /// buffer类型
        /// </summary>
        public virtual CombatBufferType BufferType { get; set; }

        /// <summary>
        /// 最大叠加层数
        /// </summary>
        public int MaxFoldCount { get; set; }

        /// <summary>
        /// 透传数据，其中caster
        /// </summary>
        protected CombatExtraData _extraData;
        public virtual CombatExtraData ExtraData
        {
            get => _extraData; set
            {
                _extraData = value;
            }
        }

        public void SetBufferOwner(CombatUnit owner)
        {
            _extraData.Owner = owner;
        }

        public CombatUnit Owner { get; private set; }


        public abstract void Update(CombatFrame frame);



        public abstract void SetDuration(float duration);
        public abstract float GetDuration();

        /// <summary>
        /// 当前层数
        /// </summary>
        public abstract void SetCurFoldCount(int foldCount);
        public abstract int GetCurFoldCount();

        public virtual void OnAttach(CombatUnit target)
        {
            _extraData.Owner = target;
            Owner = target;
        }

        public virtual void OnDetach() => Owner = null;



        #region action接口
        /// <summary>
        /// 添加action
        /// </summary>
        /// <param name="action"></param>
        public abstract void AddAction(CombatAction action);

        /// <summary>
        /// 移除action
        /// </summary>
        /// <param name="action"></param>
        public abstract void RemoveAction(CombatAction action);

        /// <summary>
        /// 更新action
        /// </summary>
        /// <param name="action"></param>
        public abstract void UpdateAction(CombatAction action);

        /// <summary>
        /// 獲取所有action
        /// </summary>
        /// <returns></returns>
        public abstract List<CombatAction> GetActions();

        public abstract void Update(IUpdateable value);

        #endregion
    }

}