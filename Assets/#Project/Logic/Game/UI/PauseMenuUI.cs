using Cysharp.Threading.Tasks;
using Lean.Gui;
using Lean.Transition;
using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class PauseMenuUI : MonoBehaviour
    {
        [field: SerializeField] public LeanButton UpgradeStatsButton { get; private set; }
        [field: SerializeField] public LeanButton ExitToMainMenuButton { get; private set; }
        [field: SerializeField] public UpgradeStatsScreenUI UpgradeStatsScreenUI { get; private set; }
        [field: SerializeField] public LeanPlayer ShowTransition { get; private set; }
        [field: SerializeField] public LeanPlayer HideTransition { get; private set; }

        private ISaveGameService _saveGameService;
        private ISceneService _sceneService;
        private PauseService _pauseService;

        [Inject]
        private void Construct(ISaveGameService saveGameService, ISceneService sceneService,
            PauseService pauseService)
        {
            _saveGameService = saveGameService;
            _sceneService = sceneService;
            _pauseService = pauseService;
        }

        private void Awake()
        {
            if (UpgradeStatsButton != null)
                UpgradeStatsButton.OnClick.AddListener(OnUpgradeStatsClicked);

            if (ExitToMainMenuButton != null)
                ExitToMainMenuButton.OnClick.AddListener(OnExitToMainMenuClicked);

            _pauseService.State
                .Skip(1)
                .Subscribe(OnPauseStateChanged)
                .AddTo(this);
        }

        private void OnDestroy()
        {
            if (UpgradeStatsButton != null)
                UpgradeStatsButton.OnClick.RemoveListener(OnUpgradeStatsClicked);

            if (ExitToMainMenuButton != null)
                ExitToMainMenuButton.OnClick.RemoveListener(OnExitToMainMenuClicked);
        }

        public void Show()
        {
            ShowTransition?.Begin();
        }

        public void Hide()
        {
            HideTransition?.Begin();
        }

        private void OnPauseStateChanged(bool isPaused)
        {
            if (isPaused)
                Show();
            else
                Hide();
        }

        private void OnUpgradeStatsClicked()
        {
            UpgradeStatsScreenUI?.Show();
        }

        private void OnExitToMainMenuClicked()
        {
            ExitToMainMenuAsync().Forget();
        }

        private async UniTaskVoid ExitToMainMenuAsync()
        {
            if (ExitToMainMenuButton != null)
                ExitToMainMenuButton.interactable = false;

            _saveGameService.SaveGame();
            await _sceneService.LoadMainMenuAsync();
        }
    }
}
