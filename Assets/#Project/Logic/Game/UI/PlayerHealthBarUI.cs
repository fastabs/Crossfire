using Lean.Transition;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class PlayerHealthBarUI : MonoBehaviour
    {
        [field: SerializeField] public Slider Slider { get; private set; }
        [field: SerializeField] public TextMeshProUGUI HealthLabel { get; private set; }

        [field: Space]

        [field: SerializeField] public LeanPlayer OnDamageTransition { get; private set; }
        [field: SerializeField] public LeanPlayer OnHealTransition { get; private set; }
        [field: SerializeField] public LeanPlayer OnMaxHealthChangedTransition { get; private set; }

        private IPlayerStatsProvider _playerStatsProvider;
        private HealthParameter _playerHealthParameter;
        private IMessageBroker _messageBroker;

        [Inject]
        private void Construct(IPlayerStatsProvider playerStatsProvider, HealthParameter playerHealthParameter,
            IMessageBroker messageBroker)
        {
            _playerStatsProvider = playerStatsProvider;
            _playerHealthParameter = playerHealthParameter;
            _messageBroker = messageBroker;
        }

        private void Awake()
        {
            _messageBroker.Receive<TakeDamageMessage>()
                .Subscribe(OnTakeDamage)
                .AddTo(this);

            _messageBroker.Receive<TakeHealMessage>()
                .Subscribe(OnTakeHeal)
                .AddTo(this);

            _messageBroker.Receive<ChangeMaxHealthMessage>()
                .Subscribe(_ => OnChangeMaxHealth())
                .AddTo(this);

            UpdateHealth();
        }

        private void OnTakeDamage(TakeDamageMessage message)
        {
            if (message.Health != _playerHealthParameter)
                return;

            OnDamageTransition?.Begin();
            UpdateHealth();
        }

        private void OnTakeHeal(TakeHealMessage message)
        {
            if (message.Health != _playerHealthParameter)
                return;

            OnHealTransition?.Begin();
            UpdateHealth();
        }

        private void OnChangeMaxHealth()
        {
            OnMaxHealthChangedTransition?.Begin();

            var maxHealth = _playerStatsProvider.MaxHealth.Value;
            if (Slider != null)
                Slider.maxValue = maxHealth;

            _playerHealthParameter.SetDirectly(maxHealth);
            UpdateHealth();
        }

        private void UpdateHealth()
        {
            var health = _playerHealthParameter.Value;

            if (Slider != null)
                Slider.value = health;

            if (HealthLabel != null)
                HealthLabel.text = health.ToString();
        }
    }
}
