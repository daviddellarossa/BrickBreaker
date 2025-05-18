using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DeeDeeR.BrickBreaker.Bricks
{
    [Serializable]
    public class Brick
    {

        [field:SerializeField]
        public BrickType BrickType { get; set; }

        [field:SerializeField]
        public int InitialHitPoints  { get; set; }

        [field:SerializeField]
        public int HitPoints  { get; set; }
    }
}