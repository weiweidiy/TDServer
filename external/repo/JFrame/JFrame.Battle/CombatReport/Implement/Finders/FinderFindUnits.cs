using System;
using System.Collections.Generic;

namespace JFramework
{


    /// <summary>
    ///  参数：0=队伍(0友军，1敌军，2所有, 3不包含自己的友军)   1=主类型  2=子类型  3模式(0模式单位， 1逻辑单位) 4=个数
    /// </summary>
    public abstract class FinderFindUnits : CombatBaseFinder
    {
        public override int GetValidArgsCount()
        {
            return 5;
        }

        /// <summary>
        /// 获取队伍参数ID
        /// </summary>
        /// <returns></returns>
        protected float GetTeamModeArg()
        {
            return GetCurArg(0);
        }

        /// <summary>
        /// 获取单位主类型
        /// </summary>
        /// <returns></returns>
        protected int GetUnitMainTypeArg()
        {
            return (int)GetCurArg(1);
        }

        /// <summary>
        /// 获取单位子类型
        /// </summary>
        /// <returns></returns>
        protected int GetUnitSubTypeArg()
        {
            return (int)GetCurArg(2);
        }

        /// <summary>
        /// 获取搜索模式（0模式单位，比如只找主目标， 1：找所有unit单元）
        /// </summary>
        /// <returns></returns>
        protected bool GetFindModeArg()
        {
            return (int)GetCurArg(3) == 0 ? true : false;
        }

        /// <summary>
        /// 查找个数
        /// </summary>
        /// <returns></returns>
        protected int GetFindAmountArg()
        {
            return (int)GetCurArg(4);
        }


        public override List<CombatUnit> FindTargets(CombatExtraData extraData)
        {
            var teamArg = GetTeamModeArg();

            switch (teamArg)
            {
                case 0: //友军
                    {
                        var targetTeamId = context.CombatManager.GetFriendTeamId(extraData.Owner);
                        var units = context.CombatManager.GetUnits(targetTeamId, GetFindModeArg());
                        return FiltUnitType(units, extraData);
                    }
                case 1://敌军
                    {
                        var targetTeamId = context.CombatManager.GetOppoTeamId(extraData.Owner);
                        var units = context.CombatManager.GetUnits(targetTeamId, GetFindModeArg());
                        return FiltUnitType(units, extraData);
                    }
                case 2://两军
                    {
                        var units = context.CombatManager.GetUnits(GetFindModeArg());
                        return FiltUnitType(units, extraData);
                    }
                case 3: //不包含自己的友军
                    {
                        var targetTeamId = context.CombatManager.GetFriendTeamId(extraData.Owner);
                        var units = context.CombatManager.GetUnits(targetTeamId, GetFindModeArg());
                        return FiltUnitType(units, extraData, false);
                    }
                default:
                    throw new NotImplementedException($"{GetType()} 没有实现队伍模式 {teamArg}");
            }

        }


        /// <summary>
        /// 获取单位类型
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected List<CombatUnit> FiltUnitType(List<CombatUnit> units, CombatExtraData extraData, bool includeSelf = true)
        {
            var result = new List<CombatUnit>();

            foreach (var unit in units)
            {
                var selfUid = extraData.Caster.Uid;
                if (unit.Uid == selfUid && !includeSelf)
                    continue;

                bool mainTypeHit = false;
                bool subTypeHit = false;

                var mainTypeArg = GetUnitMainTypeArg();
                if (mainTypeArg == 0)
                    mainTypeHit = true;

                if (!mainTypeHit)
                    mainTypeHit = unit.IsMainType((UnitMainType)mainTypeArg);

                //检查子类型
                var subTypeArg = GetUnitSubTypeArg();
                if (subTypeArg == 0)
                    subTypeHit = true;

                if (!subTypeHit)
                    subTypeHit = unit.IsSubType((UnitSubType)subTypeArg);


                //都命中则加入
                if (mainTypeHit && subTypeHit)
                    result.Add(unit);

                //子类继续过滤

                result = OnFiltUnits(result, extraData);
                result = OnSortUnits(result, extraData);

                //取指定个数
                var finalCount = Math.Min(result.Count, GetFindAmountArg());
                result = result.GetRange(0, finalCount);
            }

            return result;
        }

        /// <summary>
        /// 子类过滤方法
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        protected List<CombatUnit> OnFiltUnits(List<CombatUnit> units, CombatExtraData extraData)
        {
            for (int i = units.Count - 1; i >= 0; i--)
            {
                var unit = units[i];
                if (IsHit(unit, extraData))
                    continue;

                units.Remove(unit);
            }

            return units;
        }

        /// <summary>
        /// 是否命中
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        protected abstract bool IsHit(CombatUnit unit, CombatExtraData extraData);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="units"></param>
        /// <param name="extraData"></param>
        /// <returns></returns>
        protected virtual List<CombatUnit> OnSortUnits(List<CombatUnit> units, CombatExtraData extraData)
        { return units; }


        ///// <summary>
        ///// 獲取距離最近的n個單位()
        ///// </summary>
        ///// <returns></returns>
        //public List<CombatUnit> GetNearestOppoUnitInRange(CombatExtraData extraData, int count)
        //{
        //    var reslut = new List<CombatUnit>();
        //    var oppoTeamId = context.CombatManager.GetOppoTeamId(extraData.Caster);
        //    //獲取所有攻擊範圍内的單位
        //    var oppoUnits = context.CombatManager.GetUnitsInRange(extraData.Caster, oppoTeamId, GetAtkRange(), true, true);

        //    //CombatUnit selfUnit = null;
        //    //if (Owner is CombatUnitAction)
        //    //{
        //    //    var action = Owner as CombatUnitAction;
        //    //    selfUnit = action.Owner;
        //    //}
        //    //else //是一個buffaction
        //    //{
        //    //    var action = Owner as CombatBufferAction;
        //    //    var buffer = action.Owner;
        //    //    selfUnit = buffer.SourceUnit;

        //    //}


        //    //獲取距離最近的
        //    var myX = extraData.Caster.GetPosition().x;
        //    var arr = oppoUnits.ToArray();
        //    utility.BinarySort<CombatUnit>(arr, new Compare(myX));

        //    //保證不能超過數組長度
        //    var finalCount = Math.Min(arr.Length, count);

        //    for (int i = 0; i < finalCount; i++)
        //    {
        //        reslut.Add(arr[i]);
        //    }

        //    return reslut;
        //}


        //protected override void OnUpdate(BattleFrame frame)
        //{
        //    //throw new NotImplementedException();
        //}

        //class Compare : IComparer<CombatUnit>
        //{
        //    float myX;
        //    public Compare(float myX)
        //    {
        //        this.myX = myX;
        //    }

        //    int IComparer<CombatUnit>.Compare(CombatUnit x, CombatUnit y)
        //    {
        //        var unit1 = x as CombatUnit;
        //        var unit2 = y as CombatUnit;

        //        if (Math.Abs(myX - unit1.GetPosition().x) > Math.Abs(myX - unit2.GetPosition().x))
        //            return 1;

        //        if (Math.Abs(myX - unit1.GetPosition().x) < Math.Abs(myX - unit2.GetPosition().x))
        //            return -1;

        //        return 0;
        //    }
        //}
    }
}