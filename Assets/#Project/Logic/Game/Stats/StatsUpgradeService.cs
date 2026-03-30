using System;
using System.Collections.Generic;
using UniRx;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class StatsUpgradeService : IInitializable, IDisposable
    {
        public int AvailableUpgradeCount { get; private set; }
        public int CurrentAvailableUpgradeCount => _currentAvailableUpgradeCount.Value;
        public IReadOnlyReactiveProperty<int> CurrentAvailableUpgradeCountObservable => _currentAvailableUpgradeCount;

        private readonly IntReactiveProperty _currentAvailableUpgradeCount = new();
        private readonly IPlayerStatsProvider _playerStatsProvider;
        private readonly IMessageBroker _messageBroker;

        private List<StatUpgrade> _statUpgrades;

        public StatsUpgradeService(IPlayerStatsProvider playerStatsProvider, IMessageBroker messageBroker)
        {
            _playerStatsProvider = playerStatsProvider;
            _messageBroker = messageBroker;
        }

        public void Initialize()
        {
            _statUpgrades = new List<StatUpgrade>();
            CreateUpgrade(_playerStatsProvider.MaxHealth);
            CreateUpgrade(_playerStatsProvider.MoveSpeed);
            CreateUpgrade(_playerStatsProvider.Damage);
        }

        public void Dispose()
        {
            _currentAvailableUpgradeCount.Dispose();
        }

        public void AddAvailableUpgrade(int count = 1, bool notifyFirstUpgrade = true)
        {
            var wasWithoutUpgrades = AvailableUpgradeCount <= 0;
            AvailableUpgradeCount += count;
            SetCurrentAvailableUpgradeCount(CurrentAvailableUpgradeCount + count);

            if (notifyFirstUpgrade && count > 0 && wasWithoutUpgrades && AvailableUpgradeCount > 0)
                _messageBroker.Publish(new FirstUpgradePointReceivedMessage());
        }

        public bool TryChangeStat(Stat stat, int delta)
        {
            if (delta > 0 && CurrentAvailableUpgradeCount <= 0)
                return false;

            var statUpgradeIndex = GetStatUpgradeIndex(stat);
            var statUpgrade = _statUpgrades[statUpgradeIndex];

            var upgradeLevel = statUpgrade.NewUpgradeLevel + delta;
            if (!stat.CanSetUpgrade(upgradeLevel))
                return false;

            _statUpgrades[statUpgradeIndex].NewUpgradeLevel = upgradeLevel;
            SetCurrentAvailableUpgradeCount(CurrentAvailableUpgradeCount - delta);
            _messageBroker.Publish(new StatUpgradeValueMessage(stat, statUpgrade.NewUpgradeLevel));

            return true;
        }

        public void ApplyUpgrade()
        {
            foreach (var statUpgrade in _statUpgrades)
            {
                var stat = statUpgrade.Stat;
                var currentUpgradeLevel = stat.CurrentUpgradeLevel;
                stat.SetUpgradeLevel(statUpgrade.NewUpgradeLevel);

                if (stat is MaxHealthStat && currentUpgradeLevel != statUpgrade.NewUpgradeLevel)
                    _messageBroker.Publish(new ChangeMaxHealthMessage());
            }

            AvailableUpgradeCount = CurrentAvailableUpgradeCount;
            Reset();
            _messageBroker.Publish(new UpgradeAppliedMessage());
        }

        public void CancelUpgrade()
        {
            Reset();
            _messageBroker.Publish(new UpgradeCanceledMessage());
        }

        private void CreateUpgrade(Stat stat)
        {
            _statUpgrades.Add(new StatUpgrade
            {
                Stat = stat,
                NewUpgradeLevel = stat.CurrentUpgradeLevel
            });

            _messageBroker.Publish(new StatUpgradeCreatedMessage(stat));
        }

        private void Reset()
        {
            foreach (var statUpgrade in _statUpgrades)
            {
                var level = statUpgrade.Stat.CurrentUpgradeLevel;
                statUpgrade.NewUpgradeLevel = level;
                _messageBroker.Publish(new StatUpgradeValueMessage(statUpgrade.Stat, level));
            }

            SetCurrentAvailableUpgradeCount(AvailableUpgradeCount);
        }

        private void SetCurrentAvailableUpgradeCount(int value)
        {
            _currentAvailableUpgradeCount.Value = value;
        }

        private int GetStatUpgradeIndex(Stat stat)
        {
            for (var i = 0; i < _statUpgrades.Count; i++)
            {
                if (_statUpgrades[i].Stat == stat)
                    return i;
            }

            throw new InvalidOperationException($"StatUpgrade {stat.GetType().Name} not found.");
        }

        private sealed class StatUpgrade
        {
            public Stat Stat;
            public int NewUpgradeLevel;
        }
    }
}
