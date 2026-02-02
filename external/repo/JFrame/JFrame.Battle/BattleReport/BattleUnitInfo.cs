using System.Collections.Generic;

namespace JFramework
{
    public struct BattleUnitInfo
    {
        public string uid;
        public int id;
        public List<int> actionsId;
        public int hp;
        public int maxHp;
        public int atk;
        public float atkSpeed; 
        public float cri; //暴击率 0~1的值 百分比
        public float criDmgRate; //暴击伤害加成百分比
        public float criDmgAnti; //暴击伤害抵抗百分比
        public float skillDmgRate; //技能伤害加成百分比
        public float skillDmgAnti; //技能伤害抵抗百分比
        public float dmgRate; //伤害加成百分比
        public float dmgAnti; //伤害抵抗百分比
        public float debuffHit; //0~1异常状态命中百分比
        public float debuffAnti; //0~1异常状态抵抗百分比
        public float penetrate; //穿透 0~1 百分比
        public float block;     //格挡 0~1 百分比
        
    }

    public struct BattleUnitAttribute
    {
        public int hp;
        public int maxHp;
        public int atk;
        public float atkSpeed;
        public float cri; //暴击率 0~1的值 百分比
        public float criDmgRate; //暴击伤害加成百分比
        public float criDmgAnti; //暴击伤害抵抗百分比
        public float skillDmgRate; //技能伤害加成百分比
        public float skillDmgAnti; //技能伤害抵抗百分比
        public float dmgRate; //伤害加成百分比
        public float dmgAnti; //伤害抵抗百分比
        public float debuffHit; //0~1异常状态命中百分比
        public float debuffAnti; //0~1异常状态抵抗百分比
        public float penetrate; //穿透 0~1 百分比
        public float block;     //格挡 0~1 百分比
    }
}