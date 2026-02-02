using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// 队友顺序寻找存活目标（可复数） type = 7
    /// </summary>
    public class OrderFriendsFinder : BaseTargetFinder
    {
        public OrderFriendsFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
        {
        }

        public override List<IBattleUnit> FindTargets(object[] args)
        {
            var result = new List<IBattleUnit>();

            var units = manger.GetUnits(selfPoint.Team);

            //debug
            foreach (var unit in units)
            {
                if (unit.IsAlive() && result.Count < arg)
                {
                    result.Add(unit);
                }
            }

            return result;
        }
    }

}

///// <summary>
///// 队友顺序寻找存活目标（可复数） type = 7
///// </summary>
//public class OrderFriendsFinder : BaseTargetFinder
//{
//    public OrderFriendsFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
//    {
//    }

//    public override List<IBattleUnit> FindTargets()
//    {
//        var result = new List<IBattleUnit>();

//        var units = manger.GetUnits(selfPoint.Team);

//        //debug
//        foreach (var unit in units)
//        {
//            if (unit.IsAlive() && result.Count < arg)
//            {
//                result.Add(unit);
//            }
//        }

//        return result;
//    }
//}