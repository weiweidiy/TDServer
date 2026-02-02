using System.Collections.Generic;

namespace JFramework
{
    public enum CombatAttribute
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

    public class CombatAttributeFactory
    {
        /// <summary>
        /// 創建所有屬性對象
        /// </summary>
        /// <param name="unitInfo"></param>
        /// <returns></returns>
        public List<IUpdateable> CreateAllAttributes(CombatUnitInfo unitInfo)
        {
            var result = new List<IUpdateable>();
            var hp = new CombatAttributeDouble(CombatAttribute.CurHp.ToString(), unitInfo.hp, double.MaxValue);
            var atk = new CombatAttributeDouble(CombatAttribute.ATK.ToString(), unitInfo.atk, double.MaxValue);
            var maxHp = new CombatAttributeDouble(CombatAttribute.MaxHP.ToString(), unitInfo.maxHp, double.MaxValue);

            var bpDamage = new CombatAttributeDouble(CombatAttribute.BPDamage.ToString(), unitInfo.bpDamage, double.MaxValue);
            var bpDamageAnit = new CombatAttributeDouble(CombatAttribute.BPDamageAnit.ToString(), unitInfo.bpDamageAnti, double.MaxValue);

            var missile = new CombatAttributeDouble(CombatAttribute.MissileRate.ToString(), unitInfo.missileRate, double.MaxValue);

            var cri = new CombatAttributeDouble(CombatAttribute.Critical.ToString(), unitInfo.cri, double.MaxValue); //暴击率
            var criAnti = new CombatAttributeDouble(CombatAttribute.CriticalAnti.ToString(), unitInfo.criAnti, double.MaxValue);
            var criDmgRate = new CombatAttributeDouble(CombatAttribute.CriticalDamage.ToString(), unitInfo.criDamage, double.MaxValue);

            var damageAdvance = new CombatAttributeDouble(CombatAttribute.DamageAdvance.ToString(), unitInfo.damageAdvance, double.MaxValue);
            var damageAnti = new CombatAttributeDouble(CombatAttribute.DamageAnti.ToString(), unitInfo.damageAnti, double.MaxValue);
            var controlHit = new CombatAttributeDouble(CombatAttribute.ControlHit.ToString(), unitInfo.controlHit, double.MaxValue);
            var controlAnti = new CombatAttributeDouble(CombatAttribute.ControlAnti.ToString(), unitInfo.controlAnti, double.MaxValue);
            var hit = new CombatAttributeDouble(CombatAttribute.Hit.ToString(), unitInfo.hit, double.MaxValue);
            var dodge = new CombatAttributeDouble(CombatAttribute.Dodge.ToString(), unitInfo.dodge, double.MaxValue);

            var monsterAdd = new CombatAttributeDouble(CombatAttribute.MonsterAdd.ToString(), unitInfo.monsterAdd, double.MaxValue);
            var bossAdd = new CombatAttributeDouble(CombatAttribute.BossAdd.ToString(), unitInfo.bossAdd, double.MaxValue);
            var hpRecover = new CombatAttributeDouble(CombatAttribute.HpRecover.ToString(), unitInfo.hpRecover, double.MaxValue);

            var fightBackCoef = new CombatAttributeDouble(CombatAttribute.FightBackCoef.ToString(), unitInfo.fightBackCoef, double.MaxValue);
            var hpSteal = new CombatAttributeDouble(CombatAttribute.HpSteal.ToString(), unitInfo.hpSteal, double.MaxValue);

            var elemt = new CombatAttributeDouble(CombatAttribute.Elemt.ToString(), unitInfo.elemt, double.MaxValue);
            var elemtResist = new CombatAttributeDouble(CombatAttribute.ElemtResist.ToString(), unitInfo.elemtResist, double.MaxValue);

            //to do : 其他屬性
            result.Add(hp);
            result.Add(atk);
            result.Add(maxHp);
            result.Add(bpDamage);
            result.Add(bpDamageAnit);
            result.Add(missile);
            result.Add(cri);
            result.Add(criAnti);
            result.Add(criDmgRate);
            result.Add(damageAdvance);
            result.Add(damageAnti);
            result.Add(controlHit);
            result.Add(controlAnti);
            result.Add(hit);
            result.Add(dodge);
            result.Add(monsterAdd);
            result.Add(bossAdd);
            result.Add(hpRecover);
            result.Add(fightBackCoef);
            result.Add(hpSteal);
            result.Add(elemt);
            result.Add(elemtResist);

            return result;
        }
    }
}