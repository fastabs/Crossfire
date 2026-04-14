using Zenject;

namespace Crossfire.Workspace
{
    public sealed class MainMenuInitService : IInitializable
    {
        private readonly AudioService _audioService;

        public MainMenuInitService(AudioService audioService)
        {
            _audioService = audioService;
        }

        public void Initialize()
        {
            _audioService.SetMainThemeVolume(0.2f);
        }
    }
}