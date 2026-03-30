using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public interface IProjectileLifeController
    {
        void StartTimer();
        void ForceRelease();
    }

    public sealed class ProjectileLifeController : IProjectileLifeController, ITickable
    {
        private float _timer;
        private bool _isAlive;

        private Projectile _projectile;
        private readonly IConfigRepository _config;
        private readonly IProjectileFactory _projectileFactory;
        private readonly PauseService _pauseService;

        public ProjectileLifeController(IConfigRepository config,
            IProjectileFactory projectileFactory, PauseService pauseService)
        {
            _config = config;
            _projectileFactory = projectileFactory;
            _pauseService = pauseService;
        }

        public void SetProjectile(Projectile projectile)
        {
            _projectile = projectile;
        }

        public void Tick()
        {
            if (!_isAlive || _pauseService.IsPaused)
                return;

            _timer += Time.deltaTime;

            var projectileLifeTime = _config.GameConfig.ProjectileLifeTime;
            if (_timer < projectileLifeTime)
                return;

            ForceRelease();
        }

        public void StartTimer()
        {
            _timer = 0;
            _isAlive = true;
        }

        public void ForceRelease()
        {
            _isAlive = false;

            if (_projectile == null)
                return;

            _projectileFactory.ReleaseProjectile(_projectile);
        }
    }
}
