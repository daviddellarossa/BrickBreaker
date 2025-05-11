using UnityEngine;

namespace DeeDeeR.BrickBreaker.HitPointStrategies
{
    [CreateAssetMenu(fileName = "DecreaseHitPointStrategy", menuName = "BrickBreaker/Hit Point Strategies/Decrease", order = 2)]
    public class DecreaseHitPointStrategy : HitPointStrategy
    {
        [SerializeField] private int decreaseBy = 1;
        public int DecreaseBy
        {
            get => decreaseBy;
            set => decreaseBy = value;
        }
        
        
        public override int GetHitPoints(int currentHitPoints)
        {
            
            return currentHitPoints > 0 ? currentHitPoints - decreaseBy : 0;
        }
    }
}
