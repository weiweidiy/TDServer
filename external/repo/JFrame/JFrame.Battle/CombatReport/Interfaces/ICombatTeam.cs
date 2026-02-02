using System;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;

namespace JFramework
{
    public interface ICombatTeam<TUnit>
    {

        //event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, List<IBattleUnit>> onActionTriggerOn;
        /// <summary>
        /// int : teamId , float : duration
        /// </summary>
        //event Action<int, TUnit, TAction, List<TUnit>, float> onActionCast;
        ////event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit> onActionDone;
        //event Action<int, TUnit, TAction, float> onActionStartCD;

        //event Action<int, CombatExtraData> onDamage;
        //event Action<int, TUnit, TAction, TUnit, int> onHeal;
        //event Action<int, CombatExtraData> onDead;
        //event Action<int, TUnit, TAction, TUnit, int> onReborn;
        //event Action<int, TUnit, TAction, TUnit, int> onMaxHpUp;
        //event Action<int, TUnit, TAction, TUnit, int> onDebuffAnti;
        //event Action<int, TUnit, TBuffer> onBufferAdded;
        //event Action<int, TUnit, TBuffer> onBufferRemoved;
        //event Action<int, TUnit, TBuffer> onBufferCast;
        //event Action<int, TUnit, TBuffer, int, float[]> onBufferUpdate;

        event Action<int, CombatExtraData> onActionCast;
        //event Action<PVPBattleManager.Team, IBattleUnit, IBattleAction, IBattleUnit> onActionDone;
        event Action<int, CombatExtraData> onActionStartCD;
        event Action<int, CombatExtraData> onDamage;
        event Action<int, CombatExtraData> onHeal;
        event Action<int, CombatExtraData> onDead;
        event Action<int, CombatExtraData> onReborn;
        event Action<int, CombatExtraData> onMaxHpUp;
        event Action<int, CombatExtraData> onDebuffAnti;
        event Action<int, CombatExtraData> onBufferAdded;
        event Action<int, CombatExtraData> onBufferRemoved;
        event Action<int, CombatExtraData> onBufferCast;
        event Action<int, CombatExtraData> onBufferUpdate;
        event Action<int, CombatExtraData> onActionCdChange;


        int TeamId { get; }

        TUnit GetUnit(string uid);

        void AddUnit(TUnit unit);
        void RemoveUnit(TUnit unit);

        List<TUnit> GetUnits(bool mainTarget);

        int GetUnitCount();

        bool IsAllDead();

        //void Update(BattleFrame frame);

    }
}