namespace Crossfire.Workspace
{
    public interface IPlayerStatsProvider : IStatsProvider { }

    public sealed class PlayerStatsProvider : IPlayerStatsProvider
    {
        public MoveSpeedStat MoveSpeed { get; }
        public MaxHealthStat MaxHealth { get; }
        public DamageStat Damage { get; }

        public PlayerStatsProvider(IConfigRepository configurationRepository)
        {
            var baseStats = configurationRepository.Player.BaseStats;

            var maxHealthStatInfo = baseStats.MaxHealthStatInfo;
            MaxHealth = new MaxHealthStat(maxHealthStatInfo, StatTitleFormatter.Format(maxHealthStatInfo.TitleKey), 0);

            var moveSpeedStatInfo = baseStats.MoveSpeedStatInfo;
            MoveSpeed = new MoveSpeedStat(moveSpeedStatInfo, StatTitleFormatter.Format(moveSpeedStatInfo.TitleKey), 0);

            var damageStatInfo = baseStats.DamageStatInfo;
            Damage = new DamageStat(damageStatInfo, StatTitleFormatter.Format(damageStatInfo.TitleKey), 0);
        }
    }
}
