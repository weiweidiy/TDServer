namespace JFramework
{
    /// <summary>
    /// 执行结果信息
    /// </summary>
    public class ExecuteInfo
    {
        public int Value { get; set; }
        public bool IsCri { get; set; }
        public bool IsBlock { get;set; }
        public bool IsGuard { get; set; }

        public bool IsImmunity { get; set; }

        public IBattleUnit Source { get; set; }
    }
}

