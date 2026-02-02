/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class ActionTriggersTable : BaseConfigTable<ActionTriggersCfgData>
    {
    }

    public class ActionTriggersCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //查找器
        public string FinderUid;

        //名字
        public string Name;

        //参数
        public List<float> Args;

    }
}
