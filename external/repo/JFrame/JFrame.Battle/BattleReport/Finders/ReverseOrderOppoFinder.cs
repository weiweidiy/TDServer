using System.Collections.Generic;

namespace JFramework
{


    public class ReverseOrderOppoFinder : BaseTargetFinder
    {
        public ReverseOrderOppoFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg) { }

        /// <summary>
        /// 获取攻击目标
        /// </summary>
        /// <returns></returns>
        public override List<IBattleUnit> FindTargets(object[] args)
        {
            var result = new List<IBattleUnit>();

            var units = manger.GetUnits(manger.GetOppoTeam(selfPoint.Team));

            //debug
            for (int i = units.Count - 1; i >= 0; i--)
            {
                var unit = units[i];

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
///// 敌对队伍反序寻找存活目标（可复数） type = 2
///// </summary>
//public class ReverseOrderOppoFinder : BaseTargetFinder
//{
//    public ReverseOrderOppoFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg):base(selfPoint, manger, arg) { }

//    /// <summary>
//    /// 获取攻击目标
//    /// </summary>
//    /// <returns></returns>
//    public override List<IBattleUnit> FindTargets()
//    {
//        var result = new List<IBattleUnit>();

//        var units = manger.GetUnits(manger.GetOppoTeam(selfPoint.Team));

//        //debug
//        for(int i = units.Count - 1; i >= 0; i--)
//        {
//            var unit = units[i];

//            if (unit.IsAlive() && result.Count < arg)
//            {
//                result.Add(unit);
//            }
//        }

//        return result;
//    }
//}