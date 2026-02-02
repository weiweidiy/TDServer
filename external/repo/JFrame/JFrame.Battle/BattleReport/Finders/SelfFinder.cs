using System;
using System.Collections.Generic;

namespace JFramework
{
    public class SelfFinder : BaseTargetFinder
    {
        public SelfFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
        {
        }

        public override List<IBattleUnit> FindTargets(object[] args)
        {
            //var owner = Owner as IBattleAction;

            //if (owner == null)
            //    throw new Exception("attach owner 转换失败 ");

            return new List<IBattleUnit>() { Owner.Owner };
        }
    }

}

///// <summary>
///// 本体（不管死活） type = 6
///// </summary>
//public class SelfFinder : BaseTargetFinder
//{
//    public SelfFinder(BattlePoint selfPoint, IPVPBattleManager manger, float arg) : base(selfPoint, manger, arg)
//    {
//    }

//    public override List<IBattleUnit> FindTargets()
//    {
//        return new List<IBattleUnit>() { Owner.Owner };
//    }
//}