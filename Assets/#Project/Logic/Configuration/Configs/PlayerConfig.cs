using System;

namespace Crossfire.Workspace
{
    [Serializable]
    public struct PlayerConfig
    {
        public float RotationSpeed;
        public float ProjectileSpeed;
        public BaseStatsConfig BaseStats;
    }

    [Serializable]
    public struct BaseStatsConfig
    {
        public StatInfo MaxHealthStatInfo;
        public StatInfo MoveSpeedStatInfo;
        public StatInfo DamageStatInfo;

        [Serializable]
        public struct StatInfo
        {
            public string TitleKey;
            public int BaseValue;
            public int MaxUpgradeLevel;
            public int ValuePerUpgradeLevel;
        }
    }
}
