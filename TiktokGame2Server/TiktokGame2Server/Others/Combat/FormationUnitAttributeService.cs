using JFramework;

namespace TiktokGame2Server.Others
{
    public class FormationUnitAttributeService : IAttributeService
    {
        private readonly string formationUnitBusinessId;
        private readonly TiktokConfigManager tiktokConfigService;

        LevelNodeUnitsCfgData cfg;

        public FormationUnitAttributeService(string formationUnitBusinessId, TiktokConfigManager tiktokConfigService)
        {
            this.formationUnitBusinessId = formationUnitBusinessId;
            this.tiktokConfigService = tiktokConfigService;
            cfg = tiktokConfigService.Get<LevelNodeUnitsCfgData>(formationUnitBusinessId);
        }
        
        public int GetHp() => tiktokConfigService.GetFormationUnitMaxHp(formationUnitBusinessId, cfg);
        public int GetMaxHp() => tiktokConfigService.GetFormationUnitMaxHp(formationUnitBusinessId, cfg);
        public int GetAttack() => tiktokConfigService.GetFormationUnitAttack(formationUnitBusinessId, cfg);
        public int GetDefence() => tiktokConfigService.GetFormationUnitDefence(formationUnitBusinessId, cfg);
        public int GetSpeed() => tiktokConfigService.GetFormationUnitSpeed(formationUnitBusinessId, cfg);

        public int GetLevel() => tiktokConfigService.GetFormationUnitExtraLevel(formationUnitBusinessId, cfg);
        public int GetSex() => tiktokConfigService.GetFormationUnitSex(formationUnitBusinessId, cfg);
        public int GetPower() => tiktokConfigService.GetFormationUnitPower(formationUnitBusinessId, cfg);
        public int GetDef() => tiktokConfigService.GetFormationUnitDef(formationUnitBusinessId, cfg);
        public int GetIntel() => tiktokConfigService.GetFormationUnitIntel(formationUnitBusinessId, cfg);
    }
}
