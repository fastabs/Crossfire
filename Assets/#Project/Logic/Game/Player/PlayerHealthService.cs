using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class PlayerHealthService : IHealthService, IInitializable, IDisposable
    {
        private static readonly int DeathTrigger = Animator.StringToHash("Death");

        private readonly CompositeDisposable _subscriptions = new();
        private readonly IPlayerLocator _playerLocator;
        private readonly ICursorService _cursorService;
        private readonly HealthParameter _playerHealth;
        private readonly IMessageBroker _messageBroker;
        private readonly Player _player;

        public bool IsDead { get; private set; }

        public PlayerHealthService(IMessageBroker messageBroker, IPlayerLocator playerLocator,
            ICursorService cursorService, HealthParameter playerHealth, Player player)
        {
            _messageBroker = messageBroker;
            _playerLocator = playerLocator;
            _cursorService = cursorService;
            _playerHealth = playerHealth;
            _player = player;
        }

        public void Initialize()
        {
            _messageBroker.Receive<DeathMessage>()
                .Subscribe(OnDead)
                .AddTo(_subscriptions);
        }

        public void Dispose()
        {
            _subscriptions.Dispose();
        }

        private void OnDead(DeathMessage message)
        {
            if (message.Health != _playerHealth || IsDead)
                return;

            IsDead = true;
            _playerLocator.SetupPlayer(null);
            _player.Animator.SetTrigger(DeathTrigger);
            _cursorService.Show();

            if (_player.Rigidbody != null)
            {
                _player.Rigidbody.velocity = Vector3.zero;
                _player.Rigidbody.angularVelocity = Vector3.zero;
            }

            _player.Weapon.SetActive(false);
        }
    }
}
