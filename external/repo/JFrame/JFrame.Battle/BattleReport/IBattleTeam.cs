using System;
using System.Collections.Generic;

namespace JFramework
{
    public interface IBattleTeam
    {
        //event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, List<IBattleUnit>> onActionTriggerOn;
        event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, List<IBattleUnit>,float> onActionCast;
        //event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit> onActionDone;

        event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo> onDamage;
        event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, int> onHeal;
        event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit> onDead;
        event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, int> onReborn;
        event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, int> onMaxHpUp;
        event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit, int> onDebuffAnti;

        event Action<PVPBattleManager.Team, IBattleUnit, IBuffer> onBufferAdded;
        event Action<PVPBattleManager.Team, IBattleUnit, IBuffer> onBufferRemoved;
        event Action<PVPBattleManager.Team, IBattleUnit, IBuffer> onBufferCast;

        void Initialize();

        IBattleUnit GetUnit(BattlePoint point);

        void AddUnit(BattlePoint point, IBattleUnit unit);

        List<IBattleUnit> GetUnits();

        int GetUnitCount();

        bool IsAllDead();

        void Update(CombatFrame frame);

        PVPBattleManager.Team Team { get; } 
    }
}