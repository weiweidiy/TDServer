
using System.Collections.Generic;
using System.Diagnostics;

namespace JFramework
{
    /// <summary>
    /// type 13 参数1：类型 0 直接设置，1 乘法 2 减法  参数2：值
    /// </summary>
    public class ExecutorChangeCDArgs : ExecutorNormal
    {
        int type = 0; //0:直接设置 1：百分比：2：加减法
        float value = 0;
        float[] newArgs;
        public ExecutorChangeCDArgs(FormulaManager formulaManager, float[] args) : base(formulaManager, args)
        {
            if (args != null && args.Length >= 2)
            {
                type = (int)args[0];
                value = args[1];
                var newArgsLength = args.Length - 2;
                if(newArgsLength > 0)
                {
                    newArgs = new float[newArgsLength];
                    for(int i = 2; i < args.Length; i++)
                    {
                        newArgs[i-2] = args[i];
                    }
                }
            }
            else
            {
                throw new System.Exception(this.GetType().ToString() + " 参数数量不对");
            }
        }

        public override void Hit(IBattleUnit caster, IBattleAction action, List<IBattleUnit> target, object[] arg = null)
        {
            IBattleAction targetAction = arg[0] as IBattleAction;
            if (targetAction == null)
                throw new System.Exception("ExecutorChangeCDArgs 参数转换失败");

            if(type == 0) //直接设置值
            {
                targetAction.SetCdArgs(newArgs);
            }
            else if(type == 1) //百分比
            {
                var oldArgs = targetAction.GetCDTrigger().GetArgs();
                if (oldArgs.Length == 0) throw new System.Exception("目标action的cdtrigger参数长度为0");

                oldArgs[0] = oldArgs[0] * value;
                targetAction.SetCdArgs(oldArgs);
            }
            else //加减法
            {
                var oldArgs = targetAction.GetCDTrigger().GetArgs();
                if (oldArgs.Length == 0) throw new System.Exception("目标action的cdtrigger参数长度为0");

                oldArgs[0] = oldArgs[0] - value;
                targetAction.SetCdArgs(oldArgs);
            }

        }


    }
}


