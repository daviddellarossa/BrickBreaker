using System;
using DeeDeeR.BrickBreaker.HitPointStrategies;
using UnityEngine;

namespace DeeDeeR.BrickBreaker.Bricks
{
    [CreateAssetMenu(fileName = "NewBrickType", menuName = "BrickBreaker/Brick Type", order = 2)]
    public class BrickType : ScriptableObject
    {
        [SerializeField]
        [SerializeReference]
        private HitPointStrategy hitPointStrategy;
        
        [field:SerializeField]
        public string Id { get; set; }
        
        [field:SerializeField]
        public Color Color { get; set; }
        
        [field:SerializeField]
        public int Value { get; set; }
        
        public HitPointStrategy HitPointStrategy
        {
            get => hitPointStrategy;
            set => hitPointStrategy = value;
        }
    }
}