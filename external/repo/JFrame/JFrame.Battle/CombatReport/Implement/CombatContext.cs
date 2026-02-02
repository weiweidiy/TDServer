namespace JFramework
{
    public class CombatContext
    {
        public virtual CombatManager CombatManager { get; set; }
        
        public virtual CombatBufferFactory CombatBufferFactory { get; set; }

        public CombatBulletManager combatBulletManager = new CombatBulletManager();

        public ILogger Logger { get; set; }


    }

}