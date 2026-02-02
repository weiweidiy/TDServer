/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class ActionExecutorsTable : BaseConfigTable<ActionExecutorsCfgData>
    {
    }

    public class ActionExecutorsCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //名字
        public string Name;

        //参数
        public List<float> Args;

        //筛选器
        public string FilterName;

        //筛选器参数
        public List<float> FilterArgs;

        //公式计算
        public string FormulaName;

        //公式计算参数
        public List<float> FormulaArgs;

    }
}
