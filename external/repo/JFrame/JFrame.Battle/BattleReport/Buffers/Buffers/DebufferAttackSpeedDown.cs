using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// 攻击速度下降（只对普通攻击生效）： arg[1] ：百分比
    /// </summary>
    public class DebufferAttackSpeedDown : BufferAttackSpeedUp
    {
        public DebufferAttackSpeedDown(IBattleUnit caster,bool isBuff,int buffType, string UID, int id, int foldCount, float[] args, IBattleTrigger trigger, IBattleTargetFinder finder, List<IBattleExecutor> exutors) : base(caster, isBuff, buffType, UID, id, foldCount, args, trigger,finder,exutors)
        {
            if (args.Length < 2)
                throw new System.Exception("DebufferAttackSpeedDown 参数不能少于2个");
        }

        protected override float CalcCD(float originValue)
        {
            var x = (1 - GetValue());
            if (x == 0) 
                throw new System.Exception("减少攻速公式分母不能为0 ");

            return originValue * (1 + GetValue());
        }
    }
}


