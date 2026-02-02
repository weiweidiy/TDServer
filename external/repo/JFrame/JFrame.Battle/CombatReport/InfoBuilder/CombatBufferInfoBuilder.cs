namespace JFramework
{
    public class CombatBufferInfoBuilder : CombatActionInfoBuilder<CombatBufferInfo>
    {
        public CombatBufferDataSource DataSource { get; set; }

        public CombatBufferInfoBuilder(CombatActionArgSourceBuilder actionArgBuilder) : base(actionArgBuilder)
        {
        }

        /// <summary>
        /// 构建gjj战斗数据
        /// </summary>
        /// <param name="hp"></param>
        /// <param name="maxHp"></param>
        /// <param name="atk"></param>
        /// <param name="atkSpeed"></param>
        /// <param name="position"></param>
        /// <param name="actionsData"></param>
        /// <returns></returns>
        public override CombatBufferInfo Build()
        {
            var bufferInfo = new CombatBufferInfo();
            bufferInfo.id = DataSource.GetId();
            bufferInfo.foldType = DataSource.GetBufferFoldType();
            bufferInfo.foldMaxCount = DataSource.GetMaxFoldCount();
            bufferInfo.Uid = DataSource.GetUid();
            bufferInfo.bufferType = DataSource.GetBufferType();
            var actionIds = DataSource.GetActions();
            var actionsData = CreateActions(actionIds);
            bufferInfo.actionsData = actionsData;
            return bufferInfo;
        }

       

    }
}