/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class LevelsTable : BaseConfigTable<LevelsCfgData>
    {
    }

    public class LevelsCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //下一关Uid
        public string Next;

        //上一关Uid
        public string Pre;

    }
}
