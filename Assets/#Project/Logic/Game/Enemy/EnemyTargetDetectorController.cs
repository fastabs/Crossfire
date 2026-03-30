using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public interface IEnemyTargetDetector
    {
        Transform Target { get; }
    }

    public sealed class EnemyTargetDetectorController : IEnemyTargetDetector, ITickable
    {
        public Transform Target { get; private set; }

        private readonly Enemy _enemy;
        private readonly IPlayerLocator _playerLocator;

        public EnemyTargetDetectorController(IPlayerLocator playerLocator, Enemy enemy)
        {
            _playerLocator = playerLocator;
            _enemy = enemy;
        }

        public void Tick()
        {
            DetectPlayer();
        }

        private void DetectPlayer()
        {
            if (_playerLocator.Player == null)
            {
                Target = null;
                return;
            }

            var player = _playerLocator.Player.transform;
            var enemyTransform = _enemy.transform;
            var direction = player.position - enemyTransform.position;
            var distance = direction.magnitude;

            if (distance <= Mathf.Epsilon)
            {
                Target = player;
                return;
            }

            if (!Physics.Raycast(enemyTransform.position, direction.normalized, out var hit, distance))
            {
                Target = null;
                return;
            }

            Target = hit.transform == player || hit.transform.IsChildOf(player) ? player : null;
        }
    }
}
