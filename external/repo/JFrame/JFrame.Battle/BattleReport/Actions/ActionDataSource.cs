using System.Collections.Generic;

namespace JFramework
{
    public class ActionDataSource
    {
        public PVPBattleManager pvpManager { get; set; }

        public ActionDataSource(PVPBattleManager pvpManager)
        {
            this.pvpManager = pvpManager;
        }



        /// <summary>
        /// 获取触发器类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int GetConditionTriggerType(string unitUID, int unitId, int actionId)
        {
            return 1; //CDTrigger
        }

        /// <summary>
        /// 获取触发器参数
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual float[] GetConditionTriggerArg(string unitUID, int unitId, int actionId)
        {
            return new float[] { 3f }; //to do: 计算数值
        }

        /// <summary>
        /// 搜索器类型
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual int GetFinderType(string unitUID, int unitId, int actionId)
        {
            return 1; //normaltargetfinder
        }

        /// <summary>
        /// 搜索器参数
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual float GetFinderArg(string unitUID, int unitId, int actionId)
        {
            return 1f;
        }

        /// <summary>
        /// 获取执行器
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual List<int> GetExcutorTypes(string unitUID, int unitId, int actionId)
        {
            return new List<int>() { 1 };
        }

        /// <summary>
        /// 执行器参数
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual float[] GetExcutorArg(string unitUID, int unitId, int actionId, int executorIndex)
        {
            return new float[] { 1f , 0.5f,0.25f, 1f };//1:次数, 2：延迟 3:多段攻击间隔 4:倍率：
        }

        /// <summary>
        /// 获取触发器类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int GetCDTriggerType(string unitUID, int unitId, int actionId)
        {
            return 1; //CDTrigger
        }

        /// <summary>
        /// 获取触发器参数
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual float[] GetCDTriggerArg(string unitUID, int unitId, int actionId)
        {
            return new float[] { 3f }; //to do: 计算数值
        }

        public virtual float GetDuration(string unitUID, int unitId, int actionId)
        {
            return 1f;
        }

        /// <summary>
        /// 技能类型：区分普通和技能
        /// </summary>
        /// <param name="unitUID"></param>
        /// <param name="unitId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual int GetType(string unitUID, int unitId, int actionId)
        {
            return 1;
        }

        /// <summary>
        /// 1：主动技能 2： 被动技能
        /// </summary>
        /// <param name="unitUID"></param>
        /// <param name="unitId"></param>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual ActionMode GetActionMode(string unitUID, int unitId, int actionId)
        {
            return ActionMode.Active;
        }
    }
}