using UnityEngine;

namespace Crossfire.Workspace
{
    public interface ICursorService
    {
        void Show();
        void Hide();
        void Toggle();
        CursorMode CursorMode { get; }
    }

    public enum CursorMode
    {
        Hidden,
        Visible
    }

    public sealed class CursorService : ICursorService
    {
        public CursorMode CursorMode { get; private set; }

        public void Show()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            CursorMode = CursorMode.Visible;
        }

        public void Hide()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            CursorMode = CursorMode.Hidden;
        }

        public void Toggle()
        {
            if (CursorMode is CursorMode.Visible)
                Hide();
            else
                Show();
        }
    }
}
