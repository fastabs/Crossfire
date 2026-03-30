using Zenject;

namespace Crossfire.Workspace
{
    public static class PlayerInstaller
    {
        public static void BindPlayerCore(this DiContainer diContainer)
        {
            diContainer.BindInterfacesAndSelfTo<PlayerLocator>().AsSingle();
            diContainer.BindInterfacesAndSelfTo<PlayerStatsProvider>().AsSingle();
            diContainer.BindInterfacesAndSelfTo<HealthParameter>().AsSingle();
            diContainer.BindInterfacesAndSelfTo<PlayerMovementService>().AsSingle();
            diContainer.BindInterfacesAndSelfTo<PlayerHealthService>().AsSingle();
            diContainer.BindInterfacesAndSelfTo<StatsUpgradeService>().AsSingle();
            diContainer.BindInterfacesAndSelfTo<PlayerInitService>().AsSingle();
        }
    }
}

