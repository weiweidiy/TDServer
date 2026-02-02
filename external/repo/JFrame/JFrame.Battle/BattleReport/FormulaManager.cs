using System;

namespace JFramework
{
    /// <summary>
    /// 公式管理
    /// </summary>
    public class FormulaManager
    {
        /// <summary>
        /// 获取伤害数值
        /// </summary>
        /// <param name="hitter"></param>
        /// <param name="action"></param>
        /// <param name="actionArg">动作加成值</param>
        /// <param name="hittee"></param>
        /// <param name="isCri"></param>
        /// <param name="isBlock"></param>
        /// <returns></returns>
        public int GetNormalDamageValue(int baseValue, IBattleUnit hitter, IBattleAction action, IBattleUnit hittee, out bool isCri, out bool isBlock)
        {
            // 动作基础值 *【暴击部分： 1 + max( 暴击增伤-抗暴减免,0）】*（1 + max(伤害加成 -伤害减免 ,0））*【格挡部分：80%】
            // 动作基础值 *【暴击部分： 1 + max( 暴击增伤-抗暴减免,0）】*（1 + max(伤害加成 -伤害减免 ,0））*（1 + max(技能增伤 -技能减免,0）)*【控制抵抗】

            isCri = IsCri(hitter, action, hittee);
            float criDmage = isCri ? GetCriDmg(hitter, action, hittee) : 1f;
            float dmgRate = GetDmgRate(hitter, action, hittee);

            isBlock= IsBlock(hitter, action, hittee);
            float blockRate = isBlock ? 0.8f : 1f;

            return (int)(baseValue * criDmage * dmgRate * blockRate);
        }

        /// <summary>
        /// 技能伤害加成
        /// </summary>
        /// <param name="baseValue"></param>
        /// <param name="hitter"></param>
        /// <param name="action"></param>
        /// <param name="hittee"></param>
        /// <param name="isCri"></param>
        /// <returns></returns>
        public int GetSkillDamageValue(int baseValue, IBattleUnit hitter, IBattleAction action, IBattleUnit hittee, out bool isCri)
        {
            isCri = IsCri(hitter, action, hittee);
            float criDmage = isCri ? GetCriDmg(hitter, action, hittee) : 1f; //暴击伤害加成
            float dmgRate = GetDmgRate(hitter, action, hittee); //伤害加成
            float skillDmgRate = GetSkillDmgRate(hitter, action, hittee); //技能伤害加成

            return (int)(baseValue * criDmage * dmgRate * skillDmgRate);
        }



        /// <summary>
        /// 是否暴击
        /// </summary>
        /// <param name="hitter"></param>
        /// <param name="action"></param>
        /// <param name="hittee"></param>
        /// <returns></returns>
        bool IsCri(IBattleUnit hitter, IBattleAction action, IBattleUnit hittee)
        {
            var r = new Random().NextDouble();

            return r <= hitter.Critical;
        }

        /// <summary>
        /// 获取暴击伤害加成
        /// </summary>
        /// <param name="hitter"></param>
        /// <param name="action"></param>
        /// <param name="hittee"></param>
        /// <returns></returns>
        float GetCriDmg(IBattleUnit hitter, IBattleAction action, IBattleUnit hittee)
        {
            return 1 + Math.Max(hitter.CriticalDamage - hittee.CriticalDamageResist, 0);
        }

        /// <summary>
        /// 获取伤害加成
        /// </summary>
        /// <param name="hitter"></param>
        /// <param name="action"></param>
        /// <param name="hittee"></param>
        /// <returns></returns>
        float GetDmgRate(IBattleUnit hitter, IBattleAction action, IBattleUnit hittee)
        {
            return 1 + Math.Max(hitter.DamageEnhance - hittee.DamageReduce, -1);
        }

        /// <summary>
        /// 技能伤害加成
        /// </summary>
        /// <param name="hitter"></param>
        /// <param name="action"></param>
        /// <param name="hittee"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private float GetSkillDmgRate(IBattleUnit hitter, IBattleAction action, IBattleUnit hittee)
        {
            return 1 + Math.Max(hitter.SkillDamageEnhance - hittee.SkillDamageReduce, -1);
        }

        /// <summary>
        /// 是否被格挡
        /// </summary>
        /// <param name="hitter"></param>
        /// <param name="action"></param>
        /// <param name="hittee"></param>
        /// <returns></returns>
        bool IsBlock(IBattleUnit hitter, IBattleAction action, IBattleUnit hittee)
        {
            var r = new Random().NextDouble();
            var rate = Math.Min(1, Math.Max(hittee.Block - hitter.Puncture, 0));
            return r < rate;
        }

        /// <summary>
        /// 是否抵抗
        /// </summary>
        /// <param name="hitter"></param>
        /// <param name="action"></param>
        /// <param name="hittee"></param>
        /// <returns></returns>
        public bool IsDebuffAnti(IBattleUnit hitter, IBattleAction action, IBattleUnit hittee)
        {
            var rate = hittee.ControlResistance - hitter.ControlHit; //可能负数
            var r = new Random().NextDouble();
            return r < rate;
        }
    }
}