using Lean.Gui;
using UnityEngine;

namespace Crossfire.Workspace
{
    public sealed class StatUpgradeUI : MonoBehaviour
    {
        [field: SerializeField] public CellSlider CellSlider { get; private set; }
        [field: SerializeField] public LeanButton AddPoint { get; private set; }
        [field: SerializeField] public LeanButton RemovePoint { get; private set; }

        public Stat Stat { get; private set; }
        public int CurrentValue { get; private set; }

        public void Setup(Stat stat, StatsUpgradeService statsUpgradeService)
        {
            Stat = stat;
            CellSlider.Init(stat.MaxUpgradeLevel, stat.Title);

            AddPoint.OnClick.AddListener(() => statsUpgradeService.TryChangeStat(stat, 1));
            RemovePoint.OnClick.AddListener(() => statsUpgradeService.TryChangeStat(stat, -1));
        }

        public void SetValue(int currentValue)
        {
            CurrentValue = currentValue;
            CellSlider.SetValue(Stat.CurrentUpgradeLevel, currentValue);
        }
    }
}
