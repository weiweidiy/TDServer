namespace JFramework
{
    /// <summary>
    /// 可激活接口
    /// </summary>
    public interface IActivable
    {
        /// <summary>
        /// 设置是否激活
        /// </summary>
        /// <param name="active"></param>
        void SetActive(bool active);

        /// <summary>
        /// 激活状态
        /// </summary>
        bool Active { get; }
    }


}