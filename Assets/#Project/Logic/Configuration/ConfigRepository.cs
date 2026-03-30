using UnityEngine;

namespace Crossfire.Workspace
{
    [CreateAssetMenu(menuName = "Configs/ConfigsRepository", fileName = nameof(ConfigRepository))]
    public sealed class ConfigRepository : ScriptableObject, IConfigRepository
    {
        [field: SerializeField] public GameConfig GameConfig { get; private set; }
        [field: SerializeField] public PlayerConfig Player { get; private set; }
        [field: SerializeField] public EnemyConfig Enemy { get; private set; }
    }
}


