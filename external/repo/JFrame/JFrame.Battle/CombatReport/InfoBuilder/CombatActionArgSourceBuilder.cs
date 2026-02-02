using System.Collections.Generic;

namespace JFramework
{
    public abstract class CombatActionArgSourceBuilder
    {
        public abstract Dictionary<int, CombatActionArgSource> Build();
    }
}