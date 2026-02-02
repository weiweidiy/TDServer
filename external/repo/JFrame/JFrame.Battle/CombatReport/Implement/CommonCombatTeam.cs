using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace JFramework
{
    public class CommonCombatTeam : ListContainer<CombatUnit>, ICombatTeam<CombatUnit>, ICombatUpdatable
    {
        //public event Action<int, ICombatUnit, ICombatAction, List<ICombatUnit>, float> onActionCast;
        //public event Action<int, ICombatUnit, ICombatAction, float> onActionStartCD;

        //public event Action<int, CombatExtraData> onDamage;
        //public event Action<int, ICombatUnit, ICombatAction, ICombatUnit, int> onHeal;
        //public event Action<int, CombatExtraData> onDead;
        //public event Action<int, ICombatUnit, ICombatAction, ICombatUnit, int> onReborn;
        //public event Action<int, ICombatUnit, ICombatAction, ICombatUnit, int> onMaxHpUp;
        //public event Action<int, ICombatUnit, ICombatAction, ICombatUnit, int> onDebuffAnti;

        //public event Action<int, ICombatUnit, ICombatBuffer> onBufferAdded;
        //public event Action<int, ICombatUnit, ICombatBuffer> onBufferRemoved;
        //public event Action<int, ICombatUnit, ICombatBuffer> onBufferCast;
        //public event Action<int, ICombatUnit, ICombatBuffer, int, float[]> onBufferUpdate;


        public event Action<int, CombatExtraData> onActionCast;
        public event Action<int, CombatExtraData> onActionStartCD;
        public event Action<int, CombatExtraData> onDamage;
        public event Action<int, CombatExtraData> onMiss;
        public event Action<int, CombatExtraData> onHeal;
        public event Action<int, CombatExtraData> onDead;
        public event Action<int, CombatExtraData> onReborn;
        public event Action<int, CombatExtraData> onMaxHpUp;
        public event Action<int, CombatExtraData> onDebuffAnti;
        public event Action<int, CombatExtraData> onBufferAdded;
        public event Action<int, CombatExtraData> onBufferRemoved;
        public event Action<int, CombatExtraData> onBufferCast;
        public event Action<int, CombatExtraData> onBufferUpdate;
        public event Action<int, CombatExtraData> onUnitStartMove;
        public event Action<int, CombatExtraData> onUnitSpeedChanged;
        public event Action<int, CombatExtraData> onUnitEndMove;
        public event Action<int, CombatExtraData> onShootTargetChanged;
        public event Action<int, CombatExtraData> onActionCdChange;
        


        int team;
        public int TeamId => team;

        public void AddUnit(CombatUnit unit)
        {
            Add(unit);
        }

        public void RemoveUnit(CombatUnit unit)
        {
            Remove(unit.Uid);
        }

        public CombatUnit GetUnit(string uid)
        {
            return Get(uid);
        }

        public int GetUnitCount()
        {
            return Count();
        }

        public virtual List<CombatUnit> GetUnits(bool findMode = false)
        {
            return GetAll();
        }

        public void UpdatePosition(CombatFrame frame)
        {
            var units = GetUnits();
            if (units == null)
                return;

            foreach (var unit in units)
            {
                unit.UpdatePosition(frame);
            }
        }

        public void Update(CombatFrame frame)
        {
            var units = GetUnits();
            if (units == null)
                return;

            foreach (var unit in units)
            {
                (unit as ICombatUpdatable).Update(frame);
            }
        }

        public virtual bool IsAllDead()
        {
            foreach (var unit in GetUnits())
            {
                if (unit.IsAlive())
                    return false;
            }

            return true;
        }


        public void Initialize(int teamId, CombatContext context, List<CombatUnitInfo> teamData)
        {
            team = teamId;

            var units = CreateUnits(context, teamData);

            if (units == null)
                return;

            foreach (var unit in units)
            {

                //(unit as CombatUnit).Initialize(context);
                //unit.onActionTriggerOn += Unit_onActionTriggerOn;
                unit.onActionCast += Unit_onActionCast;
                unit.onActionStartCD += Unit_onActionStartCD;
                unit.onActionCdChanged += Unit_onActionCdChanged;
                //unit.onActionHitTarget += Unit_onActionDone;
                unit.onMiss += Unit_onMiss;
                unit.onDamaged += Unit_onDamage;
                unit.onHealed += Unit_onHeal;
                unit.onRebord += Unit_onRebord;
                unit.onDebuffAnti += Unit_onDebuffAnti;
                unit.onMaxHpUp += Unit_onMaxHpUp;
                unit.onDead += Unit_onDead;
                unit.onBufferAdded += Unit_onBufferAdded;
                unit.onBufferRemoved += Unit_onBufferRemoved;
                unit.onBufferCast += Unit_onBufferCast;
                unit.onBufferUpdate += Unit_onBufferUpdate;
                unit.onStartMove += Unit_onStartMove;
                unit.onSpeedChanged += Unit_onSpeedChanged;
                unit.onEndMove += Unit_onEndMove;
                unit.onShootTargetChanged += Unit_onShootTargetChanged;
            }
        }



        public void Start()
        {
            var units = GetUnits();
            if (units == null)
                return;

            foreach(var unit in units)
            {
                unit.Start();
            }
        }

        public void Stop()
        {
            var units = GetUnits();
            if (units == null)
                return;

            foreach (var unit in units)
            {
                unit.Stop();
            }
        }


        public virtual List<CombatUnit> CreateUnits(CombatContext context, List<CombatUnitInfo> teamData)
        {
            if (teamData != null)
            {
                var actionFactory = new CombatActionFactory();
                var attrFactory = new CombatAttributeFactory();
                var buffFactory = new CombatBufferFactory();
                //創建並初始化隊伍
                //foreach (var unitInfo in teamData)
                for(int i = 0; i < teamData.Count;i ++)
                {
                    var unitInfo = teamData[i];
                    try
                    {                 
                        //創建並初始化戰鬥單位
                        var unit = new CombatUnit();
                        var attributes = attrFactory.CreateAllAttributes(unitInfo);
                        var attrManager = new CombatAttributeManger();
                        attrManager.AddRange(attributes);
                        unit.Initialize(unitInfo, context, actionFactory.CreateActions(unitInfo.actionsData, unit, context), null/* buffFactory.CreateBuffers(unitInfo.buffersData)*/, attrManager);
                        unit.SetPosition(unitInfo.position);
                        unit.SetSpeed(unitInfo.moveSpeed);
                        unit.SetTargetPosition(unitInfo.targetPosition);
                        Add(unit);

                        //foreach(var action in unitInfo.actionsData.Keys)
                        //{
                        //    UnityEngine.Debug.LogError("team:" + team + " 創建unit " + unitInfo.id + "   actionId: " + action);
                        //}
                        
                    }
                    catch(Exception ex)
                    {
                        if(context.Logger != null)
                            context.Logger.LogError(ex.Message + $" 创建unit失败 检查配置 unitID:{unitInfo.id}");

                        continue;
                    }
                }

                return GetAll();
            }
            return new List<CombatUnit>();
        }


        #region 响应事件


        private void Unit_onActionCast(CombatExtraData extraData)
        {
            onActionCast?.Invoke(team, extraData);
        }
        private void Unit_onActionStartCD(CombatExtraData extraData)
        {
            onActionStartCD?.Invoke(team, extraData);
        }

        private void Unit_onDamage(CombatExtraData extraData /*ICombatUnit arg1, ICombatAction arg2, ICombatUnit arg3, ExecuteInfo arg4*/)
        {
            onDamage?.Invoke(team, extraData);
        }

        private void Unit_onHeal(CombatExtraData extraData)
        {
            onHeal?.Invoke(team, extraData);
        }

        private void Unit_onMaxHpUp(CombatExtraData extraData)
        {
            onMaxHpUp?.Invoke(team, extraData);
        }

        private void Unit_onRebord(CombatExtraData extraData)
        {
            onReborn?.Invoke(team, extraData);
        }


        private void Unit_onDead(CombatExtraData extraData)
        {
            onDead?.Invoke(team, extraData);
        }

        private void Unit_onBufferAdded(CombatExtraData extraData)
        {
            onBufferAdded?.Invoke(team, extraData);
        }
        private void Unit_onBufferCast(CombatExtraData extraData)
        {
            onBufferCast?.Invoke(team, extraData);
        }
        private void Unit_onBufferRemoved(CombatExtraData extraData)
        {
            onBufferRemoved?.Invoke(team, extraData);
        }

        private void Unit_onBufferUpdate(CombatExtraData extraData)
        {
            onBufferUpdate?.Invoke(team, extraData);
        }
        private void Unit_onDebuffAnti(CombatExtraData extraData)
        {
            onDebuffAnti?.Invoke(team, extraData);
        }

        private void Unit_onStartMove(CombatExtraData extraData)
        {
            onUnitStartMove?.Invoke(team, extraData);
        }
        private void Unit_onSpeedChanged(CombatExtraData extraData)
        {
            onUnitSpeedChanged?.Invoke(team, extraData);
        }
        private void Unit_onEndMove(CombatExtraData extraData)
        {
            onUnitEndMove?.Invoke(team, extraData);
        }

        private void Unit_onShootTargetChanged(CombatExtraData extraData)
        { 
            onShootTargetChanged?.Invoke(team, extraData);
        }

        private void Unit_onMiss(CombatExtraData extraData)
        {
            onMiss?.Invoke(team, extraData);
        }

        private void Unit_onActionCdChanged(CombatExtraData extraData)
        {
            onActionCdChange?.Invoke(team, extraData);
        }
        #endregion
    }
}