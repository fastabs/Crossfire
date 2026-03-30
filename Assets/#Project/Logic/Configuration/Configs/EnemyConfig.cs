using System;

namespace Crossfire.Workspace
{
    [Serializable]
    public struct EnemyConfig
    {
        public float DirectionChangeInterval;
        public float SpawnInterval;
        public float SpawnRadius;
        public FloatRange FlyHeight;
        public float RotationSpeed;
        public float ShootInterval;
        public float ProjectileSpeed;
        public float DeathVelocityForce;
        public BaseStatsConfig BaseStats;
    }
}

