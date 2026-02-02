namespace JFramework
{


    /// <summary>
    /// 参数化接口
    /// </summary>
    public interface IArgsable
    {
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="args"></param>
        void SetCurArgs(float[] args);

        /// <summary>
        /// 设置单个参数
        /// </summary>
        /// <param name="index"></param>
        /// <param name="arg"></param>
        void SetCurArg(int index, float arg);

        /// <summary>
        /// 获取指定位参数
        /// </summary>
        /// <param name="index"></param>
        float GetCurArg(int index);

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns></returns>
        float[] GetCurArgs();

        /// <summary>
        /// 获取原始参数
        /// </summary>
        /// <returns></returns>
        float[] GetOriginArgs();

        /// <summary>
        /// 重置参数到原始值
        /// </summary>
        void ResetArgs();

        /// <summary>
        /// 获取有效的参数个数（用于检查配置）
        /// </summary>
        /// <returns></returns>
        int GetValidArgsCount();
    }


}