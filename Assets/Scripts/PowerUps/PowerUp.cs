using UnityEngine;

namespace DeeDeeR.BrickBreaker.PowerUps
{
    public abstract class PowerUp : ScriptableObject
    {
        [SerializeField] protected float fallSpeed = 2.0f;
        [SerializeField] protected float rotationSpeed = 30.0f;

    }
}