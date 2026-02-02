/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class BuildingsUpgradeCostTable : BaseConfigTable<BuildingsUpgradeCostCfgData>
    {
    }

    public class BuildingsUpgradeCostCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //建筑Uid
        public string BuildingUid;

        //建筑等级
        public int BuildingLevel;

        //消费货币
        public string Currency;

        //消费货币数量
        public int CurrencyCount;

        //功能参数
        public int Arg;

    }
}
