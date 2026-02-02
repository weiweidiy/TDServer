using System;

namespace JFramework
{
    /// <summary>
    /// type = 6   参数：0=队伍(0友军，1敌军，2所有)   1=主类型  2=子类型  3模式(0模式单位， 1逻辑单位) 4=个数  5: x最小坐标 6：x最大坐标
    /// </summary>
    public class FinderFindRangeByScreen : FinderFindUnits
    {
        public override int GetValidArgsCount()
        {
            return base.GetValidArgsCount() + 2;
        }

        protected float GetMinWidthArg()
        {
            return GetCurArg(5);
        }

        protected float GetMaxWidthArg()
        {
            return GetCurArg(6);
        }

        protected override bool IsHit(CombatUnit unit, CombatExtraData extraData)
        {
 
            var x = unit.GetPosition().x;
            var widthMinX = GetMinWidthArg();
            var widthMaxX = GetMaxWidthArg();

            if (x < widthMinX || x > widthMaxX)
                return false;

            return true;
        }
    }
}