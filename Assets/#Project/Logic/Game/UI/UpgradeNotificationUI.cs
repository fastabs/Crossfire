using Lean.Transition;
using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class UpgradeNotificationUI : MonoBehaviour
    {
        [field: SerializeField] public LeanPlayer ShowTransition { get; private set; }

        private IMessageBroker _messageBroker;
        private PlayerHealthService _playerHealth;

        [Inject]
        private void Construct(IMessageBroker messageBroker, PlayerHealthService playerHealth)
        {
            _messageBroker = messageBroker;
            _playerHealth = playerHealth;
        }

        private void Awake()
        {
            _messageBroker.Receive<FirstUpgradePointReceivedMessage>()
                .Subscribe(_ => OnFirstUpgradePointReceived())
                .AddTo(this);
        }

        private void OnFirstUpgradePointReceived()
        {
            if (_playerHealth != null && _playerHealth.IsDead)
                return;

            ShowTransition?.Begin();
        }
    }
}
