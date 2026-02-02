using JFramework.Game;
using System.Reflection;
using System.Text;
using TiktokGame2Server.Entities;

namespace TiktokGame2Server.Others
{

    public class PlayerActionsBuilder : TiktokJCombatAcionsBaseBuilder
    {
        Samurai samurai;

        public PlayerActionsBuilder(Samurai samurai,  TiktokConfigManager tiktokConfigService, IJCombatContext context) :base( tiktokConfigService, context)
        {
            this.samurai = samurai ?? throw new ArgumentNullException(nameof(samurai));
        }

       
        protected override List<string> GetActionsBusiness()
        {
            var level = samurai.Level;
            var smuraiBusinessId = samurai.BusinessId;
            var samuraiAction = tiktokConfigService.GetSamuraiActions(level, smuraiBusinessId);
            var soldierAction = tiktokConfigService.GetSoldierActions(samurai.SoldierBusinessId);
            // 根据武士等级和武士ID获取可用的动作
            samuraiAction.AddRange(soldierAction);
            return samuraiAction;
        }       
    }
}

