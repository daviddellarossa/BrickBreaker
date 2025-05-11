using UnityEngine;

namespace DeeDeeR.BrickBreaker.PowerUps
{
    public class MultiBallPowerUp : PowerUp
    {
        [SerializeField] private int ballCount = 3;
        [SerializeField] private float spreadAngle = 15f;
        [SerializeField] private float duration = 10f;
    }
}