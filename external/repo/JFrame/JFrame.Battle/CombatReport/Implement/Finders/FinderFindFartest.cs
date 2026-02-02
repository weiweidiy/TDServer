using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 3 
    /// </summary>
    public class FinderFindFartest : FinderFindNearest
    {
        protected override IComparer<CombatUnit> GetCompare(float myX)
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

                if (Math.Abs(myX - unit1.GetPosition().x) < Math.Abs(myX - unit2.GetPosition().x))
                    return 1;

                if (Math.Abs(myX - unit1.GetPosition().x) > Math.Abs(myX - unit2.GetPosition().x))
                    return -1;

                return 0;
            }
        }
    }
}