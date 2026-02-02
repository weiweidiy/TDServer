using System.Collections.Generic;

namespace JFramework
{

    /// <summary>
    ///  type 1 查找触发器，只要查找器找到了对象，就触发
    /// </summary>
    public class TriggerFinder : CombatBaseTrigger
    {
        public override int GetValidArgsCount()
        {
            return 0;
        }


        public TriggerFinder(List<CombatBaseFinder> finders) : base(finders)
        {
        }

        protected override void OnUpdate(CombatFrame frame)
        {
            base.OnUpdate(frame);

            if(finders != null && finders.Count > 0)
            {
                foreach(var finder in finders)
                {
                    var targets = finder.FindTargets(ExtraData); //获取目标
                    targets = Filter(targets);
                    if (targets != null && targets.Count > 0)
                    {
                        _extraData.Targets = targets;  //会替换成后面那个finder
                        SetOn(true);
                    }
                }
            }
        }


    }
}