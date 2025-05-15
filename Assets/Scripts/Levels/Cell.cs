using System;
using DeeDeeR.BrickBreaker.Bricks;
using DeeDeeR.BrickBreaker.PowerUps;
using UnityEngine;

namespace DeeDeeR.BrickBreaker.Levels
{
    [Serializable]
    public class Cell
    {
        [field: SerializeField]
        public Brick Brick  { get; set; }
    }
}