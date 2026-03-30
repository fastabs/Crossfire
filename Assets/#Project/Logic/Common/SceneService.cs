using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Crossfire.Workspace
{
    public interface ISceneService
    {
        UniTask LoadMainMenuAsync();
        UniTask LoadGameAsync();
    }

    public sealed class SceneService : ISceneService
    {
        private bool _isLoading;

        public UniTask LoadMainMenuAsync()
        {
            return LoadSceneAsync(SceneNames.MainMenu);
        }

        public UniTask LoadGameAsync()
        {
            return LoadSceneAsync(SceneNames.Game);
        }

        private async UniTask LoadSceneAsync(string sceneName)
        {
            if (_isLoading)
                return;

            _isLoading = true;
            try
            {
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            }
            finally
            {
                _isLoading = false;
            }
        }
    }

    public static class SceneNames
    {
        public const string MainMenu = "MainMenu";
        public const string Game = "Game";
    }
}
