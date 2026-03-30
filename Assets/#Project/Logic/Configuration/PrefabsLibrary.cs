using System;
using UnityEngine;

namespace Crossfire.Workspace
{
    [CreateAssetMenu(menuName = "Configs/PrefabsLibrary", fileName = nameof(PrefabsLibrary))]
    public sealed class PrefabsLibrary : ScriptableObject
    {
        [field: SerializeField] public UIPrefabs UI { get; private set; }
        [field: SerializeField] public GamePrefabs Game { get; private set; }
        [field: SerializeField] public MaterialsLibrary Materials { get; private set; }

        [Serializable]
        public sealed class UIPrefabs
        {
            [field: SerializeField] public StatUpgradeUI StatUpgradeUIPrefab { get; private set; }
        }

        [Serializable]
        public sealed class GamePrefabs
        {
            [field: SerializeField] public Enemy EnemyPrefab { get; private set; }
            [field: SerializeField] public Projectile ProjectilePrefab { get; private set; }
        }

        [Serializable]
        public sealed class MaterialsLibrary
        {
            [field: SerializeField] public Material PlayerProjectileMaterial { get; private set; }
            [field: SerializeField] public Material EnemyProjectileMaterial { get; private set; }
        }
    }
}
