using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class EnemyHealthController : IHealthService, IInitializable, IDisposable
    {
        private const float DestroyDelay = 2f;
        private static readonly int DeathTrigger = Animator.StringToHash("Death");
        private static readonly int TakeDamageTrigger = Animator.StringToHash("TakeDamage");

        private readonly CompositeDisposable _subscriptions = new();
        private readonly IConfigRepository _config;
        private readonly IGameStatsService _gameStatsService;
        private readonly StatsUpgradeService _statsUpgradeService;
        private readonly Enemy _enemy;
        private readonly HealthParameter _healthParameter;
        private readonly IMessageBroker _messageBroker;

        public EnemyHealthController(Enemy enemy, HealthParameter healthParameter, IMessageBroker messageBroker,
            IConfigRepository config, IGameStatsService gameStatsService, StatsUpgradeService statsUpgradeService)
        {
            _enemy = enemy;
            _healthParameter = healthParameter;
            _messageBroker = messageBroker;
            _config = config;
            _gameStatsService = gameStatsService;
            _statsUpgradeService = statsUpgradeService;
        }

        public bool IsDead { get; private set; }

        public void Initialize()
        {
            _messageBroker.Receive<TakeDamageMessage>()
                .Subscribe(OnTakeDamage)
                .AddTo(_subscriptions);

            _messageBroker.Receive<DeathMessage>()
                .Subscribe(OnDead)
                .AddTo(_subscriptions);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }

        private void OnTakeDamage(TakeDamageMessage message)
        {
            if (message.Health != _healthParameter)
                return;

            _messageBroker.Publish(new HitEnemyMessage());

            if (!IsDead)
            {
                _enemy.Animator.SetTrigger(TakeDamageTrigger);
                return;
            }

            if (_enemy.Rigidbody != null)
            {
                var deathVelocityForce = _config.Enemy.DeathVelocityForce;
                var force = -_enemy.transform.forward * deathVelocityForce;
                _enemy.Rigidbody.AddForceAtPosition(force, message.HitPoint, ForceMode.Impulse);
            }
        }

        private void OnDead(DeathMessage message)
        {
            if (message.Health != _healthParameter || IsDead)
                return;

            IsDead = true;
            _enemy.Animator.SetTrigger(DeathTrigger);

            if (_enemy.Rigidbody != null)
            {
                _enemy.Rigidbody.useGravity = true;
                _enemy.Rigidbody.constraints = RigidbodyConstraints.None;
                _enemy.Rigidbody.velocity = Vector3.down;
            }

            UnityEngine.Object.Destroy(_enemy.gameObject, DestroyDelay);

            _statsUpgradeService.AddAvailableUpgrade();
            _gameStatsService.AddEnemyKill();
        }
    }
}
