using System;

namespace Crossfire.Workspace
{
    public interface IConfigRepository
    {
        GameConfig GameConfig { get; }
        PlayerConfig Player { get; }
        EnemyConfig Enemy { get; }
    }
}
