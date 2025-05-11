using System;
using Unity.VisualScripting;
using UnityEngine;

namespace DeeDeeR.BrickBreaker.Bricks
{
    [Serializable]
    public class Brick
    {
        [SerializeField]
        private BrickType _brickType;
        public BrickType BrickType
        {
            get => _brickType;
            set => _brickType = value;
        }

        [SerializeField]
        private int _initialHitPoints;
        public int InitialHitPoints 
        {
            get => _initialHitPoints;
            set => _initialHitPoints = value;
        }

        [SerializeField]
        private int _hitPoints;
        public int HitPoints
        {
            get => _hitPoints;
            set => _hitPoints = value;
        }
    }
}