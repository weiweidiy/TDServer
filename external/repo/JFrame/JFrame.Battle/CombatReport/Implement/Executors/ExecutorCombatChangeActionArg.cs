using System;

namespace JFramework
{
    /// <summary>
    /// type 9 更改action参数 参数0：执行周期， 参数1：组件类型（0:conditionFinder, 1:conditionTrigger，2：delayTrigger, 3:executorFinder , 4,formula, 5: executor, 6: cdTrigger）  参数2：组件索引    参数3： 参数索引   参数4： 加成值 参数5：计算模式（0：加法，1乘法)
    /// </summary>
    public class ExecutorCombatChangeActionArg : ExecutorCombatNormal
    {
        public ExecutorCombatChangeActionArg(CombatBaseFinder combinFinder, CombatBaseFormula formula) : base(combinFinder, formula)
        {
        }

        public override int GetValidArgsCount()
        {
            return 6;
        }

        protected int GetComponentType()
        {
            return (int)GetCurArg(1);
        }

        protected int GetComponentIndex()
        {
            return (int)GetCurArg(2);
        }

        protected int GetComponentArgIndex()
        {
            return (int)GetCurArg(3);
        }

        protected float GetComponentArgValue() //加成值
        {
            return GetCurArg(4);
        }


        protected int GetComponentCalMode()
        {
            return (int)GetCurArg(5);
        }
        protected override void SetValueType(CombatExtraData data)
        {
            data.ValueType = CombatValueType.None;
        }

        //这个值
        protected override double GetExecutorValue()
        {
            return 0;
        }

        protected override void DoHit(CombatUnit target, CombatExtraData data)
        {
            var componentType = GetComponentType();
            var componentIndex = GetComponentIndex();
            var componentArgIndex = GetComponentArgIndex();
            var componentArgValue = GetComponentArgValue() * data.FoldCount;
            var calMode = GetComponentCalMode();

            var actions = data.TargetActions; //收集到的所有技能
            foreach (var action in actions)
            {
                if(action.GetCurState() != nameof(ActionCdingState))
                    continue;

                //改变指定组件类型（暂时只有cdtrigger)
                if ((ActionComponentType)componentType == ActionComponentType.CdTrigger)
                {
                    var originValue = action.GetCdTriggerArg(componentIndex, componentArgIndex);


                    if(calMode == 0) //加法
                    {
                        action.SetCdTriggerArg(componentIndex, componentArgIndex, componentArgValue + originValue);
                    }
                    else //乘法，减少就是0.9
                    {
                        var k = Math.Max(0, (1 + componentArgValue));
                        action.SetCdTriggerArg(componentIndex, componentArgIndex, k * originValue);
                    }
                   
                }
            }

            data.TargetActions.Clear();
        }

    }
}