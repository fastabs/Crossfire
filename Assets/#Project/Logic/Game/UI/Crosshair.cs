using Lean.Transition;
using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class Crosshair : MonoBehaviour
    {
        [field: SerializeField] public LeanPlayer HitTransition { get; private set; }

        private IMessageBroker _messageBroker;

        [Inject]
        private void Construct(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        private void Awake()
        {
            _messageBroker.Receive<HitEnemyMessage>()
                .Subscribe(_ => ShowHit())
                .AddTo(this);
        }

        public void ShowHit()
        {
            HitTransition?.Begin();
        }
    }
}
