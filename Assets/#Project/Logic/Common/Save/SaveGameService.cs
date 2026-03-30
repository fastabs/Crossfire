using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public interface ISaveGameService
    {
        void SaveGame();
    }

    public sealed class SaveGameService : ISaveGameService
    {
        private readonly Player _player;
        private readonly HealthParameter _playerHealthParameter;
        private readonly IPlayerStatsProvider _playerStatsProvider;
        private readonly StatsUpgradeService _statsUpgradeService;
        private readonly ISaveEntryRepository _saveEntryRepository;
        private readonly IGameStatsService gameStatsService;
        private readonly EnemySpawnController enemySpawnController;

        public SaveGameService(Player player, HealthParameter playerHealthParameter,
            IPlayerStatsProvider playerStatsProvider, StatsUpgradeService statsUpgradeService,
            ISaveEntryRepository saveEntryRepository, IGameStatsService gameStatsService,
            [InjectOptional] EnemySpawnController enemySpawnController = null)
        {
            _player = player;
            _playerHealthParameter = playerHealthParameter;
            _playerStatsProvider = playerStatsProvider;
            _statsUpgradeService = statsUpgradeService;
            _saveEntryRepository = saveEntryRepository;
            this.gameStatsService = gameStatsService;
            this.enemySpawnController = enemySpawnController;
        }

        public void SaveGame()
        {
            var entry = new SaveEntry
            {
                GameStatistics = CollectGameStatistics(),
                Player = CollectPlayerData(),
                Enemies = CollectEnemiesData()
            };

            _saveEntryRepository.Save(entry);
        }

        private SaveEntry.GameStatisticsData CollectGameStatistics()
        {
            return new SaveEntry.GameStatisticsData
            {
                KilledEnemies = gameStatsService.KilledEnemies,
                AliveTime = gameStatsService.AliveTime
            };
        }

        private SaveEntry.PlayerData CollectPlayerData()
        {
            var headRotationX = 0f;
            if (_player.Head != null)
                headRotationX = NormalizeAngle(_player.Head.localEulerAngles.x);

            return new SaveEntry.PlayerData
            {
                Position = _player.transform.position,
                EulerAnglesRotation = new Vector3(headRotationX, NormalizeAngle(_player.transform.eulerAngles.y), 0f),
                Health = _playerHealthParameter.Value,
                MaxHealthUpgradeLevel = _playerStatsProvider.MaxHealth.CurrentUpgradeLevel,
                MoveSpeedUpgradeLevel = _playerStatsProvider.MoveSpeed.CurrentUpgradeLevel,
                DamageUpgradeLevel = _playerStatsProvider.Damage.CurrentUpgradeLevel,
                AvailableUpgradeCount = _statsUpgradeService.AvailableUpgradeCount
            };
        }

        private SaveEntry.EnemyData[] CollectEnemiesData()
        {
            if (enemySpawnController == null)
                return System.Array.Empty<SaveEntry.EnemyData>();

            var enemies = new List<SaveEntry.EnemyData>();
            foreach (var enemy in enemySpawnController.GetAllEnemies())
            {
                if (enemy == null || enemy.HealthParameter == null || enemy.StatsProvider == null)
                    continue;

                enemies.Add(new SaveEntry.EnemyData
                {
                    Position = enemy.transform.position,
                    Health = enemy.HealthParameter.Value,
                    MaxHealthUpgradeLevel = enemy.StatsProvider.MaxHealth.CurrentUpgradeLevel,
                    MoveSpeedUpgradeLevel = enemy.StatsProvider.MoveSpeed.CurrentUpgradeLevel,
                    DamageUpgradeLevel = enemy.StatsProvider.Damage.CurrentUpgradeLevel
                });
            }

            return enemies.ToArray();
        }

        private float NormalizeAngle(float angle)
        {
            if (angle > 180f)
                angle -= 360f;

            return angle;
        }
    }
}
