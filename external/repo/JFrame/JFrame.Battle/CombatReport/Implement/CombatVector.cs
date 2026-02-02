using System;


namespace JFramework
{
    [Serializable]
    public class CombatVector
    {
        public float x;
        public float y;

        public static CombatVector operator + (CombatVector vec1, CombatVector vec2)
        {
            var result = new CombatVector();
            result.x = vec1.x + vec2.x;
            result.y = vec1.y + vec2.y;
            return result;
        }

        public static CombatVector operator -(CombatVector a, CombatVector b)
        => new CombatVector { x = a.x - b.x, y = a.y - b.y };

        public float Magnitude()
        => (float)Math.Sqrt(x * x + y * y);

    }




}