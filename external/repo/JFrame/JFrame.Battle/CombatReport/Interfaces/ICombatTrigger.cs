using System;

namespace JFramework
{
    /// <summary>
    /// 触发器
    /// </summary>
    public interface ICombatTrigger
    {
        /// <summary>
        /// 是否是觸發狀態
        /// </summary>
        /// <returns></returns>
        bool IsOn();

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();

        /// <summary>
        /// 設置觸發狀態
        /// </summary>
        /// <param name="on"></param>
        void SetOn(bool on);

        /// <summary>
        /// 透傳參數
        /// </summary>
        CombatExtraData ExtraData { get; }
    }


}