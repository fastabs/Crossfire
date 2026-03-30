using Cysharp.Threading.Tasks;
using Lean.Gui;
using Lean.Transition;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class GameOverUI : MonoBehaviour
    {
        [field: SerializeField] public LeanPlayer ShowTransition { get; private set; }
        [field: SerializeField] public TextMeshProUGUI Label { get; private set; }
        [field: SerializeField] public LeanButton GoToMainMenuButton { get; private set; }
        [field: SerializeField] public PauseMenuUI PauseMenuUI { get; private set; }

        private string _labelTemplate;
        private bool _isGameOver;

        private ISceneService _sceneService;
        private IGameStatsService _gameStatsService;
        private ISaveEntryRepository _saveEntryRepository;
        private HealthParameter _playerHealthParameter;
        private IMessageBroker _messageBroker;

        [Inject]
        private void Construct(IMessageBroker messageBroker, ISceneService sceneService,
            IGameStatsService gameStatsService, ISaveEntryRepository saveEntryRepository,
            HealthParameter playerHealthParameter)
        {
            _messageBroker = messageBroker;
            _sceneService = sceneService;
            _gameStatsService = gameStatsService;
            _saveEntryRepository = saveEntryRepository;
            _playerHealthParameter = playerHealthParameter;
        }

        private void Awake()
        {
            if (Label != null)
                _labelTemplate = Label.text;

            if (GoToMainMenuButton != null)
                GoToMainMenuButton.OnClick.AddListener(OnGoToMainMenuClicked);

            _messageBroker.Receive<DeathMessage>()
                .Subscribe(ShowGameOver)
                .AddTo(this);
        }

        private void OnDestroy()
        {
            if (GoToMainMenuButton != null)
                GoToMainMenuButton.OnClick.RemoveListener(OnGoToMainMenuClicked);
        }

        private void ShowGameOver(DeathMessage message)
        {
            if (message.Health != _playerHealthParameter)
                return;

            if (_isGameOver)
                return;

            _isGameOver = true;

            if (Label != null)
            {
                var aliveTime = _gameStatsService.AliveTime;
                var killedEnemies = _gameStatsService.KilledEnemies;
                Label.text = string.Format(_labelTemplate, aliveTime, killedEnemies);
            }

            ShowTransition?.Begin();
            PauseMenuUI?.Hide();
            _saveEntryRepository.DeleteSave();
        }

        private void OnGoToMainMenuClicked()
        {
            GoToMainMenuAsync().Forget();
        }

        private async UniTaskVoid GoToMainMenuAsync()
        {
            if (GoToMainMenuButton != null)
                GoToMainMenuButton.interactable = false;

            await _sceneService.LoadMainMenuAsync();
        }
    }
}
