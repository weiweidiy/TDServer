/*
* 此类由ConfigTools自动生成. 不要手动修改!
*/
using System.Collections;
using System.Collections.Generic;
using JFramework.Game;

namespace JFramework
{
    public partial class FormationPointsUnlockTable : BaseConfigTable<FormationPointsUnlockCfgData>
    {
    }

    public class FormationPointsUnlockCfgData : IUnique
    {
        //唯一标识
        public string Uid{ get;set;} 

        //阵型Uid
        public string FormationBusinessId;

        //点位
        public int PointIndex;

        //解锁等级
        public int UnlockLevel;

    }
}
