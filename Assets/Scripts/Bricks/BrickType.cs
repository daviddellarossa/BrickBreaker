using System;
using DeeDeeR.BrickBreaker.HitPointStrategies;
using UnityEngine;

namespace DeeDeeR.BrickBreaker.Bricks
{
    [CreateAssetMenu(fileName = "NewBrickType", menuName = "BrickBreaker/Brick Type", order = 2)]
    public class BrickType : ScriptableObject
    {
        [SerializeField]
        private string id;
        
        [SerializeField]
        private Color color;
        
        [SerializeField]
        private int value;
        
        [SerializeField]
        [SerializeReference]
        private HitPointStrategy hitPointStrategy;
        
        public string Id
        {
            get => id;
            set => id = value;
        }
        
        public Color Color
        {
            get => color;
            set => color = value;
        }
        
        public int Value
        {
            get => value;
            set => this.value = value;
        }
        
        public HitPointStrategy HitPointStrategy
        {
            get => hitPointStrategy;
            set => hitPointStrategy = value;
        }
    }
}