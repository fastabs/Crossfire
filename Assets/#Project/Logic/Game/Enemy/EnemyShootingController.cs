using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class EnemyShootingController : ITickable
    {
        private static readonly int TargetLayer = LayerMask.NameToLayer("Player");

        private readonly Enemy _enemy;
        private readonly PrefabsLibrary _prefabs;
        private readonly IStatsProvider _statsProvider;
        private readonly IConfigRepository _config;
        private readonly IEnemyTargetDetector _targetDetector;
        private readonly IProjectileFactory _projectileFactory;
        private readonly IHealthService _healthService;
        private readonly PauseService _pauseService;

        private float _timer;
        private Transform _lastTarget;

        public EnemyShootingController(IEnemyTargetDetector targetDetector, Enemy enemy, IStatsProvider statsProvider,
            IConfigRepository config, IProjectileFactory projectileFactory, PrefabsLibrary prefabs,
            IHealthService healthService, PauseService pauseService)
        {
            _enemy = enemy;
            _config = config;
            _projectileFactory = projectileFactory;
            _prefabs = prefabs;
            _healthService = healthService;
            _pauseService = pauseService;
            _statsProvider = statsProvider;
            _targetDetector = targetDetector;
        }

        public void Tick()
        {
            if (_pauseService.IsPaused || _healthService.IsDead)
                return;

            if (_lastTarget != _targetDetector.Target)
                StartTimer();

            TickTimer();
        }

        private void StartTimer()
        {
            if (_targetDetector.Target == null)
            {
                ResetTimer();
                return;
            }

            _lastTarget = _targetDetector.Target;
            _timer = 0f;
        }

        private void ResetTimer()
        {
            _lastTarget = null;
            _timer = 0f;
        }

        private void TickTimer()
        {
            if (_lastTarget == null)
                return;

            _timer += Time.deltaTime;
            var shootInterval = _config.Enemy.ShootInterval;
            if (_timer < shootInterval)
                return;

            Shoot();
            ResetTimer();
        }

        private void Shoot()
        {
            var shootPoint = _enemy.ShootPoint;
            var position = shootPoint.position;
            var targetPosition = _targetDetector.Target.position + Vector3.up * 1.5f;
            var enemyPosition = _enemy.transform.position;
            var direction = (targetPosition - enemyPosition).normalized;
            var rotation = Quaternion.LookRotation(direction);

            var projectileSpeed = _config.Enemy.ProjectileSpeed;
            var velocity = direction * projectileSpeed;

            if (_prefabs.Materials == null)
                return;

            var material = _prefabs.Materials.EnemyProjectileMaterial;
            var damage = _statsProvider.Damage.Value;
            var layer = TargetLayer;

            var launchParameters = new Projectile.LaunchParameters
                (position, rotation, velocity, material, damage, layer);
            _projectileFactory.LaunchProjectile(launchParameters);
        }
    }
}

