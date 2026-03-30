namespace Crossfire.Workspace
{
    public interface IStatsProvider
    {
        MoveSpeedStat MoveSpeed { get; }
        MaxHealthStat MaxHealth { get; }
        DamageStat Damage { get; }
    }
}

