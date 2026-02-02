using JFramework.Common;
using System;
using System.Collections.Generic;

namespace JFramework
{

    public abstract class CombatBaseFinder : BaseActionComponent, ICombatFinder
    {
        public abstract List<CombatUnit> FindTargets(CombatExtraData extraData);

        protected override void OnUpdate(CombatFrame frame)
        {
            //throw new NotImplementedException();
        }
    }
}