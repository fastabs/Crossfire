using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Crossfire.Workspace
{
    public interface IProjectileFactory
    {
        void LaunchProjectile(Projectile.LaunchParameters parameters);
        void ReleaseProjectile(Projectile projectile);
    }

    public sealed class ProjectileFactory : IProjectileFactory
    {
        private readonly ObjectPool<Projectile> _pool;
        private readonly PrefabsLibrary _prefabs;
        private readonly DiContainer _container;

        public ProjectileFactory(PrefabsLibrary prefabs, DiContainer container)
        {
            _pool = new ObjectPool<Projectile>(CreateProjectile, ActionOnGet, ActionOnRelease, ActionOnDestroy);
            _prefabs = prefabs;
            _container = container;
        }

        public void LaunchProjectile(Projectile.LaunchParameters parameters)
        {
            var projectile = _pool.Get();
            if (projectile == null)
                return;

            projectile.Launch(parameters);
        }

        public void ReleaseProjectile(Projectile projectile)
        {
            if (projectile == null)
                return;

            _pool.Release(projectile);
        }

        private Projectile CreateProjectile()
        {
            if (_prefabs.Game == null || _prefabs.Game.ProjectilePrefab == null)
                return null;

            return _container.InstantiatePrefabForComponent<Projectile>(_prefabs.Game.ProjectilePrefab);
        }

        private void ActionOnGet(Projectile projectile)
        {
            if (projectile == null)
                return;

            projectile.gameObject.SetActive(true);
        }

        private void ActionOnRelease(Projectile projectile)
        {
            if (projectile == null)
                return;

            projectile.Rigidbody.velocity = Vector3.zero;
            projectile.Rigidbody.angularVelocity = Vector3.zero;
            projectile.gameObject.SetActive(false);
        }

        private void ActionOnDestroy(Projectile projectile)
        {
            if (projectile == null)
                return;

            Object.Destroy(projectile.gameObject);
        }
    }
}

