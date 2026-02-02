using System;

namespace JFramework
{
    /// <summary>
    /// unit上的action
    /// </summary>
    public class CombatUnitAction : CombatAction, ICombatAttachable<IActionOwner>
    {
        public virtual IActionOwner Owner { get; private set; }


        public void OnAttach(IActionOwner ower)
        {
            Owner = ower;
        }

        public void OnDetach()
        {
            Owner = null;
        }
    }

}