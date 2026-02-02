using System;
using System.Collections;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 2 查找距离最近的N个单位    参数：0=队伍(0友军，1敌军，2所有)   1=主类型  2=子类型  3模式(0模式单位， 1逻辑单位) 4=个数 5=距离
    /// </summary>
    public class FinderFindNearest : FinderFindUnits
    {
        protected Utility utility = new Utility();

        public override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 1;
        }

        protected float GetDistance()
        {
            return GetCurArg(5);
        }

        protected override bool IsHit(CombatUnit unit, CombatExtraData extraData)
        {
            var myX = extraData.Owner.GetPosition().x;
            return unit.IsAlive() && Math.Abs(myX - unit.GetPosition().x) <= GetDistance(); 
        }


        protected override List<CombatUnit> OnSortUnits(List<CombatUnit> units, CombatExtraData extraData)
        {
            var myX = extraData.Owner.GetPosition().x;
            utility.BinarySort(units, GetCompare(myX)); //按距离排序
            return units;
        }



        protected virtual IComparer<CombatUnit> GetCompare(float myX)
        {
            return new Compare(myX);
        }



        /// <summary>
        /// 按距離由近到遠
        /// </summary>
        class Compare : IComparer<CombatUnit>
        {
            float myX;
            public Compare(float myX)
            {
                this.myX = myX;
            }

            int IComparer<CombatUnit>.Compare(CombatUnit x, CombatUnit y)
            {
                var unit1 = x as CombatUnit;
                var unit2 = y as CombatUnit;

                if (Math.Abs(myX - unit1.GetPosition().x) > Math.Abs(myX - unit2.GetPosition().x))
                    return 1;

                if (Math.Abs(myX - unit1.GetPosition().x) < Math.Abs(myX - unit2.GetPosition().x))
                    return -1;

                return 0;
            }
        }
    }
}