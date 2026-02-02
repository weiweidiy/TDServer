namespace JFramework
{

    public class CombatBulletManager : UpdateableContainer<CombatBullet>, ICombatUpdatable
    {
        public void Update(CombatFrame frame)
        {
            foreach (var bullet in GetAll())
            {
                bullet.Update(frame);
            }

            //更新待添加，刪除的對象
            UpdateWaitingItems();
        }
    }

}