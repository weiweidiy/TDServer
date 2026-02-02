using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JFramework
{
    /// <summary>
    /// 獲取所有單位時，只會返回特殊的單位
    /// </summary>
    public class SpecialCombatTeam : CommonCombatTeam
    {
        public override List<CombatUnit> GetUnits(bool mainTarget = false)
        {
            if (!mainTarget)
                return GetAll();

            var units = GetAll();
            if (units.Count > 0)
                return new List<CombatUnit>() { units[0] };

            return units;
        }

        /// <summary>
        /// 属性全部用主单位的
        /// </summary>
        /// <param name="context"></param>
        /// <param name="teamData"></param>
        public override List<CombatUnit> CreateUnits(CombatContext context, List<CombatUnitInfo> teamData)
        {
            if (teamData == null || teamData.Count == 0)
                return new List<CombatUnit>();

            var actionFactory = new CombatActionFactory();
            var attrFactory = new CombatAttributeFactory();
            var buffFactory = new CombatBufferFactory();

            //主单位
            var mainUnit = teamData[0];
            var attributes = attrFactory.CreateAllAttributes(mainUnit);
            var attrManager = new CombatAttributeManger();
            attrManager.AddRange(attributes);

            //創建並初始化隊伍
            for (int i = 0; i < teamData.Count; i++)
            {
                var unitInfo = teamData[i];
                //創建並初始化戰鬥單位
                var unit = new CombatUnit();

                var readyCd = unitInfo.readyCd;
                //if (unitInfo.mainType == UnitMainType.Hero)
                //{
                //    readyCd = unitInfo.readyCd;
                //   // UnityEngine.Debug.LogError("cd " + readyCd);
                //}                

                var actions = actionFactory.CreateActions(unitInfo.actionsData, unit, context, readyCd);

                unit.Initialize(unitInfo, context, actions, null,/*buffFactory.CreateBuffers(unitInfo.buffersData),*/ attrManager, readyCd);
                unit.SetPosition(unitInfo.position);
                unit.SetSpeed(unitInfo.moveSpeed);
                unit.SetTargetPosition(unitInfo.targetPosition);
                Add(unit);

    //            foreach (var action in unitInfo.actionsData.Keys)
    //            {
    //                try
    //                {
    //                    UnityEngine.Debug.LogError("team:" + TeamId + " 創建unit " + unitInfo.id + "   actionId: " + action + "position " + unitInfo.position.x
    //+ /*"  tarPos: " + unitInfo.targetPosition.x +*/ "  speed:" + unitInfo.moveSpeed.x);
    //                } catch (Exception e)
    //                {
    //                    UnityEngine.Debug.Assert(unitInfo.position != null, unitInfo.id + "  unitInfo.position is null");
    //                    UnityEngine.Debug.Assert(unitInfo.targetPosition != null, unitInfo.id + "  unitInfo.targetPosition is null");
    //                    UnityEngine.Debug.Assert(unitInfo.moveSpeed != null, unitInfo.id + "  unitInfo.moveSpeed is null");

    //                }
                                        

    //            }
            }

            return GetAll();
        }
    }
}