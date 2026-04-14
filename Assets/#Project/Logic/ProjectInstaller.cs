using System;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private ConfigRepository gameConfiguration;
        [SerializeField] private AudioSource audioSource;

        public override void InstallBindings()
        {
            if (gameConfiguration == null)
                throw new InvalidOperationException($"{nameof(ProjectInstaller)} requires a {nameof(ConfigRepository)} reference.");

            Container.Bind<ConfigRepository>().FromInstance(gameConfiguration).AsSingle();
            Container.Bind<IConfigRepository>().FromInstance(gameConfiguration).AsSingle();

            var audioSourceInstance = CreateAudioSourceInstance();
            var audioService = new AudioService(audioSourceInstance);
            Container.Bind<AudioService>().FromInstance(audioService).AsSingle();

            Container.BindInterfacesAndSelfTo<CursorService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SceneService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveEntryRepository>().AsSingle();
        }

        private AudioSource CreateAudioSourceInstance()
        {
            var audioObject = Instantiate(audioSource.gameObject);
            DontDestroyOnLoad(audioObject);
            return audioObject.GetComponent<AudioSource>();
        }
    }
}
