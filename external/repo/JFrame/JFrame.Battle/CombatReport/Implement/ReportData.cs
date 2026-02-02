using System;
using System.Collections.Generic;

namespace JFramework
{
    [Serializable]
    public class  ReportData
    {
        public string CastUnitUid { get;  set; }

        public int ActionId { get; set; }

        public string ActionUid { get; set; }
        public List<string> TargetsUid { get; set; }
        public string TargetUid { get; set; }

        public float CastDuration { get; set; }
        public float CdDuration { get; set; }

        public double Value { get; set; }

        public double TargetHp { get; set; }
        public double TargetMaxHp { get; set; }

        /// <summary>
        /// 是否暴击
        /// </summary>
        public bool IsCri { get; set; }

        /// <summary>
        /// 是否格挡
        /// </summary>
        public bool IsBlock { get; set; }

        /// <summary>
        /// 移动速度
        /// </summary>
        public CombatVector Velocity { get; set; }

        public string BufferUid { get; set; }

        public int BufferId { get; set; }

        public int BufferFoldCount { get; set; }    

        /// <summary>
        /// 发射次数
        /// </summary>
        public int ShootCount { get; set; }
    }




}