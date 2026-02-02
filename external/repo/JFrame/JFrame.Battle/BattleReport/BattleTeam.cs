using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace JFramework
{

    public class BattleTeam : IBattleTeam
    {
        //public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, List<IBattleUnit>> onActionTriggerOn;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, List<IBattleUnit>,float> onActionCast;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction,  float> onActionStartCD;

        public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo> onDamage;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, int> onHeal;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit> onDead;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, int> onReborn;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, int> onMaxHpUp;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, int> onDebuffAnti;

        public event Action<PVPBattleManager.Team, IBattleUnit, IBuffer> onBufferAdded;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBuffer> onBufferRemoved;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBuffer> onBufferCast;
        public event Action<PVPBattleManager.Team, IBattleUnit, IBuffer,int, float[]> onBufferUpdate;

        Dictionary<BattlePoint, IBattleUnit> units = new Dictionary<BattlePoint, IBattleUnit>();

        PVPBattleManager.Team team;
        public PVPBattleManager.Team Team { get => team; }

        public BattleTeam(PVPBattleManager.Team team, Dictionary<BattlePoint, IBattleUnit> units)
        {
            this.team = team;
            this.units = units;

            

        }


        public void Initialize()
        {
            if (this.units != null)
            {
                foreach (var key in units.Keys)
                {
                    var unit = units[key];

                    unit.Initialize();
                    //unit.onActionTriggerOn += Unit_onActionTriggerOn;
                    unit.onActionCast += Unit_onActionCast;
                    unit.onActionStartCD += Unit_onActionStartCD;
                    //unit.onActionHitTarget += Unit_onActionDone;
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
                }
            }
        }





        #region 响应事件


        private void Unit_onActionCast(IBattleUnit arg1, IBattleAction arg2, List<IBattleUnit> arg3, float duration)
        {
            onActionCast?.Invoke(team, arg1, arg2, arg3, duration);
        }
        private void Unit_onActionStartCD(IBattleUnit arg1, IBattleAction arg2, float arg3)
        {
            onActionStartCD?.Invoke(team,arg1, arg2, arg3);
        }



        private void Unit_onDamage(IBattleUnit arg1, IBattleAction arg2, IBattleUnit arg3, ExecuteInfo arg4)
        {
            onDamage?.Invoke(team, arg1, arg2, arg3, arg4);
        }

        private void Unit_onHeal(IBattleUnit arg1, IBattleAction arg2, IBattleUnit arg3, int arg4)
        {
            onHeal?.Invoke(team,arg1, arg2, arg3, arg4);
        }

        private void Unit_onMaxHpUp(IBattleUnit arg1, IBattleAction arg2, IBattleUnit arg3, int arg4)
        {
            onMaxHpUp?.Invoke(team,arg1,arg2, arg3, arg4);
        }

        private void Unit_onRebord(IBattleUnit arg1, IBattleAction arg2, IBattleUnit arg3, int arg4)
        {
            onReborn?.Invoke(team, arg1, arg2, arg3, arg4);
        }


        private void Unit_onDead(IBattleUnit arg1, IBattleAction arg2, IBattleUnit arg3)
        {
            onDead?.Invoke(team, arg1, arg2, arg3);
        }

        private void Unit_onBufferAdded(IBattleUnit arg1, IBuffer arg2)
        {
            onBufferAdded?.Invoke(team, arg1, arg2);
        }
        private void Unit_onBufferCast(IBattleUnit arg1, IBuffer arg2)
        {
            onBufferCast?.Invoke(team, arg1, arg2);
        }
        private void Unit_onBufferRemoved(IBattleUnit arg1, IBuffer arg2)
        {
            onBufferRemoved?.Invoke(team, arg1, arg2);
        }

        private void Unit_onBufferUpdate(IBattleUnit arg1, IBuffer arg2, int arg3, float[] arg4)
        {
            onBufferUpdate?.Invoke(team, arg1, arg2, arg3, arg4);
        }




        private void Unit_onDebuffAnti(IBattleUnit arg1, IBattleAction arg2, IBattleUnit arg3, int arg4)
        {
            onDebuffAnti?.Invoke(team,arg1,arg2, arg3, arg4);
        }


        #endregion

        /// <summary>
        /// 添加一个单位
        /// </summary>
        /// <param name="point"></param>
        /// <param name="unit"></param>
        public void AddUnit(BattlePoint point, IBattleUnit unit)
        {
            if (units == null)
                units = new Dictionary<BattlePoint, IBattleUnit>();

            units.Add(point, unit);
        }


        /// <summary>
        /// 获取指定位置战斗单位
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IBattleUnit GetUnit(BattlePoint point)
        {
            if (units.ContainsKey(point))
                return units[point];

            throw new Exception("没有找到对应的点位 " + point.Point);
        }

        /// <summary>
        /// 获取指定位置单位
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IBattleUnit GetUnit(int point)
        {
            foreach (var btPoint in units.Keys)
            {
                if (btPoint.Point == point)
                    return units[btPoint];
            }

            throw new Exception("没有找到对应的点位 " + point);
        }


        public BattlePoint GetPoint(IBattleUnit unit)
        {
            foreach (var item in units)
            {
                if (item.Value.Uid == unit.Uid)
                    return item.Key;
            }
            return null;
        }

        /// <summary>
        /// 获取所有战斗对象
        /// </summary>
        /// <returns></returns>
        public List<IBattleUnit> GetUnits()
        {
            return units.Values.ToList();
        }

        public void Update(CombatFrame frame)
        {
            var collection = GetUnits();
            if(collection == null)
                return;

            foreach (var unit in collection)
            {
                unit.Update(frame);
            }
        }

        public int GetUnitCount()
        {
            return units.Count;
        }

        public bool IsAllDead()
        {
            foreach (var unit in units.Values)
            {
                if (unit.IsAlive())
                    return false;
            }

            return true;
        }


    }
}