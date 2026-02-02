namespace JFramework
{
    public class CombatFrame
    {
        /// <summary>
        /// 当前帧数
        /// </summary>
        public int CurFrame { get; private set; }

        /// <summary>
        /// 每一逻辑帧流逝时间
        /// </summary>
        float _deltaTime = 0.25f; //固定逻辑帧时间
        public virtual float DeltaTime { get => _deltaTime; }

        /// <summary>
        /// 战斗最大时长
        /// </summary>
        float _allTime = 90f; //to do:从配置表读取
        public float AllTime { get => _allTime; set => _allTime = value; }


        public CombatFrame(float limitTime, float deltaTime)
        {
            _deltaTime = deltaTime;
            _allTime = limitTime;
        }

        public CombatFrame() : this(90f, 0.25f) { }

        /// <summary>
        /// 重置当前帧
        /// </summary>
        public void ResetFrame()
        {
            CurFrame = 0;
        }

        /// <summary>
        /// 下一个逻辑帧
        /// </summary>
        public void NextFrame()
        {
            CurFrame++;
        }

        /// <summary>
        /// 获取最大逻辑帧数
        /// </summary>
        /// <returns></returns>
        public int GetMaxFrameCount()
        {
            return (int)(_allTime / _deltaTime) + 1;
        }

        /// <summary>
        /// 获取指定帧所有的流逝时间
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public float GetDeltaTime(int frame)
        {
            return CurFrame * _deltaTime;
        }

        /// <summary>
        /// 是否已达最大帧
        /// </summary>
        /// <returns></returns>
        public bool IsMaxFrame()
        {
            return CurFrame >= GetMaxFrameCount();
        }
    }
}