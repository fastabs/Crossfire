using UnityEngine;
using Zenject;

namespace Crossfire.Workspace
{
    public interface IGameStatsService
    {
        int KilledEnemies { get; set; }
        float AliveTime { get; set; }

        void AddEnemyKill();
    }

    public sealed class GameStatsService : IGameStatsService, ITickable
    {
        public int KilledEnemies { get; set; }
        public float AliveTime { get; set; }

        public void AddEnemyKill()
        {
            KilledEnemies++;
        }

        public void Tick()
        {
            AliveTime += Time.deltaTime;
        }
    }
}

