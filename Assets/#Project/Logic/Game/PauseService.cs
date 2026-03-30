using UniRx;

namespace Crossfire.Workspace
{
    public sealed class PauseService
    {
        private readonly BoolReactiveProperty _state = new(false);

        public bool IsPaused => _state.Value;
        public IReadOnlyReactiveProperty<bool> State => _state;

        public void Pause()
        {
            SetState(true);
        }

        public void Unpause()
        {
            SetState(false);
        }

        public void Toggle()
        {
            SetState(!IsPaused);
        }

        private void SetState(bool isPaused)
        {
            if (_state.Value == isPaused)
                return;

            _state.Value = isPaused;
        }
    }
}
