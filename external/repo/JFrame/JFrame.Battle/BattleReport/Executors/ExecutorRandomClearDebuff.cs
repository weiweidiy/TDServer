
using System;
using System.Collections.Generic;
using System.Linq;

namespace JFramework
{
    /// <summary>
    /// type 16
    /// </summary>
    public class ExecutorRandomClearDebuff : ExecutorNormal
    {
        int count;
        public ExecutorRandomClearDebuff(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
            if (args.Length < 4)
                throw new System.Exception("ExecutorRandomClearDebuff 参数不能少于4个");

            count = (int)args[3];
        }

        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> targets, object[] args = null)
        {
            foreach(var target in targets)
            {
                var buffs = target.GetBuffers();
                var random = new Random();
                Func<IBuffer, bool> customCondition = i => !i.IsBuff();


                var result = buffs
                .Where(customCondition) // 应用自定义条件
                .OrderBy(i => random.Next()) // 随机排序
                .Take(count) // 取前三个
                                //.Distinct() // 去重
                .ToList(); // 转换为列表


                if(result != null)
                {
                    foreach( var debuff in result)
                    {
                        target.RemoveBuffer(debuff.Uid);
                    }
                }

            }
        }
    }
}


