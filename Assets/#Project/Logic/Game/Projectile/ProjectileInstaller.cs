using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class ProjectileInstaller : MonoInstaller
    {
        [SerializeField] private Projectile projectile;

        public override void InstallBindings()
        {
            projectile ??= GetComponent<Projectile>();

            Container.BindInstance(projectile).AsSingle();
            Container.QueueForInject(projectile);
            Container.BindInterfacesAndSelfTo<ProjectileLifeController>().AsSingle()
                .OnInstantiated<ProjectileLifeController>((_, service) => service.SetProjectile(projectile));
        }
    }
}
