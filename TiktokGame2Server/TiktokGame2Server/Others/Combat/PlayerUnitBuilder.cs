using JFramework.Game;
using TiktokGame2Server.Entities;
using static TiktokGame2Server.Others.LevelNodeCombatService;

namespace TiktokGame2Server.Others
{
    public class PlayerUnitBuilder : JCombatBaseUnitBuilder
    {
        Samurai samurai;
        TiktokConfigManager tiktokConfigService ;
        public PlayerUnitBuilder(IJCombatAttrBuilder attrBuilder, IJCombatActionBuilder actionBuilder, Samurai samurai, TiktokConfigManager tiktokConfigService) : base(attrBuilder, actionBuilder)
        {
            this.samurai = samurai;
            this.tiktokConfigService = tiktokConfigService;
        }

        public override IJCombatUnitInfo Build()
        {
            var unitInfo = new TiktokJCombatUnitInfo
            {
                Uid = samurai.Id.ToString(),
                AttrList = attrBuilder.Create(),
                Actions = actionBuilder.Create(),
                SamuraiBusinessId = samurai.BusinessId,
                SoldierBusinessId = samurai.SoldierBusinessId,
                SoldierType = tiktokConfigService.GetSoldierType(samurai.SoldierBusinessId)
            };

            return unitInfo;
        }
    }
}



