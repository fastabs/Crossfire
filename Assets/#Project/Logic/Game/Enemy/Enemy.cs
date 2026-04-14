using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class Enemy : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Transform ShootPoint { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public AudioSource AudioSource { get; private set; }

        public HealthParameter HealthParameter { get; private set; }
        public IStatsProvider StatsProvider { get; private set; }

        [Inject]
        private void Construct(HealthParameter healthParameter, IStatsProvider statsProvider)
        {
            HealthParameter = healthParameter;
            StatsProvider = statsProvider;
        }

        public void TakeDamage(int damage, Vector3 hitPoint)
        {
            HealthParameter?.TakeDamage(damage, hitPoint);
        }

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
            Rigidbody ??= GetComponent<Rigidbody>();
            Animator ??= GetComponentInChildren<Animator>();
        }
    }
}
