using System.Collections.Generic;
using static JFramework.PVPBattleManager;

namespace JFramework
{
    public interface IPVPBattleManager
    {
        void Initialize(Dictionary<BattlePoint, BattleUnitInfo> attacker, Dictionary<BattlePoint, BattleUnitInfo> defence
            , ActionDataSource dataSource, BufferDataSource bufferDataSource
            , IBattleReporter reporter, FormulaManager formulaManager, float battleDuration = 90
            , Dictionary<BattlePoint, BattleUnitInfo> global = null, ActionDataSource globalActionDataSource = null
            , IBattleNotifier[] notifiers = null
            , object extraData = null);

        void Release();

        BattleTeam CreateTeam(Team team, Dictionary<BattlePoint, BattleUnitInfo> units, ActionDataSource dataSource);

        void Update();

        void AddTeam(Team team, BattleTeam teamObj);

        List<IBattleUnit> GetUnits(Team team);

        Team GetOppoTeam(Team team);

        Team GetOppoTeam(IBattleUnit unit);

        Team GetFriendTeam(IBattleUnit unit);

        float GetBattleTimeLimit();

        bool IsBuffer(int buffId);

        object GetExtraData();

    }
}