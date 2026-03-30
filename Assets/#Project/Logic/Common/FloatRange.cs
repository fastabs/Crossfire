using System;
using Random = UnityEngine.Random;

namespace Crossfire.Workspace
{
    [Serializable]
    public struct FloatRange
    {
        public float Min;
        public float Max;

        public float GetRandom()
        {
            return Random.Range(Min, Max);
        }
    }
}

