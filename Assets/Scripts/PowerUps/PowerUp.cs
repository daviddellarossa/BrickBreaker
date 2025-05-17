using UnityEngine;

namespace DeeDeeR.BrickBreaker.PowerUps
{
    public class PowerUp : ScriptableObject
    {
        [SerializeField] 
        protected float fallSpeed = 2.0f;
        [SerializeField] 
        protected float rotationSpeed = 30.0f;
        [SerializeField] 
        protected string powerUpName;
        
        [SerializeField]
        protected Sprite sprite;
        
        public float FallSpeed
        {
            get => fallSpeed;
            set => fallSpeed = value;
        }
        
        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }
        
        public string PowerUpName
        {
            get => powerUpName;
            set => powerUpName = value;
        }
        
        public Sprite Sprite
        {
            get => sprite;
            set => sprite = value;
        }
    }
}