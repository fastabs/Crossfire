namespace Crossfire.Workspace
{
    public sealed class EnemyStatsProvider : IStatsProvider
    {
        public MoveSpeedStat MoveSpeed { get; }
        public MaxHealthStat MaxHealth { get; }
        public DamageStat Damage { get; }

        public EnemyStatsProvider(IConfigRepository configurationRepository)
        {
            var baseStats = configurationRepository.Enemy.BaseStats;

            var maxHealthStatInfo = baseStats.MaxHealthStatInfo;
            MaxHealth = new MaxHealthStat(maxHealthStatInfo, StatTitleFormatter.Format(maxHealthStatInfo.TitleKey), 0);

            var moveSpeedStatInfo = baseStats.MoveSpeedStatInfo;
            MoveSpeed = new MoveSpeedStat(moveSpeedStatInfo, StatTitleFormatter.Format(moveSpeedStatInfo.TitleKey), 0);

            var damageStatInfo = baseStats.DamageStatInfo;
            Damage = new DamageStat(damageStatInfo, StatTitleFormatter.Format(damageStatInfo.TitleKey), 0);
        }
    }
}
