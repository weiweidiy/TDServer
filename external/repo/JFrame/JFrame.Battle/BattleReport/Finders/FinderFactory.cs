using System;

namespace JFramework
{


    public class FinderFactory
    {
        public IBattleTargetFinder Create(PVPBattleManager pvpBattleManager, int finderType, BattlePoint point, float arg)
        {
            switch (finderType)
            {
                case 0:
                    return new NoneFinder(point, pvpBattleManager, arg);
                case 1: //顺序找目标（可复数）
                    return new OrderOppoFinder(point, pvpBattleManager, arg);
                case 2: //倒序找目标（可复数）
                    return new ReverseOrderOppoFinder(point, pvpBattleManager, arg);
                case 3: //正序找自己队伍非满血目标（可复数）
                    return new FriendsLowestHpFinder(point, pvpBattleManager, arg);
                case 4: //随机敌方
                    return new RandomOppoFinder(point, pvpBattleManager, arg);
                case 6: //本体
                    return new SelfFinder(point, pvpBattleManager, arg);
                case 7: //顺序己方（可复数）
                    return new OrderFriendsFinder(point, pvpBattleManager, arg);
                case 8: //顺序敌方攻击最高的
                    return new OrderOppoTopAtkFinder(point, pvpBattleManager, arg);
                case 9:
                    return new FliterHpFinder(point, pvpBattleManager, arg);
                case 10:
                    return new FliterFinder(point, pvpBattleManager, arg);
                case 11:
                    return new TeamFinder(point, pvpBattleManager, arg);
                case 12:
                    return new RandomFriendFinder(point, pvpBattleManager, arg); 
                default:
                    throw new Exception("没有实现目标 finder type " + finderType);
            }
        }
    }
}

//public class FinderFactory
//{
//    public IBattleTargetFinder Create(PVPBattleManager pvpBattleManager, int finderType, BattlePoint point, float arg)
//    {
//        switch (finderType)
//        {
//            case 1: //顺序找目标（可复数）
//                return new OrderOppoFinder(point, pvpBattleManager, arg);
//            case 2: //倒序找目标（可复数）
//                return new ReverseOrderOppoFinder(point, pvpBattleManager, arg);
//            case 3: //正序找自己队伍非满血目标（可复数）
//                return new OrderFriendsHurtFinder(point, pvpBattleManager, arg);
//            case 4: //随机敌方
//                return new RandomOppoFinder(point, pvpBattleManager, arg);
//            case 6: //本体
//                return new SelfFinder(point, pvpBattleManager, arg);
//            case 7: //顺序己方（可复数）
//                return new OrderFriendsFinder(point, pvpBattleManager, arg);
//            case 8: //顺序敌方攻击最高的
//                return new OrderOppoTopAtkFinder(point, pvpBattleManager, arg);
//            default:
//                throw new Exception("没有实现目标 finder type " + finderType);
//        }
//    }
//}