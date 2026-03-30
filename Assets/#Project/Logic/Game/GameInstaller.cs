using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class GameInstaller : MonoInstaller
    {
        [SerializeField] private Player player;
        [SerializeField] private PrefabsLibrary prefabsLibrary;

        public override void InstallBindings()
        {
            player ??= SceneObjectLocator.FindInScene<Player>(gameObject.scene);
            if (player == null)
                throw new InvalidOperationException($"{nameof(GameInstaller)} requires a Player in the Game scene.");

            Container.BindInstance(new InputActions()).AsSingle();
            Container.BindInstance(player).AsSingle();
            Container.QueueForInject(player);

            if (prefabsLibrary != null)
                Container.BindInstance(prefabsLibrary).AsSingle();

            Container.BindInterfacesAndSelfTo<MessageBroker>().AsSingle();
            Container.BindInterfacesAndSelfTo<PauseService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameStatsService>().AsSingle();
            Container.BindPlayerCore();

            Container.BindInterfacesAndSelfTo<ProjectileFactory>().AsSingle();
            Container.Bind<IPlayerShootingService>().To<PlayerShootingService>().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemySpawnController>().AsSingle();

            Container.BindInterfacesAndSelfTo<PlayerInputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveGameService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameInitService>().AsSingle();
            Container.BindInterfacesAndSelfTo<LayerCollisionInitService>().AsSingle();
        }
    }
}
