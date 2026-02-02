using static JFramework.PVPBattleManager;

namespace JFramework
{
    public class BattlePoint
    {
        public enum PointType
        {
            Front,
            Back
        }

        public int Point { get; private set; }

        public Team Team { get; private set; }

        public BattlePoint(int point, Team team)
        {
            this.Point = point;
            this.Team = team;
        }

        public PointType GetPointType()
        {
            return GetPointType(this.Point);
        }

        public PointType GetPointType(int point)
        {
            switch (point)
            {
                case 1:
                    return PointType.Front;
                case 2:
                    return PointType.Front;
                case 3:
                    return PointType.Back;
                case 4:
                    return PointType.Back;
                case 5:
                    return PointType.Back;
                default:
                    throw new System.Exception("没有实现的点位 " + point);
            }
        }
    }
}