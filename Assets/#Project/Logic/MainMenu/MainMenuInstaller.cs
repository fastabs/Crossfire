using System;
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
            if (mainMenuUI == null)
                throw new InvalidOperationException($"{nameof(MainMenuInstaller)} requires a {nameof(MainMenuUI)} in the MainMenu scene.");

            Container.BindInstance(mainMenuUI);
            Container.QueueForInject(mainMenuUI);
            Container.BindInterfacesAndSelfTo<MainMenuService>().AsSingle();
        }
    }
}
