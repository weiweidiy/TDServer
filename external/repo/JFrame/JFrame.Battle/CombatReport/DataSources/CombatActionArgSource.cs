using System;
using System.Collections.Generic;

namespace JFramework
{
    public abstract class CombatActionArgSource
    {
        /// <summary>
        /// 獲取子彈飛行速度，如果是0，則使用延遲參數
        /// </summary>
        /// <returns></returns>
        public abstract float GetBulletSpeed();
        /// <summary>
        /// 获取action类型：0：普通，1：技能
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public abstract ActionType GetActionType();

        /// <summary>
        /// 获取action模式 0：主动 1：被动
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public abstract ActionMode GetActionMode();

        /// <summary>
        /// 獲取action組id
        /// </summary>
        /// <returns></returns>
        public abstract int GetActionGroupId();

        /// <summary>
        /// 獲取技能排序id
        /// </summary>
        /// <returns></returns>
        public abstract int GetActionSortId();

        /// <summary>
        /// 获取条件查找器
        /// </summary>
        /// <returns></returns>
        public abstract int[] GetConditionFindersId();

        /// <summary>
        /// 获取条件查找器参数
        /// </summary>
        /// <returns></returns>
        public abstract float[] GetConditionFindersArgs(int index);

        /// <summary>
        /// 获取所有条件触发器id列表
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public abstract int[] GetConditionTriggersId();

        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <param name="aciontId"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract float[] GetConditionTriggersArgs(int index);

        /// <summary>
        /// 延迟生效触发器
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public abstract int GetDelayTriggerId();

        /// <summary>
        /// 获取延迟触发器参数列表
        /// </summary>
        /// <returns></returns>
        public abstract float[] GetDelayTriggerArgs();

        /// <summary>
        /// 获取查找器id
        /// </summary>
        /// <returns></returns>
        public abstract int[] GetFindersId();

        /// <summary>
        /// 获取查找器参数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract float[] GetFindersArgs(int index);

        /// <summary>
        /// 公式计算器
        /// </summary>
        /// <returns></returns>
        public abstract int GetFormulaId();

        /// <summary>
        /// 公式计算器参数
        /// </summary>
        /// <returns></returns>
        public abstract float[] GetFormulaArgs();

        /// <summary>
        /// 获取执行器id列表
        /// </summary>
        /// <returns></returns>
        public abstract int[] GetExecutorsId();

        /// <summary>
        /// 获取执行器参数列表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract float[] GetExecutorsArgs(int index);

        /// <summary>
        /// 获取cd触发器id列表
        /// </summary>
        /// <returns></returns>
        public abstract int[] GetCdTriggersId();

        /// <summary>
        /// 获取cd触发器参数列表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public abstract float[] GetCdTriggersArgs(int index);


    }
}