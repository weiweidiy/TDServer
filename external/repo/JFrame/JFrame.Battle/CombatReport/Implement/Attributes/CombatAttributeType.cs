namespace JFramework.Combat
{
    public enum CombatAttributeType
    {
        ATK = 101,
        MaxHP = 102,
        CurHp = 103,
        MoveSpeed = 104,

        BPDamage = 202, //臂炮伤害加成
        BPDamageAnit = 203,

        MissileRate = 205, //飞弹连射概率

        Critical = 206, //暴击率
        CriticalAnti = 207, //暴击抵抗
        CriticalDamage = 208,
        Cd = 209, //cd 加成
        ControlHit = 212,
        ControlAnti = 213,

        DamageAdvance = 214, //伤害加成
        DamageAnti = 215,    //伤害抵抗

        Hit = 216,
        Dodge = 217,
        FightBackCoef = 220, // 反伤系数
        HpRecover = 221, // 血量恢复效率
        Elemt = 222, // 元素强化
        ElemtResist = 223, // 元素抗性
        MonsterAdd = 224, // 对小怪伤害加成
        BossAdd = 225, // 对BOSS伤害加成
        HpSteal = 226, // 吸血
        CounterAnti = 227, //反击概率抵抗
        FightBackAnti = 228, //反伤系数减免
    }
}