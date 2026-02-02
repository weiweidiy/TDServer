using JFramework.Game;

namespace TiktokGame2Server.Others
{
    public class LevelNodeActionsBuilder : TiktokJCombatAcionsBaseBuilder
    {
        string formationUnitBusinessId;

        public LevelNodeActionsBuilder(string formationUnitBusinessId,  TiktokConfigManager tiktokConfigService, IJCombatContext context) :base(tiktokConfigService, context)
        {
            this.formationUnitBusinessId = formationUnitBusinessId;

        }

        protected override List<string> GetActionsBusiness()
        {
            return GetActionsBusiness(formationUnitBusinessId);
        }

        List<string> GetActionsBusiness(string formationUnitBusinessId)
        {
            var result = new List<string>();

            var level = tiktokConfigService.GetFormationUnitExtraLevel(formationUnitBusinessId);
            var soldierBusinessId = tiktokConfigService.GetFormationUnitSoldierBusinessId(formationUnitBusinessId);
            var soldierActions = tiktokConfigService.GetSoldierActions(soldierBusinessId);
            var samuraiActions = tiktokConfigService.GetSamuraiActions(level, formationUnitBusinessId);
            result.AddRange(soldierActions);
            if(samuraiActions!= null && samuraiActions.Count >0)
            {
                result.AddRange(samuraiActions);
            }
            return result;
        }

        //public List<IJCombatAction> Create()
        //{
        //    var result = new List<IJCombatAction>();

        //    var actionBusinessIds = GetActionsBusiness(formationUnitBusinessId);

        //    foreach(var actionBusinessId in actionBusinessIds)
        //    {
        //        var args = attributeService.GetActionFormulasArgs(actionBusinessId, 0);
        //        var finder1 = new JCombatDefaultFinder(null);
        //        var formula = new TiktokDamageFormula(new float[] { 1});
        //        var executor1 = new JCombatExecutorDamage(finder1, formula, null);
        //        var lstExecutor1 = new List<IJCombatExecutor>();
        //        lstExecutor1.Add(executor1);

        //        var actionInfo = new TiktokJCombatActionInfo();
        //        actionInfo.Uid = Guid.NewGuid().ToString();
        //        actionInfo.ActionBusinessId = actionBusinessId;
        //        actionInfo.Executors = lstExecutor1;

        //        var action1 = new JCombatActionBase(actionInfo, recorder);
        //        result.Add(action1);
        //    }


        //    return result;
        //}




    }
}

