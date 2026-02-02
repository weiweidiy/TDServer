using System;

namespace JFramework
{
    // xx 普通攻击了 yy 和 zz, 造成了 yy 10点伤害， zz 20点伤害
    // xx 特殊攻击了 yy 和 zz, 造成了 yy 10点伤害，并添加了a buff, 造成了 zz 20点伤害
    // xx 移除a buff

    //xx 向yy 发起了普通
    //yy 受到了10点伤害
    //zz 添加了一个buff 16


    public interface IBattleReportData : ICloneable
    {
        string UID { get; }
        int Frame { get; }
        float EscapeTime { get; }
        string CasterUID { get; }
        string TargetUID {  get; }
        ReportType ReportType { get; }

        /// <summary>
        /// 对应id
        /// </summary>
        object[] Arg {  get; } //如果reportType = action , arg = action id, 如果是damage , Arg = damage

    }


}