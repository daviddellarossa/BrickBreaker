using UnityEngine;

namespace DeeDeeR.BrickBreaker.PowerUps
{
    public class SpeedUpPowerUp : PowerUp
    {
        [SerializeField] private float speedIncreaseFactor = 1.5f;
        [SerializeField] private float duration = 10f;
    }
}