using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// 对象过滤器，根据trigger传入的目标进行过滤 type 9
    /// </summary>
    public class FliterHpFinder : BaseTargetFinder
    {
        float hpPercent;
        public FliterHpFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
        {
            hpPercent = arg;
        }

        public override List<IBattleUnit> FindTargets(object[] args)
        {
            //0: action, 1 target, 2 info
            IBattleUnit ts = args[1] as IBattleUnit;
            if (ts == null)
                throw new System.Exception("FliterHpFinder 转换错误");

            List<IBattleUnit> result = new List<IBattleUnit>();

            if (ts.HP / (float)ts.MaxHP <= hpPercent)
                result.Add(ts);

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