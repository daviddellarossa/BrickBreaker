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
        
        [SerializeField]
        private string id;
        public string Id
        {
            get => id;
            set => id = value;
        }
        
        [SerializeField]
        private Color color;
        public Color Color
        {
            get => color;
            set => color = value;
        }
        
        [SerializeField]
        private int value;
        public int Value
        {
            get => value;
            set => value = value;
        }
        
        [SerializeField]
        private string brickTypeName;
        public string BrickTypeName
        {
            get => brickTypeName;
            set => brickTypeName = value;
        }
        
        public HitPointStrategy HitPointStrategy
        {
            get => hitPointStrategy;
            set => hitPointStrategy = value;
        }
    }
}