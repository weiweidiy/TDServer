/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class SamuraiTable : BaseConfigTable<SamuraiCfgData>
    {
    }

    public class SamuraiCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //组别
        public int Group;

        //稀有度
        public int Rare;

        //武力
        public int Power;

        //守备
        public int Def;

        //智力
        public int Intel;

        //速度
        public int Speed;

        //默认兵种
        public string SoldierUid;

        //性别
        public int Sex;

    }
}
