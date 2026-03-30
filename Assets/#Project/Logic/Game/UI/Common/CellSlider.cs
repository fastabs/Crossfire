using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Crossfire.Workspace
{
    public sealed class CellSlider : MonoBehaviour
    {
        [SerializeField] private RectTransform container;
        [SerializeField] private TextMeshProUGUI titleLabel;
        [SerializeField] private CellSliderCell cellTemplate;

        public int BaseValue { get; private set; }
        public int CurrentValue { get; private set; }

        private List<CellSliderCell> _cells;

        public void Init(int cellCount, string title)
        {
            _cells = new List<CellSliderCell>(cellCount);

            for (var i = 0; i < cellCount; i++)
            {
                var cell = Instantiate(cellTemplate, container);
                cell.gameObject.SetActive(true);
                cell.name = $"Cell {i}";
                _cells.Add(cell);
            }

            titleLabel.text = title;
        }

        public void SetValue(int baseValue, int currentValue)
        {
            BaseValue = baseValue;
            CurrentValue = currentValue;

            for (var i = 0; i < _cells.Count; i++)
            {
                if (baseValue <= currentValue)
                {
                    if (i + 1 <= baseValue)
                        _cells[i].State = CellSliderCell.CellState.Filled;
                    else if (i + 1 <= currentValue)
                        _cells[i].State = CellSliderCell.CellState.IncreaseCandidate;
                    else
                        _cells[i].State = CellSliderCell.CellState.Empty;
                }
                else
                {
                    if (i + 1 <= currentValue)
                        _cells[i].State = CellSliderCell.CellState.Filled;
                    else if (i + 1 <= baseValue)
                        _cells[i].State = CellSliderCell.CellState.DecreaseCandidate;
                    else
                        _cells[i].State = CellSliderCell.CellState.Empty;
                }
            }
        }
    }
}
