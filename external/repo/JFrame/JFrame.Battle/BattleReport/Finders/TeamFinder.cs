using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 11
    /// </summary>
    public class TeamFinder : BaseTargetFinder
    {
        public TeamFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
        {
        }

        public override List<IBattleUnit> FindTargets(object[] args)
        {
            var result = new List<IBattleUnit>();

            var units = manger.GetUnits(arg == 1? PVPBattleManager.Team.Attacker : PVPBattleManager.Team.Defence);

            //debug
            foreach (var unit in units)
            {
                if (unit.IsAlive())
                {
                    result.Add(unit);
                }
            }

            return result;
        }
    }

}

///// <summary>
///// 敌对队伍顺序寻找存活目标（可复数） type = 1
///// </summary>
//public class OrderOppoFinder : BaseTargetFinder
//{

//    public OrderOppoFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg) { }

//    /// <summary>
//    /// 获取攻击目标
//    /// </summary>
//    /// <returns></returns>
//    public override List<IBattleUnit> FindTargets()
//    {
//        var result = new List<IBattleUnit>();

//        var units = manger.GetUnits(manger.GetOppoTeam(selfPoint.Team));

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