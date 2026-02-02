using System.Collections.Generic;
using System.Linq;

namespace JFramework
{
    public class RemixCombatTeam : CommonCombatTeam
    {
        public override List<CombatUnit> GetUnits(bool mainTarget = false)
        {
            if (!mainTarget)
                return GetAll();

            var units = GetAll();
            if (units.Count > 0)
            {
                return units.Where(i => i.IsMainTarget()).ToList();
            }

            return units;
        }

        public override List<CombatUnit> CreateUnits(CombatContext context, List<CombatUnitInfo> teamData)
        {
            if (teamData == null || teamData.Count == 0)
                return new List<CombatUnit>();

            var actionFactory = new CombatActionFactory();
            var attrFactory = new CombatAttributeFactory();
            var buffFactory = new CombatBufferFactory();

            var parents = teamData.Where(i => i.parent == null).ToList();
            var children = teamData.Where(i => i.parent != null).ToList();
            var parentsAttr = new Dictionary<string, CombatAttributeManger>();

            foreach (var parent in parents)
            {
                var attributes = attrFactory.CreateAllAttributes(parent);
                var attrManager = new CombatAttributeManger();
                attrManager.AddRange(attributes);

                //創建並初始化戰鬥單位
                var unit = new CombatUnit();
                unit.Initialize(parent, context, actionFactory.CreateActions(parent.actionsData, unit, context), null,/*buffFactory.CreateBuffers(unitInfo.buffersData),*/ attrManager);
                unit.SetPosition(parent.position);
                unit.SetSpeed(parent.moveSpeed);
                unit.SetTargetPosition(parent.targetPosition);
                Add(unit);

                parentsAttr.Add(parent.uid, attrManager);
            }

            foreach (var child in children)
            {
                var attrManager = parentsAttr[child.parent.uid];
                var unit = new CombatUnit();
                unit.Initialize(child, context, actionFactory.CreateActions(child.actionsData, unit, context), null,/*buffFactory.CreateBuffers(unitInfo.buffersData),*/ attrManager);
                unit.SetPosition(child.position);
                unit.SetSpeed(child.moveSpeed);
                unit.SetTargetPosition(child.targetPosition);
                Add(unit);
            }

            return GetAll();
        }
    }
}