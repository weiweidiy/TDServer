/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class ActionsTable : BaseConfigTable<ActionsCfgData>
    {
    }

    public class ActionsCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        /*
        JiChunWei:
1.自己受到伤害前触发，返回自己+伤害数据
2.战斗开始时触发1次，返回自己
        */
        //触发器
        public List<string> TriggersUid;

        //查找器
        public string FinderUid;

        //执行器
        public List<string> ExecutorsUid;

    }
}
