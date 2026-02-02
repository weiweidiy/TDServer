using System;
using System.Collections.Generic;

namespace JFramework
{
    public enum CombatValueType
    {
        None,
        Damage,
        TurnBackDamage, //反射

        Heal = 50,

        FireDamage = 100, //灼烧

    }
    /// <summary>
    /// 透传参数
    /// </summary>
    public class CombatExtraData : ICloneable
    {
        public string Uid { get; set; }
        /// <summary>
        /// 屬性唯一id
        /// </summary>
        public CombatValueType ValueType { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// 额外参数
        /// </summary>
        public double ExtraArg { get; set; }

        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool IsCri { get; set; }

        /// <summary>
        /// 是否miss
        /// </summary>
        public bool IsMiss { get; set; }

        /// <summary>
        /// 是否格挡
        /// </summary>
        public bool IsBlock { get; set; }

        /// <summary>
        /// 是否抵消
        /// </summary>
        public bool IsGuard { get; set; }
        
        /// <summary>
        /// 是否免疫
        /// </summary>
        public bool IsImmunity { get; set; }

        /// <summary>
        /// 持有者
        /// </summary>
        public virtual CombatUnit Owner { get; set; }

        /// <summary>
        /// 释放者
        /// </summary>
        public virtual CombatUnit Caster { get; set; }

        /// <summary>
        /// 哪個aciton造成的
        /// </summary>
        public CombatAction Action { get; set; }

        /// <summary>
        /// 目标单位
        /// </summary>
        public virtual List<CombatUnit> Targets { get; set; }

        /// <summary>
        /// 单一目标
        /// </summary>
        public CombatUnit Target { get; set; }

        /// <summary>
        /// 目标技能，用于修改他们技能参数
        /// </summary>
        public List<CombatAction> TargetActions{  get; set; }

        /// <summary>
        /// 释放时长
        /// </summary>
        public float CastDuration { get; set; } 

        /// <summary>
        /// cd时长
        /// </summary>
        public float CdDuration { get; set; }

        /// <summary>
        /// 控制时间
        /// </summary>
        public float ControlDuration { get;set; }

        /// <summary>
        /// 移动速度
        /// </summary>
        public CombatVector Velocity { get; set; }  

        /// <summary>
        /// buffer
        /// </summary>
        public CombatBuffer Buffer { get; set; }

        /// <summary>
        /// 层数，默认1
        /// </summary>
        public int FoldCount { get; set; }

        /// <summary>
        /// 技能发射次数
        /// </summary>
        public int ShootCount { get; set; }


        public CombatExtraData()
        {
            TargetActions = new List<CombatAction>();
        }

        /// <summary>
        /// 浅拷贝
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public List<string> GetTargetsUid()
        {
            var result = new List<string>();

            if (Targets == null)
                return result;

            foreach(var target in Targets)
            {
                result.Add(target.Uid);
            }

            return result;
        }

        public virtual string GetActionUid()
        {
            return Action.Uid;
        }
    }




}