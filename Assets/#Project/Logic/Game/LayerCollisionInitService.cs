using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public sealed class LayerCollisionInitService : IInitializable
    {
        private readonly Player _player;

        public LayerCollisionInitService(Player player)
        {
            _player = player;
        }

        public void Initialize()
        {
            var playerLayer = LayerMask.NameToLayer("Player");
            var enemyLayer = LayerMask.NameToLayer("Enemy");

            if (playerLayer >= 0 && enemyLayer >= 0)
                Physics.IgnoreLayerCollision(playerLayer, enemyLayer, true);

            if (_player != null)
            {
                if (playerLayer >= 0)
                    SetLayerRecursively(_player.transform, playerLayer);

                if (_player.Rigidbody != null)
                    _player.Rigidbody.freezeRotation = true;
            }
        }

        private static void SetLayerRecursively(Transform root, int layer)
        {
            root.gameObject.layer = layer;
            for (var i = 0; i < root.childCount; i++)
                SetLayerRecursively(root.GetChild(i), layer);
        }
    }
}

