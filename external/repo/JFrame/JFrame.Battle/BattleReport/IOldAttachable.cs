namespace JFramework
{
    public interface IOldAttachable
    {
        IAttachOwner Owner { get; }

        void OnAttach(IAttachOwner target);

        void OnDetach();  
    }

    public interface IAttachOwner
    {
       string Name { get; }
        
        int Id { get; }

        IBattleUnit Owner { get; }

        /// <summary>
        /// 获取层数
        /// </summary>
        /// <returns></returns>
        float GetFoldCount();

        /// <summary>
        /// 获取周期
        /// </summary>
        /// <returns></returns>
        float GetDuration();

        /// <summary>
        /// 设置是否有效
        /// </summary>
        /// <param name="valid"></param>
        void SetValid(bool valid);

        /// <summary>
        /// 修改属性
        /// </summary>
        /// <param name="args"></param>
        void SetConditionTriggerArgs(float[] args);
        void SetFinderArgs(float[] args);
        void SetExecutorArgs(float[] args);
        void SetCdArgs(float[] args);


    }

}

