using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    /// type 3 修改：改成血量最低的
    /// </summary>
    public class FriendsLowestHpFinder : BaseTargetFinder
    {
        public FriendsLowestHpFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg) { }

        public override List<IBattleUnit> FindTargets(object[] args)
        {
            var result = new List<IBattleUnit>();

            var units = manger.GetUnits(selfPoint.Team);

            int lowestHp = int.MaxValue;
            int index = 0;
            //debug
            for (int i = 0; i < units.Count; i ++)
            {
                var unit = units[i];
                if (unit.IsAlive() && !unit.IsHpFull() && result.Count < arg  && unit.HP < lowestHp)
                {
                    index = i;
                    lowestHp = unit.HP;
                }
            }

            result.Add(units[index]);
            return result;
        }
    }
}

///// <summary>
///// 自己队伍顺序伤员成员 ： type = 3
///// </summary>
//public class OrderFriendsHurtFinder : BaseTargetFinder
//{
//    public OrderFriendsHurtFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg) { }

//    public override List<IBattleUnit> FindTargets()
//    {
//        var result = new List<IBattleUnit>();

//        var units = manger.GetUnits(selfPoint.Team);

//        //debug
//        foreach (var unit in units)
//        {
//            if (unit.IsAlive() && !unit.IsHpFull() && result.Count < arg)
//            {
//                result.Add(unit);
//            }
//        }

//        return result;
//    }
//}