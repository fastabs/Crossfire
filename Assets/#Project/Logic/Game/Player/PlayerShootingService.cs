using UnityEngine;

namespace Crossfire.Workspace
{
    public interface IPlayerShootingService
    {
        void Shoot();
    }

    public sealed class PlayerShootingService : IPlayerShootingService
    {
        private static readonly int TargetLayer = LayerMask.NameToLayer("Enemy");

        private readonly Player _player;
        private readonly PrefabsLibrary _prefabs;
        private readonly IConfigRepository _config;
        private readonly IPlayerStatsProvider _playerStatsProvider;
        private readonly IProjectileFactory _projectileFactory;

        public PlayerShootingService(Player player, IConfigRepository config,
            IPlayerStatsProvider playerStatsProvider, IProjectileFactory projectileFactory, PrefabsLibrary prefabs)
        {
            _player = player;
            _config = config;
            _playerStatsProvider = playerStatsProvider;
            _projectileFactory = projectileFactory;
            _prefabs = prefabs;
        }

        public void Shoot()
        {
            var shootPoint = _player.GunShootPoint;
            if (shootPoint == null || _prefabs.Materials == null)
                return;

            var position = shootPoint.position;
            var direction = shootPoint.forward;
            var rotation = Quaternion.LookRotation(direction);

            var projectileSpeed = _config.Player.ProjectileSpeed;
            var velocity = direction * projectileSpeed;

            var material = _prefabs.Materials.PlayerProjectileMaterial;
            var damage = _playerStatsProvider.Damage.Value;
            var layer = TargetLayer;

            var launchParameters = new Projectile.LaunchParameters
                (position, rotation, velocity, material, damage, layer);
            _projectileFactory.LaunchProjectile(launchParameters);
        }
    }
}

