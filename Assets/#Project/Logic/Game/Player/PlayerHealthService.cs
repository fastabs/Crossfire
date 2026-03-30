using System;
using UniRx;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class PlayerHealthService : IHealthService, IInitializable, IDisposable
    {
        private readonly CompositeDisposable _subscriptions = new();
        private readonly IPlayerLocator _playerLocator;
        private readonly ICursorService _cursorService;
        private readonly HealthParameter _playerHealth;
        private readonly IMessageBroker _messageBroker;

        public bool IsDead { get; private set; }

        public PlayerHealthService(IMessageBroker messageBroker, IPlayerLocator playerLocator,
            ICursorService cursorService, HealthParameter playerHealth)
        {
            _messageBroker = messageBroker;
            _playerLocator = playerLocator;
            _cursorService = cursorService;
            _playerHealth = playerHealth;
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
            if (message.Health != _playerHealth)
                return;

            IsDead = true;
            _playerLocator.SetupPlayer(null);
            _cursorService.Show();
        }
    }
}
