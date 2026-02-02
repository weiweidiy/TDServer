namespace Game.Share
{
    public class RequestDrawSamurai
    {
        /// <summary>
        /// 抽卡池子
        /// </summary>
        public DrawSamuraiPoolType DrawPoolType { get; set; }
        public int Count { get; set; }
    }
}