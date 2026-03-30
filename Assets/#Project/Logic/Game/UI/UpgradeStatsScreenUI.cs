using System.Collections.Generic;
using Lean.Gui;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class UpgradeStatsScreenUI : MonoBehaviour
    {
        [field: SerializeField] public LeanWindow LeanWindow { get; private set; }
        [field: SerializeField] public RectTransform StatUpgradesContainer { get; private set; }
        [field: SerializeField] public TextMeshProUGUI UpgradeCountLabel { get; private set; }
        [field: SerializeField] public LeanButton ApplyUpgradesButton { get; private set; }
        [field: SerializeField] public LeanButton CancelUpgradeButton { get; private set; }

        private readonly List<StatUpgradeUI> _statUpgrades = new();

        private string _upgradeCountTemplate;
        private PrefabsLibrary _prefabs;
        private StatsUpgradeService _statsUpgradeService;
        private IMessageBroker _messageBroker;

        [Inject]
        private void Construct(PrefabsLibrary prefabs, StatsUpgradeService statsUpgradeService, IMessageBroker messageBroker)
        {
            _prefabs = prefabs;
            _statsUpgradeService = statsUpgradeService;
            _messageBroker = messageBroker;
        }

        private void Awake()
        {
            if (UpgradeCountLabel != null)
                _upgradeCountTemplate = UpgradeCountLabel.text;

            ApplyUpgradesButton?.OnClick.AddListener(_statsUpgradeService.ApplyUpgrade);
            CancelUpgradeButton?.OnClick.AddListener(_statsUpgradeService.CancelUpgrade);

            _messageBroker.Receive<StatUpgradeCreatedMessage>()
                .Subscribe(OnStatUpgradeCreated)
                .AddTo(this);

            _messageBroker.Receive<StatUpgradeValueMessage>()
                .Subscribe(OnStatUpgradeValue)
                .AddTo(this);

            _messageBroker.Receive<UpgradeAppliedMessage>()
                .Subscribe(_ => Hide())
                .AddTo(this);

            _messageBroker.Receive<UpgradeCanceledMessage>()
                .Subscribe(_ => Hide())
                .AddTo(this);

            _statsUpgradeService.CurrentAvailableUpgradeCountObservable
                .Subscribe(SetUpgradesCount)
                .AddTo(this);
        }

        private void OnDestroy()
        {
            ApplyUpgradesButton?.OnClick.RemoveListener(_statsUpgradeService.ApplyUpgrade);
            CancelUpgradeButton?.OnClick.RemoveListener(_statsUpgradeService.CancelUpgrade);
        }

        public void Show()
        {
            RedrawStats();
            LeanWindow?.TurnOn();
        }

        public void Hide()
        {
            LeanWindow?.TurnOff();
        }

        public void SetUpgradesCount(int count)
        {
            if (UpgradeCountLabel != null)
                UpgradeCountLabel.text = string.Format(_upgradeCountTemplate, count);

            UpdateInteractables();
        }

        private void OnStatUpgradeCreated(StatUpgradeCreatedMessage message)
        {
            CreateStatUpgradeUI(message.Stat);
        }

        private void OnStatUpgradeValue(StatUpgradeValueMessage message)
        {
            SetStatValue(message.Stat, message.Value);
        }

        private void CreateStatUpgradeUI(Stat stat)
        {
            var statUpgradePrefab = _prefabs.UI?.StatUpgradeUIPrefab;
            if (statUpgradePrefab == null || StatUpgradesContainer == null)
            {
                Debug.LogWarning($"{nameof(UpgradeStatsScreenUI)} requires {nameof(PrefabsLibrary)}.{nameof(PrefabsLibrary.UIPrefabs)}.{nameof(PrefabsLibrary.UIPrefabs.StatUpgradeUIPrefab)}.");
                return;
            }

            var statUpgradeUI = Instantiate(statUpgradePrefab, StatUpgradesContainer);
            statUpgradeUI.Setup(stat, _statsUpgradeService);
            statUpgradeUI.SetValue(stat.CurrentUpgradeLevel);
            _statUpgrades.Add(statUpgradeUI);
            UpdateInteractables();
        }

        private void SetStatValue(Stat stat, int value)
        {
            foreach (var statUpgradeUI in _statUpgrades)
            {
                if (statUpgradeUI.Stat == stat)
                    statUpgradeUI.SetValue(value);
            }

            UpdateInteractables();
        }

        private void RedrawStats()
        {
            foreach (var statUpgradeUI in _statUpgrades)
                statUpgradeUI.SetValue(statUpgradeUI.Stat.CurrentUpgradeLevel);

            UpdateInteractables();
        }

        private void UpdateInteractables()
        {
            foreach (var statUpgradeUI in _statUpgrades)
            {
                var isAddPointInteractable = _statsUpgradeService.CurrentAvailableUpgradeCount > 0 &&
                    statUpgradeUI.CurrentValue < statUpgradeUI.Stat.MaxUpgradeLevel;
                var isRemovePointInteractable = statUpgradeUI.CurrentValue > 0;

                statUpgradeUI.AddPoint.interactable = isAddPointInteractable;
                statUpgradeUI.RemovePoint.interactable = isRemovePointInteractable;
            }
        }
    }
}
