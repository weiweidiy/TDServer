using System;
using System.Collections.Generic;

namespace JFramework
{
    public class BufferDataSource
    {
        public virtual bool IsBuff(int buffId)
        {
            return true;
        }



        public virtual BufferFoldType GetFoldType(int buffId)
        {
            return BufferFoldType.Fold;
        }

        public virtual float[] GetArgs(int buffId)
        {
            return new float[] { GetDuration(buffId),  10f };
        }

        public virtual float GetDuration(int buffId)
        {
            return 10;
        }

        public virtual int GetMaxValue(int buffId)
        {
            return 10;
        }

        public BufferTriggerType GetTriigerType(int bufferId)
        {
            return BufferTriggerType.OnDamage;
        }




        /// <summary>
        /// 获取触发器类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual int GetConditionTriggerType(int bufferId)
        {
            return 1; //CDTrigger
        }

        /// <summary>
        /// 获取触发器参数
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual float[] GetConditionTriggerArg(int bufferId)
        {
            return new float[] { 3f }; //to do: 计算数值
        }

        /// <summary>
        /// 搜索器类型
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual int GetFinderType(int bufferId)
        {
            return 1; //normaltargetfinder
        }

        /// <summary>
        /// 搜索器参数
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual float GetFinderArg(int bufferId)
        {
            return 1f;
        }

        /// <summary>
        /// 获取执行器
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual List<int> GetExcutorTypes(int bufferId)
        {
            return new List<int>() { 1 };
        }

        /// <summary>
        /// 执行器参数
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public virtual float[] GetExcutorArg(int bufferId, int executorIndex)
        {
            return new float[] { 1f, 0.5f, 0.25f, 1f };//1:次数, 2：延迟 3:多段攻击间隔 4:倍率：
        }

        public virtual int GetBuffType(int buffId)
        {
            return 1;
        }
    }
}