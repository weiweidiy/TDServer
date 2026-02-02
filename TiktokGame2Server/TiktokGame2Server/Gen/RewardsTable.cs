/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class RewardsTable : BaseConfigTable<RewardsCfgData>
    {
    }

    public class RewardsCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //货币奖励类型列表
        public List<int> Currencies;

        //货币奖励数量列表
        public List<int> CurrenciesCount;

        //道具奖励Uid列表
        public List<string> Items;

        //道具奖励数量列表
        public List<int> ItemsCount;

    }
}
