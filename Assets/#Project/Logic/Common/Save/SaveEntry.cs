using System;
using UnityEngine;

namespace Crossfire.Workspace
{
    [Serializable]
    public sealed class SaveEntry
    {
        public GameStatisticsData GameStatistics;
        public PlayerData Player;
        public EnemyData[] Enemies;

        [Serializable]
        public struct GameStatisticsData
        {
            public int KilledEnemies;
            public float AliveTime;
        }

        [Serializable]
        public struct PlayerData
        {
            public Vector3 Position;
            public Vector3 EulerAnglesRotation;
            public int Health;
            public int MaxHealthUpgradeLevel;
            public int MoveSpeedUpgradeLevel;
            public int DamageUpgradeLevel;
            public int AvailableUpgradeCount;
        }

        [Serializable]
        public struct EnemyData
        {
            public Vector3 Position;
            public int Health;
            public int MaxHealthUpgradeLevel;
            public int MoveSpeedUpgradeLevel;
            public int DamageUpgradeLevel;
        }
    }
}
