using UniRx;
using UnityEngine;

namespace Crossfire.Workspace
{
    public sealed class HealthParameter : IParameter<int>
    {
        public int Value { get; private set; }

        private readonly IStatsProvider _statsProvider;
        private readonly IMessageBroker _messageBroker;

        public HealthParameter(IStatsProvider statsProvider, IMessageBroker messageBroker)
        {
            _statsProvider = statsProvider;
            _messageBroker = messageBroker;
        }

        public void TakeDamage(int damage, Vector3 hitPoint)
        {
            var health = Value - damage;
            Value = health;

            if (health <= 0)
            {
                Value = 0;
                _messageBroker.Publish(new DeathMessage(this));
            }

            _messageBroker.Publish(new TakeDamageMessage(hitPoint, this));
        }

        public void TakeHeal(int heal)
        {
            SetDirectly(Value + heal);
        }

        public void SetDirectly(int health)
        {
            var maxHealth = _statsProvider.MaxHealth.Value;
            Value = Mathf.Clamp(health, 0, maxHealth);
            _messageBroker.Publish(new TakeHealMessage(this));
        }
    }
}
