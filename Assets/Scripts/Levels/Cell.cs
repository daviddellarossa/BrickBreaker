using System;
using DeeDeeR.BrickBreaker.Bricks;
using DeeDeeR.BrickBreaker.PowerUps;
using UnityEngine;

namespace DeeDeeR.BrickBreaker.Levels
{
    [Serializable]
    public class Cell
    {
        [SerializeField]
        private Brick brick;
        public Brick Brick 
        {
            get => brick;
            set => brick = value;
        }
    }
}