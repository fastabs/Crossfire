using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class GameInitService : IInitializable
    {
        private readonly ICursorService _cursorService;
        private readonly ISaveEntryRepository _saveEntryRepository;
        private readonly EnemySpawnController _enemySpawnController;

        public GameInitService(ICursorService cursorService, ISaveEntryRepository saveEntryRepository,
            [InjectOptional] EnemySpawnController enemySpawnController = null)
        {
            _cursorService = cursorService;
            _saveEntryRepository = saveEntryRepository;
            _enemySpawnController = enemySpawnController;
        }

        public void Initialize()
        {
            Application.targetFrameRate = 60;
            ApplySaveEntryForEnemies();
            _cursorService.Hide();
        }

        private void ApplySaveEntryForEnemies()
        {
            if (_enemySpawnController == null)
                return;

            if (!_saveEntryRepository.TryGetLoadedSave(out var saveEntry))
                return;

            foreach (var enemyData in saveEntry.Enemies)
            {
                var enemy = _enemySpawnController.SpawnEnemy(enemyData.Position);
                if (enemy == null)
                    continue;

                enemy.HealthParameter?.SetDirectly(enemyData.Health);

                var statsProvider = enemy.StatsProvider;
                if (statsProvider == null)
                    continue;

                statsProvider.MaxHealth.SetUpgradeLevel(enemyData.MaxHealthUpgradeLevel);
                statsProvider.MoveSpeed.SetUpgradeLevel(enemyData.MoveSpeedUpgradeLevel);
                statsProvider.Damage.SetUpgradeLevel(enemyData.DamageUpgradeLevel);
            }
        }
    }
}
