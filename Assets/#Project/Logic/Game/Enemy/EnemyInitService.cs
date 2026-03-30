using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class EnemyInitService : IInitializable
    {
        private readonly IStatsProvider _statsProvider;
        private readonly HealthParameter _healthParameter;

        public EnemyInitService(IStatsProvider statsProvider, HealthParameter healthParameter)
        {
            _statsProvider = statsProvider;
            _healthParameter = healthParameter;
        }

        public void Initialize()
        {
            //рандомное начальное здоровье врага
            _statsProvider.MaxHealth.SetUpgradeLevel(Random.Range(0, 5));
            _healthParameter.TakeHeal(_statsProvider.MaxHealth.Value);
        }
    }
}
