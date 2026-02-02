using Game.Share;
using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class TiktokJCombatTurnBasedReportBuilder : JCombatTurnBasedReportBuilder
    {
        public TiktokJCombatTurnBasedReportBuilder(IJCombatSeatBasedQuery jcombatQuery) : base(jcombatQuery)
        {
        }



        protected override JCombatTurnBasedReportData<T> CreateReportData<T>()
        {
            return (JCombatTurnBasedReportData<T>)(object)new TiktokJCombatTurnBasedReportData();
        }

        protected override T CreateOriginUnitData<T>(IJCombatUnit unit)
        {
            return new TiktokJCombatUnitData
            {
                Uid = unit.Uid,
                Seat = seatQuery.GetSeat(unit.Uid),
                SamuraiBusinessId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SamuraiBusinessId,
                SoldierBusinessId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SoldierBusinessId,
                Actions = GetActions(unit),
                CurHp = ((unit as IJCombatTurnBasedUnit).GetOriginAttribute(TiktokAttributesType.Hp.ToString()) as GameAttributeInt).CurValue,
                MaxHp = ((unit as IJCombatTurnBasedUnit).GetOriginAttribute(TiktokAttributesType.MaxHp.ToString()) as GameAttributeInt).MaxValue,
            } as T;
        }

        protected override T CreateUnitData<T>(IJCombatUnit unit)
        {
            return new TiktokJCombatUnitData
            {
                Uid = unit.Uid,
                Seat = seatQuery.GetSeat(unit.Uid),
                SamuraiBusinessId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SamuraiBusinessId,
                SoldierBusinessId = ((unit as IJCombatTurnBasedUnit).GetUnitInfo() as TiktokJCombatUnitInfo).SoldierBusinessId,
                Actions = GetActions(unit),
                CurHp = ((unit as IJCombatTurnBasedUnit).GetAttribute(TiktokAttributesType.Hp.ToString()) as GameAttributeInt).CurValue,
                MaxHp = ((unit as IJCombatTurnBasedUnit).GetAttribute(TiktokAttributesType.MaxHp.ToString()) as GameAttributeInt).MaxValue,
            } as T;
        }



        List<KeyValuePair<string, string>> GetActions(IJCombatUnit unit)
        {
            var result = new List<KeyValuePair<string, string>>();

            var actionInfos = (unit as IJCombatTurnBasedUnit).GetActionInfos();

            var tiktokActionInfos = actionInfos.OfType<TiktokJCombatActionInfo>().ToList();

            foreach(var actionInfo in tiktokActionInfos)
            {
                result.Add(new KeyValuePair<string, string>(actionInfo.Uid, actionInfo.ActionBusinessId));
            }

            return result;
        }
    }

    public class TiktokJCombatActionInfo : IJCombatAcionInfo
    {
        public string? Uid { get ; set ; }
        public string? ActionBusinessId { get; set; }
        public List<IJCombatTrigger>? Triggers { get; set; }
        public List<IJCombatExecutor>? Executors { get ; set ; }
        public IJCombatTargetsFinder? Finder { get ; set ; }
    }
}


