using System.Collections.Generic;

namespace JFramework
{



    public class OrderOppoTopAtkFinder : BaseTargetFinder
    {
        public OrderOppoTopAtkFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
        {
        }

        public override List<IBattleUnit> FindTargets(object[] args)
        {
            var result = new List<IBattleUnit>();

            var units = manger.GetUnits(manger.GetOppoTeam(selfPoint.Team));

            int index = 0;
            int topAtk = 0;
            //debug
            for (int i = 0; i < units.Count; i++)
            {
                var unit = units[i];

                if (unit.IsAlive() && unit.Atk > topAtk)
                {
                    topAtk = unit.Atk;
                    index = i;
                }
            }

            result.Add(units[index]);

            return result;
        }
    }

}

///// <summary>
///// 敌对队伍顺序寻找存活攻击最高目标（可复数） type = 8
///// </summary>
//public class OrderOppoTopAtkFinder : BaseTargetFinder
//{
//    public OrderOppoTopAtkFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
//    {
//    }

//    public override List<IBattleUnit> FindTargets()
//    {
//        var result = new List<IBattleUnit>();

//        var units = manger.GetUnits(manger.GetOppoTeam(selfPoint.Team));

//        int index = 0; 
//        int topAtk = 0;
//        //debug
//        for(int i = 0; i < units.Count; i ++)
//        {
//            var unit = units[i];

//            if (unit.IsAlive() && unit.Atk > topAtk)
//            {
//                topAtk = unit.Atk;
//                index = i; 
//            }
//        }

//        result.Add(units[index] );

//        return result;
//    }
//}