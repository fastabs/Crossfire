using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class MainMenuInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuUI mainMenuUI;

        public override void InstallBindings()
        {
            mainMenuUI ??= SceneObjectLocator.FindInScene<MainMenuUI>(gameObject.scene);

            Container.BindInstance(mainMenuUI);
            Container.QueueForInject(mainMenuUI);
            Container.BindInterfacesAndSelfTo<MainMenuInitService>().AsSingle();
            Container.BindInterfacesAndSelfTo<MainMenuService>().AsSingle();
        }
    }
}
