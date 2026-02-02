using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type 7 參數0：actionGroupId 参数1：sortid:  参数2: 概率  参数3：hp小于百分比  参数4：数值类型  参数5：倍率
    /// </summary>
    public class TriggerActionHitting : CombatBaseTrigger
    {
        List<CombatUnit> unitList = new List<CombatUnit>();

        Utility utility = new Utility();

        public TriggerActionHitting(List<CombatBaseFinder> finders) : base(finders)
        {
        }

        public override int GetValidArgsCount()
        {
            return 6;
        }

        protected int GetGroupIdArg()
        {
            return (int)GetCurArg(0);
        }

        protected int GetSortIdArg()
        {
            return (int)GetCurArg(1);
        }

        protected float GetRandomArg()
        {
            return GetCurArg(2);
        }

        protected float GetHpLessPercentArg()
        {
            return GetCurArg(3);
        }

        protected int GetValueTypeArg()
        {
            return (int)GetCurArg(4);
        }

        protected float GetValueRateArg()
        {
            return GetCurArg(5);
        }

        public override void OnEnterState()
        {
            base.OnEnterState();
            unitList.Clear();
            if (finders != null && finders.Count > 0)
            {
                var finder = finders[0];

                var targets = finder.FindTargets(ExtraData); //获取目标
                targets = Filter(targets);
                if (targets != null && targets.Count > 0)
                {
                    foreach (var target in targets)
                    {
                        target.onHittingTarget += Target_onHittingTarget;
                        unitList.Add(target);
                    }
                }
            }
            else
            {
                ExtraData.Owner.onHittingTarget += Target_onHittingTarget;
                unitList.Add(ExtraData.Owner);
            }
        }

        private void Target_onHittingTarget(CombatExtraData extraData)
        {
            if (extraData.Action.Uid == ExtraData.Action.Uid)
                return;

            if (!utility.RandomHit(GetRandomArg() * 100))
                return;

            if (extraData.Target.GetHpPercent() > GetHpLessPercentArg())
                return;

            if (GetGroupIdArg() != 0 && extraData.Action.GroupId != GetGroupIdArg())
                return;

            if (GetSortIdArg() != 0 && extraData.Action.SortId != GetSortIdArg())
                return;

            if (extraData.ValueType != (CombatValueType)GetValueTypeArg())
                return;


            if (finders != null && finders.Count > 1)
            {
                var finder = finders[1];
                var targets = finder.FindTargets(ExtraData);
                targets = Filter(targets);
                if (targets != null && targets.Count > 0)
                {
                    ExtraData.Value = extraData.Value;
                    ExtraData.Targets = targets;
                    ExtraData.Target = targets[0];

                    ExtraData.Value *= GetValueRateArg();
                }
                else
                {
                    var lst = new List<CombatUnit>();
                    if (extraData.Targets != null)
                        lst.AddRange(extraData.Targets);

                    ExtraData.Value = extraData.Value;
                    ExtraData.Targets = lst;

                    if (extraData.Target != null)
                        ExtraData.Target = extraData.Target;

                    extraData.Value *= GetValueRateArg();
                }

            }
            else
            {
                var lst = new List<CombatUnit>();
                if (extraData.Targets != null)
                    lst.AddRange(extraData.Targets);

                ExtraData.Value = extraData.Value;
                ExtraData.Targets = lst;

                if (extraData.Target != null)
                    ExtraData.Target = extraData.Target;

                extraData.Value *= GetValueRateArg();
            }

            //这个触发器不会继续执行后面的执行器
            //SetOn(true);
        }



        public override void OnExitState()
        {
            base.OnExitState();

            foreach (var target in unitList)
            {
                target.onHittingTarget -= Target_onHittingTarget;
            }

            unitList.Clear();
        }

        public override void OnStop()
        {
            base.OnStop();

            foreach (var target in unitList)
            {
                target.onHittingTarget -= Target_onHittingTarget;
            }

            unitList.Clear();
        }
    }
}