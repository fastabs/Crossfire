using System;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ConfigRepository gameConfiguration;

        public override void InstallBindings()
        {
            if (gameConfiguration == null)
                throw new InvalidOperationException($"{nameof(ProjectInstaller)} requires a {nameof(ConfigRepository)} reference.");

            Container.Bind<ConfigRepository>().FromInstance(gameConfiguration).AsSingle();
            Container.Bind<IConfigRepository>().FromInstance(gameConfiguration).AsSingle();
            Container.BindInterfacesAndSelfTo<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveEntryRepository>().AsSingle();
        }
    }
}
