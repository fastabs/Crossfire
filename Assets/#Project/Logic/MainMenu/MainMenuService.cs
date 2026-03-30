using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class MainMenuService : IInitializable, IDisposable
    {
        private readonly MainMenuUI _mainMenuUI;
        private readonly ISceneService _sceneService;
        private readonly ISaveEntryRepository _saveEntryRepository;

        public MainMenuService(MainMenuUI mainMenuUI, ISceneService sceneService,
            ISaveEntryRepository saveEntryRepository)
        {
            _mainMenuUI = mainMenuUI;
            _sceneService = sceneService;
            _saveEntryRepository = saveEntryRepository;
        }

        public void Initialize()
        {
            _mainMenuUI.NewGameButton.OnClick.AddListener(OnStartClicked);
            _mainMenuUI.ExitButton.OnClick.AddListener(OnExit);

            if (_mainMenuUI.LoadGameButton != null)
            {
                _mainMenuUI.LoadGameButton.interactable = _saveEntryRepository.IsSaveFileExists;
                _mainMenuUI.LoadGameButton.OnClick.AddListener(OnLoadClicked);
            }
        }

        public void Dispose()
        {
            _mainMenuUI.NewGameButton.OnClick.RemoveListener(OnStartClicked);
            _mainMenuUI.ExitButton.OnClick.RemoveListener(OnExit);

            if (_mainMenuUI.LoadGameButton != null)
                _mainMenuUI.LoadGameButton.OnClick.RemoveListener(OnLoadClicked);
        }

        private void OnStartClicked()
        {
            StartNewGameAsync().Forget();
        }

        private void OnLoadClicked()
        {
            LoadGameAsync().Forget();
        }

        private async UniTaskVoid StartNewGameAsync()
        {
            _saveEntryRepository.DeleteSave();
            _mainMenuUI.NewGameButton.interactable = false;
            await _sceneService.LoadGameAsync();
        }

        private async UniTaskVoid LoadGameAsync()
        {
            if (!_saveEntryRepository.IsSaveFileExists)
                return;

            _saveEntryRepository.LoadSave();
            _mainMenuUI.LoadGameButton.interactable = false;
            await _sceneService.LoadGameAsync();
        }

        private void OnExit()
        {
            Application.Quit();
        }
    }
}
