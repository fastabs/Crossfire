using System;
using UnityEngine;
using UnityEngine.UI;

namespace Crossfire.Workspace
{
    public sealed class CellSliderCell : MonoBehaviour
    {
        [SerializeField] private Graphic graphic;
        [SerializeField] private Color emptyColor;
        [SerializeField] private Color increaseCandidateColor;
        [SerializeField] private Color decreaseCandidateColor;
        [SerializeField] private Color filledColor;

        private CellState _state;
        public CellState State
        {
            get => _state;
            set
            {
                _state = value;

                switch (value)
                {
                    case CellState.Empty:
                        graphic.color = emptyColor;
                        break;
                    case CellState.IncreaseCandidate:
                        graphic.color = increaseCandidateColor;
                        break;
                    case CellState.DecreaseCandidate:
                        graphic.color = decreaseCandidateColor;
                        break;
                    case CellState.Filled:
                        graphic.color = filledColor;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }

        public enum CellState
        {
            Empty,
            IncreaseCandidate,
            DecreaseCandidate,
            Filled
        }
    }
}
