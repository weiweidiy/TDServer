using System.Collections.Generic;

namespace JFramework
{
    /// <summary>
    /// type8 參數0：actionGroupId 参数1：sortID  参数2: 概率
    /// </summary>
    public class TriggerActionStartCd : CombatBaseTrigger
    {
        List<CombatUnit> unitList = new List<CombatUnit>();

        Utility utility = new Utility();

        public TriggerActionStartCd(List<CombatBaseFinder> finders) : base(finders)
        {
        }


        public override int GetValidArgsCount()
        {
            return 3;
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
                        target.onActionStartCD += Target_onActionStartCD; 
                        unitList.Add(target);
                    }
                }
            }
            else
            {
                ExtraData.Owner.onActionStartCD += Target_onActionStartCD;
                unitList.Add(ExtraData.Owner);
            }
        }

        public override void OnExitState()
        {
            base.OnExitState();

            foreach (var target in unitList)
            {
                target.onActionStartCD -= Target_onActionStartCD;
            }
        }

        private void Target_onActionStartCD(CombatExtraData extraData)
        {
            if (extraData.Action.Uid == ExtraData.Action.Uid)
                return;

            if (!utility.RandomHit(GetRandomArg() * 100))
                return;

            if (GetGroupIdArg() != 0 && extraData.Action.GroupId != GetGroupIdArg())
                return;

            if (GetSortIdArg() != 0 && extraData.Action.SortId != GetSortIdArg())
                return;

            if (finders != null && finders.Count > 1)
            {
                var finder = finders[1];
                var targets = finder.FindTargets(ExtraData);
                targets = Filter(targets);
                if (targets != null && targets.Count > 0)
                {
                    ExtraData.Targets = targets;
                    ExtraData.Target = targets[0];
                    

                }
                else
                {
                    var lst = new List<CombatUnit>();
                    if (extraData.Targets != null)
                        lst.AddRange(extraData.Targets);

                    ExtraData.Targets = lst;

                    if (extraData.Target != null)
                        ExtraData.Target = extraData.Target;

                }

            }
            else
            {
                var lst = new List<CombatUnit>();
                if (extraData.Targets != null)
                    lst.AddRange(extraData.Targets);

                ExtraData.Targets = lst;

                if (extraData.Target != null)
                    ExtraData.Target = extraData.Target;

            }


            if (!ExtraData.TargetActions.Contains(extraData.Action))
                ExtraData.TargetActions.Add(extraData.Action);

            SetOn(true);
        }
    }
}