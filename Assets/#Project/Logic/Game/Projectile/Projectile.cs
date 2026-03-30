using UniRx;
using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class Projectile : MonoBehaviour
    {
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        [field: SerializeField] public Renderer Renderer { get; private set; }

        private int _obstacleLayer;
        private int _damage;
        private int _targetLayer;

        private Vector3 _savedVelocity;
        private Vector3 _savedAngularVelocity;

        private IProjectileLifeController _projectileLifeController;
        private PauseService _pauseService;

        [Inject]
        private void Construct(IProjectileLifeController projectileLifeController, PauseService pauseService)
        {
            _projectileLifeController = projectileLifeController;
            _pauseService = pauseService;
        }

        private void Awake()
        {
            _obstacleLayer = LayerMask.NameToLayer("Obstacle");
            if (_obstacleLayer == -1)
                _obstacleLayer = LayerMask.NameToLayer("Default");

            _pauseService.State
                .Subscribe(OnPauseStateChanged)
                .AddTo(this);
        }

        public void Launch(LaunchParameters parameters)
        {
            _damage = parameters.Damage;
            _targetLayer = parameters.TargetLayer;
            Renderer.material = parameters.Material;

            Rigidbody.position = parameters.LaunchPosition;
            Rigidbody.rotation = parameters.Rotation;
            Rigidbody.velocity = parameters.Velocity;

            _projectileLifeController.StartTimer();
        }

        private void OnPauseStateChanged(bool isPaused)
        {
            if (isPaused)
            {
                _savedVelocity = Rigidbody.velocity;
                _savedAngularVelocity = Rigidbody.angularVelocity;
                Rigidbody.isKinematic = true;
            }
            else
            {
                Rigidbody.isKinematic = false;
                Rigidbody.velocity = _savedVelocity;
                Rigidbody.angularVelocity = _savedAngularVelocity;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherGo = other.gameObject;

            if (otherGo.layer == _obstacleLayer)
            {
                _projectileLifeController.ForceRelease();
                return;
            }

            if (otherGo.layer != _targetLayer)
                return;

            _projectileLifeController.ForceRelease();

            var enemy = otherGo.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(_damage, transform.position);
                return;
            }

            var player = otherGo.GetComponentInParent<Player>();
            player?.TakeDamage(_damage, transform.position);
        }

        public readonly struct LaunchParameters
        {
            public readonly Vector3 LaunchPosition;
            public readonly Quaternion Rotation;
            public readonly Vector3 Velocity;
            public readonly Material Material;
            public readonly int Damage;
            public readonly int TargetLayer;

            public LaunchParameters(Vector3 launchPosition, Quaternion rotation, Vector3 velocity,
                Material material, int damage, int targetLayer)
            {
                LaunchPosition = launchPosition;
                Rotation = rotation;
                Velocity = velocity;
                Material = material;
                Damage = damage;
                TargetLayer = targetLayer;
            }
        }
    }
}
