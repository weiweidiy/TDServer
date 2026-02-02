using System;
using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 战斗单元接口
    /// </summary>
    public interface IBattleUnit : IUnique
    {
        /// <summary>
        /// 行动时主动事件
        /// </summary>
        //event Action<IBattleUnit, IBattleAction, List<IBattleUnit>> onActionTriggerOn;
        event Action<IBattleUnit, IBattleAction, List<IBattleUnit>, float> onActionCast; //执行效果之前，只有首目标
        event Action<IBattleUnit, IBattleAction, float> onActionStartCD;
        event Action<IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo> onHittingTarget; //动作命中对方,一个目标1次调用

        /// <summary>
        /// 被动事件
        /// </summary>
        event Action<IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo> onDamaging; //即将受到伤害
        event Action<IBattleUnit, IBattleAction, IBattleUnit, ExecuteInfo> onDamaged; //受到伤害之后
        
        event Action<IBattleUnit, IBattleAction, IBattleUnit, int> onHealed;        //回血
        event Action<IBattleUnit, IBattleAction, IBattleUnit> onDead;        //死亡
        event Action<IBattleUnit, IBattleAction, IBattleUnit, int> onRebord;        //复活
        event Action<IBattleUnit, IBattleAction, IBattleUnit, int> onMaxHpUp;
        event Action<IBattleUnit, IBattleAction, IBattleUnit, int> onDebuffAnti;    //状态抵抗

        event Action<IBattleUnit, int, ExecuteInfo> onBufferAdding; //即将添加buff
        event Action<IBattleUnit, IBuffer> onBufferAdded;
        event Action<IBattleUnit, IBuffer> onBufferRemoved;
        event Action<IBattleUnit, IBuffer> onBufferCast;
        event Action<IBattleUnit, IBuffer, int, float[]> onBufferUpdate;

        void Update(CombatFrame frame);

       // string Uid { get; }

        /// <summary>
        /// 名字
        /// </summary>
        string Name { get; }

        void Initialize();

        #region 属性
        /// <summary>
        /// 当前攻击力
        /// </summary>
        int Atk { get;  }

        /// <summary>
        /// 攻击力提升，返回实际提升的值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int AtkUpgrade(int value);
        /// <summary>
        /// 攻击力降低
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int AtkReduce(int value);

        /// <summary>
        /// 攻击速度
        /// </summary>
        float AtkSpeed { get; set; } 

        /// <summary>
        /// 当前生命值
        /// </summary>
        int HP { get; }

        /// <summary>
        /// 最大生命值
        /// </summary>
        int MaxHP { get; }

        /// <summary>
        /// 最大生命提升
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int MaxHPUpgrade(int value);
        int MaxHPReduce(int value);

        /// <summary>
        /// 暴击率 0~1的值 百分比
        /// </summary>
        float Critical { get; }

        /// <summary>
        /// 暴击率提升
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float CriUpgrade(float value);
        /// <summary>
        /// 暴击率降低
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float CriReduce(float value);

        /// <summary>
        /// //暴击伤害加成百分比
        /// </summary>
        float CriticalDamage { get; }
        float CriticalDamageUpgrade(float value);
        float CriticalDamageReduce(float value);

        /// <summary>
        /// //暴击伤害抵抗百分比
        /// </summary>
        float CriticalDamageResist { get; }
        float CriticalDamageResistUpgrade(float value);
        float CriticalDamageResistReduce(float value);
        /// <summary>
        /// //技能伤害加成百分比
        /// </summary>
        float SkillDamageEnhance { get; }
        float SkillDamageEnhanceUpgrade(float value);
        float SkillDamageEnhanceReduce(float value);
        /// <summary>
        /// //技能伤害抵抗百分比
        /// </summary>
        float SkillDamageReduce { get; }
        float SkillDamageReduceUpgrade(float value);
        float SkillDamageReduceReduce(float value);

        /// <summary>
        /// //伤害加成百分比
        /// </summary>
        float DamageEnhance { get; }
        float DamageEnhanceUpgrade(float value);
        float DamageEnhanceReduce(float value);
        /// <summary>
        /// //伤害抵抗百分比
        /// </summary>
        float DamageReduce { get; }
        float DamageReduceUpgrade(float value);
        float DamageReduceReduce(float value);
        /// <summary>
        /// //0~1异常状态命中百分比
        /// </summary>
        float ControlHit { get; }
        float ControlHitUpgrade(float value);
        float ControlHitReduce(float value);
        /// <summary>
        /// //0~1异常状态抵抗百分比
        /// </summary>
        float ControlResistance { get; }
        /// <summary>
        /// 抵抗数值提升
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float ControlResistanceUpgrade(float value);
        /// <summary>
        /// 抵抗数值降低
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float ControlResistanceReduce(float value);
        /// <summary>
        /// //穿透 0~1 百分比
        /// </summary>
        float Puncture { get; }
        float PunctureUpgrade(float value);
        float PunctureReduce(float value);

        /// <summary>
        ///  //格挡 0~1 百分比
        /// </summary>
        float Block { get; }
        float BlockUpgrade(float value);
        float BlockReduce(float value);



        #endregion

        #region 回调
        /// <summary>
        /// 受到伤害了
        /// </summary>
        /// <param name="damage"></param>
        void OnDamage(IBattleUnit caster, IBattleAction action, ExecuteInfo damage);

        /// <summary>
        /// 受到治疗了
        /// </summary>
        /// <param name="heal"></param>
        void OnHeal(IBattleUnit caster, IBattleAction action, ExecuteInfo heal);

        /// <summary>
        /// 复活了
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="heal"></param>
        void OnReborn(IBattleUnit caster, IBattleAction action, ExecuteInfo heal);

        /// <summary>
        /// 生命上限增加
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="value"></param>
        void OnMaxHpUp(IBattleUnit caster, IBattleAction action, ExecuteInfo hp);

        /// <summary>
        /// 抵抗控制
        /// </summary>
        void OnDebuffAnti(IBattleUnit caster, IBattleAction action, int debuffId);

        /// <summary>
        /// 眩晕
        /// </summary>
        /// <param name="caster"></param>
        /// <param name="action"></param>
        /// <param name="duration"></param>
        void OnStunning(ActionType actionType, float duration);

        /// <summary>
        /// 眩晕恢复
        /// </summary>
        void OnResumeFromStunning(ActionType actionType);

        #endregion

        /// <summary>
        /// 是否活着
        /// </summary>
        /// <returns></returns>
        bool IsAlive();

        /// <summary>
        /// 是否满血
        /// </summary>
        /// <returns></returns>
        bool IsHpFull();

        /// <summary>
        /// 添加buffer
        /// </summary>
        /// <param name="bufferId"></param>
        /// <param name="foldCout"></param>
        /// <returns></returns>
        IBuffer AddBuffer( IBattleUnit caster, int bufferId, int foldCout = 1);

        /// <summary>
        /// 获取所有buffers
        /// </summary>
        /// <returns></returns>
        IBuffer[] GetBuffers();

        /// <summary>
        /// 是否是增益
        /// </summary>
        /// <param name="bufferId"></param>
        /// <returns></returns>
        bool IsBuffer(int bufferId);


        /// <summary>
        /// 移除buffer
        /// </summary>
        /// <param name="bufferId"></param>
        void RemoveBuffer(string bufferUID);

        /// <summary>
        /// 获取所有技能
        /// </summary>
        /// <returns></returns>
        IBattleAction[] GetActions();

        /// <summary>
        /// 获取所有指定类型动作
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IBattleAction[] GetActions(ActionType type);

        /// <summary>
        /// 获取指定技能动作
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        IBattleAction GetAction(int actionId);

    }
}

