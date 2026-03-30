using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class PlayerInitService : IInitializable
    {
        private readonly IStatsProvider _statsProvider;
        private readonly HealthParameter _healthParameter;
        private readonly IPlayerLocator _playerLocator;
        private readonly Player _player;
        private readonly ISaveEntryRepository _saveEntryRepository;
        private readonly StatsUpgradeService _statsUpgradeService;
        private readonly IMessageBroker _messageBroker;
        private readonly IGameStatsService _gameStatsService;
        private readonly IPlayerMovementService _playerMovementService;

        public PlayerInitService(IStatsProvider statsProvider, HealthParameter healthParameter,
            IPlayerLocator playerLocator, Player player, ISaveEntryRepository saveEntryRepository,
            StatsUpgradeService statsUpgradeService, IMessageBroker messageBroker,
            IGameStatsService gameStatsService, IPlayerMovementService playerMovementService)
        {
            _statsProvider = statsProvider;
            _healthParameter = healthParameter;
            _playerLocator = playerLocator;
            _player = player;
            _saveEntryRepository = saveEntryRepository;
            _statsUpgradeService = statsUpgradeService;
            _messageBroker = messageBroker;
            _gameStatsService = gameStatsService;
            _playerMovementService = playerMovementService;
        }

        public void Initialize()
        {
            _playerLocator.SetupPlayer(_player);
            _healthParameter.TakeHeal(_statsProvider.MaxHealth.Value);
            ApplySaveEntry();
        }

        private void ApplySaveEntry()
        {
            if (!_saveEntryRepository.TryGetLoadedSave(out var saveEntry))
                return;

            var gameStatistics = saveEntry.GameStatistics;
            _gameStatsService.KilledEnemies = gameStatistics.KilledEnemies;
            _gameStatsService.AliveTime = gameStatistics.AliveTime;

            var playerData = saveEntry.Player;
            if (_player.Rigidbody != null)
                _player.Rigidbody.position = playerData.Position;
            else
                _player.transform.position = playerData.Position;

            _playerMovementService.SetDirectRotation(new Vector2(
                playerData.EulerAnglesRotation.x,
                playerData.EulerAnglesRotation.y));

            _statsProvider.MaxHealth.SetUpgradeLevel(playerData.MaxHealthUpgradeLevel);
            _statsProvider.MoveSpeed.SetUpgradeLevel(playerData.MoveSpeedUpgradeLevel);
            _statsProvider.Damage.SetUpgradeLevel(playerData.DamageUpgradeLevel);

            _statsUpgradeService.AddAvailableUpgrade(playerData.AvailableUpgradeCount, false);
            _statsUpgradeService.CancelUpgrade();
            _messageBroker.Publish(new ChangeMaxHealthMessage());

            _healthParameter.SetDirectly(playerData.Health);
        }
    }
}
