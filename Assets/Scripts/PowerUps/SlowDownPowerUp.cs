using UnityEngine;

namespace DeeDeeR.BrickBreaker.PowerUps
{
    public class SlowDownPowerUp : PowerUp
    {
        [SerializeField] private float speedReductionFactor = 0.5f;
        [SerializeField] private float duration = 10f;
    }
}