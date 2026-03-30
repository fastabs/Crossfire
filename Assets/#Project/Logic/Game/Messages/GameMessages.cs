using UnityEngine;

namespace Crossfire.Workspace
{
    public readonly struct ChangeMaxHealthMessage { }

    public readonly struct DeathMessage
    {
        public HealthParameter Health { get; }

        public DeathMessage(HealthParameter health)
        {
            Health = health;
        }
    }

    public readonly struct FirstUpgradePointReceivedMessage { }

    public readonly struct HitEnemyMessage { }

    public readonly struct StatUpgradeCreatedMessage
    {
        public Stat Stat { get; }

        public StatUpgradeCreatedMessage(Stat stat)
        {
            Stat = stat;
        }
    }

    public readonly struct StatUpgradeValueMessage
    {
        public Stat Stat { get; }
        public int Value { get; }

        public StatUpgradeValueMessage(Stat stat, int value)
        {
            Stat = stat;
            Value = value;
        }
    }

    public readonly struct TakeDamageMessage
    {
        public Vector3 HitPoint { get; }
        public HealthParameter Health { get; }

        public TakeDamageMessage(Vector3 hitPoint, HealthParameter health)
        {
            HitPoint = hitPoint;
            Health = health;
        }
    }

    public readonly struct TakeHealMessage
    {
        public HealthParameter Health { get; }

        public TakeHealMessage(HealthParameter health)
        {
            Health = health;
        }
    }

    public readonly struct UpgradeAppliedMessage { }

    public readonly struct UpgradeCanceledMessage { }
}
