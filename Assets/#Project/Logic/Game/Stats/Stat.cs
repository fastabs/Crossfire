namespace Crossfire.Workspace
{
    public abstract class Stat
    {
        public int Value => BaseValue + CurrentUpgradeLevel * ValuePerUpgradeLevel;
        public int BaseValue { get; }
        public int MaxUpgradeLevel { get; }
        public int ValuePerUpgradeLevel { get; }
        public string Title { get; }
        public int CurrentUpgradeLevel { get; private set; }

        protected Stat(BaseStatsConfig.StatInfo statInfo, string title, int startUpgradeLevel)
        {
            BaseValue = statInfo.BaseValue;
            MaxUpgradeLevel = statInfo.MaxUpgradeLevel;
            ValuePerUpgradeLevel = statInfo.ValuePerUpgradeLevel;
            Title = title;
            CurrentUpgradeLevel = startUpgradeLevel;
        }

        public bool CanSetUpgrade(int level)
        {
            return level <= MaxUpgradeLevel && level >= 0;
        }

        public void SetUpgradeLevel(int level)
        {
            if (CanSetUpgrade(level))
                CurrentUpgradeLevel = level;
        }
    }
}

