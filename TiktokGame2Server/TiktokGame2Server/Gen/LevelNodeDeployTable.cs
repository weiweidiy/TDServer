/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class LevelNodeDeployTable : BaseConfigTable<LevelNodeDeployCfgData>
    {
    }

    public class LevelNodeDeployCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //名字
        public List<string> UnitsUid;

    }
}
