public static class ExpCalculator
{
    public static int GetLevelUpExp(int level)
    {
        if (level <= 1) return 0;
        int baseExp = 100;
        int perLevelInc = 30;
        int extraInc = 30;
        int groupInc = 0;
        for (int i = 2; i <= level; i++)
        {
            if ((i - 1) % 5 == 0) groupInc += extraInc;
        }
        return baseExp + perLevelInc * (level - 1) + groupInc * (level - 1) / 5;
    }

    public static int GetTotalExpToLevel(int level)
    {
        int total = 0;
        for (int i = 2; i <= level; i++)
        {
            total += GetLevelUpExp(i);
        }
        return total;
    }

    public static int GetLevelByTotalExp(int totalExp)
    {
        int level = 1;
        while (true)
        {
            int nextExp = GetTotalExpToLevel(level + 1);
            if (totalExp < nextExp) break;
            level++;
        }
        return level;
    }
}