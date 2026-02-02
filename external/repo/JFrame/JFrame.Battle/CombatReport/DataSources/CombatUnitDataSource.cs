namespace JFramework
{
    //    ATK = 101,
    //MaxHP = 102,
    //CurHp = 103,
    //MoveSpeed = 104,
    //Critical = 206, //暴击率
    //CriticalAnti = 207, //暴击抵抗
    //CriticalDamage = 208,
    //Cd = 209, //cd 加成
    //ControlHit = 212,
    //ControlAnti = 213,
    //Hit = 216,
    //Dodge = 217,
    /// <summary>
    /// 战斗单位数据源
    /// </summary>
    public abstract class CombatUnitDataSource : CombatActionDataSource
    {
        public abstract string GetUid();
        public abstract int GetId();

        public abstract int GetLevel();
        public abstract UnitMainType GetUnitMainType();
        public abstract UnitSubType GetUnitSubType();
        public abstract long GetHp();
        public abstract long GetMaxHp();
        public abstract long GetAtk();
        public abstract float GetBpDamage();
        public abstract float GetBpDamageAnti();
        public abstract float GetMissileRate();
        public abstract float GetCri();
        public abstract float GetCriAnti();
        public abstract float GetCriDamage();
        public abstract float GetDamageAdvance();
        public abstract float GetDamageAnti();
        public abstract float GetControlHit();
        public abstract float GetControlAnti();
        public abstract float GetHit();
        public abstract float GetDodge();
        public abstract float GetMonsterAdd();
        public abstract float GetBossAdd();
        public abstract float GetHpRecover();
        public abstract float GetFightBackCoef();
        public abstract float GetHpSteal();
        public abstract float GetElemt();
        public abstract float GetElemtResist();
        public abstract CombatVector GetPosition();
        public abstract CombatVector GetVelocity();
        public abstract CombatVector GetTargetPosition();

    }
}