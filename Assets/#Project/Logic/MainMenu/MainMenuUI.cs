using Lean.Gui;
using UnityEngine;

namespace Crossfire.Workspace
{
    public sealed class MainMenuUI : MonoBehaviour
    {
        [field: SerializeField] public LeanButton NewGameButton { get; private set; }
        [field: SerializeField] public LeanButton LoadGameButton { get; private set; }
        [field: SerializeField] public LeanButton ExitButton { get; private set; }
    }
}
