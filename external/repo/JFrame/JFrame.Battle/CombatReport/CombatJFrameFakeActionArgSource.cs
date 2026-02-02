using System.Collections.Generic;

namespace JFramework
{

    public class CombatJFrameFakeActionArgSource : CombatActionArgSource
    {
        public CombatJFrameFakeActionArgSource(int actionId) { }
        /// <summary>
        /// 获取action类型：0：普通，1：技能
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public override ActionType GetActionType()
        {
            return ActionType.Normal;
        }

        /// <summary>
        /// 获取action模式 0：主动 1：被动
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public override ActionMode GetActionMode()
        {
            return ActionMode.Active;
        }


        public override int[] GetConditionFindersId()
        {
            return new int[] { 0 };
        }

        public override float[] GetConditionFindersArgs(int index)
        {
            return new float[] { };
        }


        /// <summary>
        /// 获取所有条件触发器id列表
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public override int[] GetConditionTriggersId()
        {
 
            return new int[] { 1};
        }

        /// <summary>
        /// 获取参数列表
        /// </summary>
        /// <param name="aciontId"></param>
        /// <param name="triggerIndex"></param>
        /// <returns></returns>
        public override float[] GetConditionTriggersArgs(int triggerIndex)
        {
            var result = new List<float>();
            result.Add(1f);
            result.Add(1f);
            result.Add(3f);
            return result.ToArray();
        }

        /// <summary>
        /// 延迟生效触发器
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        public override int GetDelayTriggerId()
        {
            return 3; //时间触发器
        }

        public override float[] GetDelayTriggerArgs()
        {
            var result = new List<float>();
            result.Add(0.1f);
            return result.ToArray();
        }

        /// <summary>
        /// 获取查找器id
        /// </summary>
        /// <returns></returns>
        public override int[] GetFindersId()
        {
            return new int[] { };
        }

        /// <summary>
        /// 获取查找器参数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float[] GetFindersArgs(int index)
        {
            var result = new List<float>();

            return result.ToArray();
        }

        /// <summary>
        /// 获取执行器id列表
        /// </summary>
        /// <returns></returns>
        public override int[] GetExecutorsId()
        {

            return new int[] {1 };
        }

        /// <summary>
        /// 获取执行器参数列表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float[] GetExecutorsArgs(int index)
        {
            var result = new List<float>();
            result.Add(0.1f);
            result.Add(1f);
            return result.ToArray();
        }

        /// <summary>
        /// 获取cd触发器id列表
        /// </summary>
        /// <returns></returns>
        public override int[] GetCdTriggersId()
        {

            return new int[] { 3};
        }

        /// <summary>
        /// 获取cd触发器参数列表
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override float[] GetCdTriggersArgs(int index)
        {
            var result = new List<float>();
            result.Add(0.1f);
            return result.ToArray();
        }

        public override int GetFormulaId()
        {
            throw new System.NotImplementedException();
        }

        public override float[] GetFormulaArgs()
        {
            throw new System.NotImplementedException();
        }

        public override int GetActionGroupId()
        {
            throw new System.NotImplementedException();
        }

        public override int GetActionSortId()
        {
            throw new System.NotImplementedException();
        }

        public override float GetBulletSpeed()
        {
            throw new System.NotImplementedException();
        }
    }
}