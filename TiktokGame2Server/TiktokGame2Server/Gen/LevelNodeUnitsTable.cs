/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class LevelNodeUnitsTable : BaseConfigTable<LevelNodeUnitsCfgData>
    {
    }

    public class LevelNodeUnitsCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //武将
        public string SamuraiUid;

        //兵种Uid
        public string SoldierUid;

        //额外攻击力
        public int Atk;

        //额外防御力
        public int Def;

        //额外生命力
        public int Hp;

        //额外速度
        public int Speed;

        //等级
        public int Level;

    }
}
