namespace JFramework
{
    /// <summary>
    /// buffer数据源
    /// </summary>
    public abstract class CombatBufferDataSource : CombatActionDataSource
    {
        public abstract string GetUid();
        public abstract int GetId();
        public abstract int GetMaxFoldCount();
        public abstract CombatBufferFoldType GetBufferFoldType();
        public abstract CombatBufferType GetBufferType();
    }
}