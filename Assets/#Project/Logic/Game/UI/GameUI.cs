using UnityEngine;

namespace Crossfire.Workspace
{
    public sealed class GameUI : MonoBehaviour
    {
        [field: SerializeField] public PlayerHealthBarUI PlayerHealthBarUI { get; private set; }
        [field: SerializeField] public GameOverUI GameOverUI { get; private set; }
        [field: SerializeField] public PauseMenuUI PauseMenuUI { get; private set; }
        [field: SerializeField] public Crosshair Crosshair { get; private set; }
        [field: SerializeField] public UpgradeNotificationUI UpgradeNotificationUI { get; private set; }

        private void Reset()
        {
            AutoAssign();
        }

        private void OnValidate()
        {
            AutoAssign();
        }

        private void AutoAssign()
        {
            PlayerHealthBarUI ??= GetComponentInChildren<PlayerHealthBarUI>(true);
            GameOverUI ??= GetComponentInChildren<GameOverUI>(true);
            PauseMenuUI ??= GetComponentInChildren<PauseMenuUI>(true);
            Crosshair ??= GetComponentInChildren<Crosshair>(true);
            UpgradeNotificationUI ??= GetComponentInChildren<UpgradeNotificationUI>(true);
        }
    }
}
