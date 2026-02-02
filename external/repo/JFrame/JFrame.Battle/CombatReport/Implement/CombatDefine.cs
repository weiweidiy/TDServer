using System.Collections.Generic;
using System.Linq;

namespace JFramework
{
    public enum ActionComponentType
    {
        ReadyCdTrigger,
        ConditionFinder,
        ConditionTrigger,
        DelayTrigger,
        ExecutorFinder,
        ExecuteFormulator,
        Executor,
        CdTrigger
    }

    public enum CombatTeamType
    {
        Combine, //组合 类似gjj+hero 公用gjj属性
        Single,  //独立 每个单位独自计算
        Remix
    }

    public enum UnitMainType
    {

        Gjj = 1 << 0,  //1
        Hero = 1 << 1, //2
        Monster = 1 << 2, //4
        Boss = 1 << 3, //8
        Pet1 = 1 << 4, //16
        Pet2 = 1 << 5, //32 64 128 256 512
    }

    public enum UnitSubType
    {

        Ground = 1 << 10, //1024
        Sky = 1 << 11, //2048
    }

    /// <summary>
    /// action組件：4種類型
    /// </summary>
    public class ActionComponentInfo
    {
        public int id;
        public float[] args;
    }



    /// <summary>
    /// action数据结构
    /// </summary>
    public class ActionInfo
    {
        public string uid;
        public ActionType type;
        public ActionMode mode;
        public int groupId; //技能組id，覺醒后都是同一組
        public int sortId; //技能排序ID
        public float bulletSpeed; //子彈飛行速度
        public Dictionary<ActionComponentType, List<ActionComponentInfo>> componentInfo;
    }

    /// <summary>
    /// unit数据结构
    /// </summary>
    public class CombatUnitInfo
    {
        public string uid;
        public int id;
        public int level;
        public Dictionary<int, ActionInfo> actionsData;
        public Dictionary<int, Dictionary<ActionComponentType, List<ActionComponentInfo>>> buffersData;
        public UnitMainType mainType;
        public UnitSubType unitSubType;
        public long hp;
        public long maxHp;
        public long atk;
        public float bpDamage;
        public float bpDamageAnti;
        public float missileRate;
        public float atkSpeed;//废弃
        public float cri; //暴击率 0~1的值 百分比
        public float criAnti;
        public float criDamage; //暴击伤害加成百分比
        public float controlHit;
        public float controlAnti;
        public float damageAdvance;
        public float damageAnti;
        public float hit;
        public float dodge;
        public float monsterAdd;
        public float bossAdd;
        public float hpRecover;
        public float fightBackCoef;
        public float hpSteal;
        public float elemt;
        public float elemtResist;
        public CombatVector position; //初始坐標點
        public CombatVector moveSpeed; //移動速度，向左就是負數，向右是正數
        public CombatVector targetPosition;//目标点

        public CombatUnitInfo parent;//父亲单位
        public float readyCd; //預置cd


        public bool HasAction(int actionId)
        {
            return actionsData.ContainsKey(actionId);
        }

        public float GetArg(int actionId, ActionComponentType componentType, int componentIndex, int argIndex)
        {
            if (!actionsData.ContainsKey(actionId))
                throw new System.Exception($"没有找到技能参数 {actionId} {componentType} {componentIndex} {argIndex}");

            var actionInfo = actionsData[actionId];
            var components = actionInfo.componentInfo[componentType];
            if (components.Count <= componentIndex)
                throw new System.Exception($"获取技能参数时，componentIndex 越界{componentIndex},实际长度{components.Count}");

            var component = components[componentIndex];
            if (component.args.Length <= argIndex)
                throw new System.Exception($"获取技能参数时，argIndex 越界{argIndex},实际长度{component.args.Length}");

            return component.args[argIndex];
        }

        public bool HasAction(int sortId, ActionComponentType componentType, int componentId)
        {
            var actionInfo = actionsData.Values.Where(i => i.sortId == sortId).FirstOrDefault();
            if (actionInfo == null)
                return false;

            var components = actionInfo.componentInfo[componentType];

            foreach (var component in components)
            {
                if (component.id == componentId)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取指定类型的参数
        /// </summary>
        /// <param name="sortId"></param>
        /// <param name="componentType"></param>
        /// <param name="componentId"></param>
        /// <param name="componentIndex"></param>
        /// <param name="argIndex"></param>
        /// <returns></returns>
        public (int actionId, float arg) GetArg(int sortId, ActionComponentType componentType, int componentId, int componentIndex, int argIndex)
        {
            var dicActionInfo = actionsData.Where(i => i.Value.sortId == sortId).FirstOrDefault();

            var components = dicActionInfo.Value.componentInfo[componentType];
            if (components.Count <= componentIndex)
                throw new System.Exception($"获取技能参数时，componentIndex 越界{componentIndex},实际长度{components.Count}");

            var component = components[componentIndex];

            if (component.id != componentId)
                throw new System.Exception($"获取技能参数时，componentId 不正确");

            if (component.args.Length <= argIndex)
                throw new System.Exception($"获取技能参数时，argIndex 越界{argIndex},实际长度{component.args.Length}");

            return (dicActionInfo.Key, component.args[argIndex]);
        }

        public void SetArg(int actionId, ActionComponentType componentType, int componentIndex, int argIndex, float value)
        {
            if (!actionsData.ContainsKey(actionId))
                throw new System.Exception($"没有找到技能参数 {actionId} {componentType} {componentIndex} {argIndex}");

            var actionInfo = actionsData[actionId];
            var components = actionInfo.componentInfo[componentType];
            if (components.Count <= componentIndex)
                throw new System.Exception($"获取技能参数时，componentIndex 越界{componentIndex},实际长度{components.Count}");

            var component = components[componentIndex];
            if (component.args.Length <= argIndex)
                throw new System.Exception($"获取技能参数时，argIndex 越界{argIndex},实际长度{component.args.Length}");

            component.args[argIndex] = value;
        }
    }

    /// <summary>
    /// buffer数据结构
    /// </summary>
    public class CombatBufferInfo : IUnique , IUpdateable
    {
        //public string uid;
        public int id;
        /// <summary>
        /// 可叠加最大层数
        /// </summary>
        public int foldMaxCount;

        /// <summary>
        /// buffer类型 0=buff  1=debuff
        /// </summary>
        public CombatBufferType bufferType;

        /// <summary>
        /// 当前层数
        /// </summary>
        //public int foldCount;
        public CombatBufferFoldType foldType;
        //public float duration; //持续时间
        public Dictionary<int, ActionInfo> actionsData;

        public string Uid { get; set; }

        public void Update(IUpdateable value)
        {
            throw new System.NotImplementedException();
        }
    }
}
