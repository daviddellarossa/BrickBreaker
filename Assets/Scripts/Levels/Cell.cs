using System;
using DeeDeeR.BrickBreaker.Bricks;
using DeeDeeR.BrickBreaker.PowerUps;
using UnityEngine;
using UnityEngine.Serialization;

namespace DeeDeeR.BrickBreaker.Levels
{
    [Serializable]
    public class Cell
    {
        [SerializeField]
        private Brick brick;
        
        [SerializeField]
        private PowerUp powerUp;
        
        public Brick Brick
        {
            get => brick;
            set => brick = value;
        }
        
        public PowerUp PowerUp
        {
            get => powerUp;
            set => powerUp = value;
        }
    }
}