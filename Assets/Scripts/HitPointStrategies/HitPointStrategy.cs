using UnityEngine;

namespace DeeDeeR.BrickBreaker.HitPointStrategies
{
    public abstract class HitPointStrategy : ScriptableObject
    {
        public abstract int GetHitPoints(int currentHitPoints);
    }
}