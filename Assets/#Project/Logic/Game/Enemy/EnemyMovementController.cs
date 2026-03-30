using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class EnemyMovementController : IInitializable, IFixedTickable
    {
        private readonly Enemy _enemy;
        private readonly IConfigRepository _config;
        private readonly IEnemyTargetDetector _targetDetector;
        private readonly IHealthService _healthService;
        private readonly IStatsProvider _statsProvider;
        private readonly PauseService _pauseService;

        private Vector3 _direction;
        private float _timeSinceDirectionChange;

        public EnemyMovementController(IEnemyTargetDetector targetDetector, IConfigRepository config,
            IStatsProvider statsProvider, IHealthService healthService, Enemy enemy, PauseService pauseService)
        {
            _targetDetector = targetDetector;
            _config = config;
            _statsProvider = statsProvider;
            _healthService = healthService;
            _enemy = enemy;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            ChooseNewDirection();
        }

        public void FixedTick()
        {
            if (_pauseService.IsPaused || _healthService.IsDead)
                return;

            MoveInDirection();
            SetRotateDirection();
        }

        private void MoveInDirection()
        {
            var directionChangeInterval = _config.Enemy.DirectionChangeInterval;
            var moveSpeed = _statsProvider.MoveSpeed.Value;

            var delta = _direction * moveSpeed * Time.deltaTime;
            var newPosition = _enemy.Rigidbody.position + delta;

            var spawnRadius = _config.Enemy.SpawnRadius;
            if (Vector3.Distance(Vector3.zero, newPosition) > spawnRadius)
            {
                var toCenter = (Vector3.zero - _enemy.transform.position).normalized;
                _direction = Vector3.Lerp(_direction, toCenter, 0.8f).normalized;
                _timeSinceDirectionChange = 0f;

                return;
            }

            _enemy.Rigidbody.MovePosition(newPosition);

            _timeSinceDirectionChange += Time.deltaTime;
            if (_timeSinceDirectionChange >= directionChangeInterval)
                ChooseNewDirection();
        }

        private void ChooseNewDirection()
        {
            _direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            _timeSinceDirectionChange = 0f;
        }

        private void SetRotateDirection()
        {
            var rotationSpeed = _config.Enemy.RotationSpeed;

            var direction = _targetDetector.Target != null
                ? (_targetDetector.Target.position - _enemy.transform.position).normalized : _direction;

            var forward = Vector3.Lerp(_enemy.transform.forward, direction, Time.fixedDeltaTime * rotationSpeed);
            var rotation = Quaternion.LookRotation(forward);
            _enemy.Rigidbody.MoveRotation(rotation);
        }
    }
}

