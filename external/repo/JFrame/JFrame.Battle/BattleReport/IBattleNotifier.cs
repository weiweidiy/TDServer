using System;

namespace JFramework
{
    /// <summary>
    /// 战斗通知器
    /// </summary>
    public interface IBattleNotifier
    {
        /// <summary>
        /// 事件委托
        /// </summary>
        event Action<string, object> onRaiseEvent;
    }
}