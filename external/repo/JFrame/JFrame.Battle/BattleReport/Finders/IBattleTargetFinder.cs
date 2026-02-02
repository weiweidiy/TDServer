using System.Collections.Generic;

namespace JFramework
{
    public interface IBattleTargetFinder : IOldAttachable
    {
        List<IBattleUnit> FindTargets(object[] args);

        /// <summary>
        /// 获取CD
        /// </summary>
        /// <returns></returns>
        float[] GetArgs();

        /// <summary>
        /// 设置cd
        /// </summary>
        /// <param name="cd"></param>
        /// <exception cref="NotImplementedException"></exception>
        void SetArgs(float[] args);
    }
}

//public interface IBattleTargetFinder
//{
//    IBattleAction Owner { get; }

//    List<IBattleUnit> FindTargets();

//    void OnAttach(IBattleAction action);
//}