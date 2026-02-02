using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework
{


    public class RandomOppoFinder : BaseTargetFinder
    {
        public RandomOppoFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
        {
        }

        public override List<IBattleUnit> FindTargets(object[] args)
        {
            var units = manger.GetUnits(manger.GetOppoTeam(selfPoint.Team));
            var random = new Random();
            Func<IBattleUnit, bool> customCondition = i => i.IsAlive();

            var result = units
            .Where(customCondition) // 应用自定义条件
            .OrderBy(i => random.Next()) // 随机排序
            .Take((int)arg) // 取前三个
            //.Distinct() // 去重
            .ToList(); // 转换为列表

            return result;
        }
    }
}

///// <summary>
///// 随机敌方存活目标（可复数）type = 4
///// </summary>
//public class RandomOppoFinder : BaseTargetFinder
//{
//    public RandomOppoFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
//    {
//    }

//    public override List<IBattleUnit> FindTargets()
//    {
//        var units = manger.GetUnits(manger.GetOppoTeam(selfPoint.Team));
//        var random = new Random();
//        Func<IBattleUnit, bool> customCondition = i => i.IsAlive();

//        var result = units
//        .Where(customCondition) // 应用自定义条件
//        .OrderBy(i => random.Next()) // 随机排序
//        .Take((int)arg) // 取前三个
//        //.Distinct() // 去重
//        .ToList(); // 转换为列表

//        return result;
//    }
//}