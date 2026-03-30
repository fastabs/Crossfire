using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class EnemySpawnController : ITickable
    {
        private const float CheckRadius = 2f;
        private const int TrySpawnCount = 10;
        private const float FirstSpawnDelaySeconds = 1f;

        private readonly IConfigRepository _config;
        private readonly PrefabsLibrary _prefabs;
        private readonly PauseService _pauseService;
        private readonly DiContainer _container;
        private readonly PlayerHealthService _playerHealthService;

        private readonly List<Enemy> _enemies;
        private float _timer;
        private bool _isFirstSpawnDone;

        public EnemySpawnController(IConfigRepository config, PrefabsLibrary prefabs,
            PauseService pauseService, DiContainer container, PlayerHealthService playerHealthService)
        {
            _enemies = new List<Enemy>();

            _config = config;
            _prefabs = prefabs;
            _pauseService = pauseService;
            _container = container;
            _playerHealthService = playerHealthService;
        }

        public void Tick()
        {
            if (_pauseService.IsPaused || _playerHealthService.IsDead)
                return;

            var spawnInterval = _isFirstSpawnDone
                ? _config.Enemy.SpawnInterval
                : FirstSpawnDelaySeconds;

            _timer += Time.deltaTime;

            if (_timer < spawnInterval)
                return;

            TrySpawnEnemy();
        }

        public Enemy[] GetAllEnemies()
        {
            _enemies.RemoveAll(enemy => enemy == null);
            return _enemies.ToArray();
        }

        public Enemy SpawnEnemy(Vector3 position)
        {
            if (_prefabs.Game == null || _prefabs.Game.EnemyPrefab == null)
            {
                Debug.LogWarning($"{nameof(EnemySpawnController)} requires an enemy prefab in {nameof(PrefabsLibrary)}.");
                return null;
            }

            var enemyPrefab = _prefabs.Game.EnemyPrefab;
            var enemy = _container.InstantiatePrefabForComponent<Enemy>(enemyPrefab, position, Quaternion.identity, null);
            _enemies.Add(enemy);

            return enemy;
        }

        private void TrySpawnEnemy()
        {
            for (var i = 0; i < TrySpawnCount; i++)
            {
                var randomPosition = GetRandomPosition();

                if (Physics.CheckSphere(randomPosition, CheckRadius))
                    continue;

                if (SpawnEnemy(randomPosition) == null)
                    return;

                _isFirstSpawnDone = true;
                _timer = 0f;
                return;
            }

            Debug.Log("Spawn point not found");
        }

        private Vector3 GetRandomPosition()
        {
            var spawnRadius = _config.Enemy.SpawnRadius;
            var flyHeight = _config.Enemy.FlyHeight.GetRandom();

            var randomCircle = Random.insideUnitCircle * spawnRadius;
            var spawnPosition = new Vector3(randomCircle.x, flyHeight, randomCircle.y);

            return spawnPosition;
        }
    }
}

