using System.Collections.Generic;
using UnityEngine;

namespace DeeDeeR.BrickBreaker.Levels
{
    [CreateAssetMenu(fileName = "New Level", menuName = "BrickBreaker/Levels/New Level", order = 1)]
    public class Level : ScriptableObject
    {
        private static readonly int DefaultRowNum = 14;
        private static readonly int DefaultColNum = 14;
        
        [SerializeField]
        private List<Cell> cells = new List<Cell>();
        public List<Cell> Cells
        {
            get => cells;
            set => cells = value;
        }

        [SerializeField]
        private int RowNum = DefaultRowNum;
        public int NumRows
        {
            get => RowNum;
            set => RowNum = value;
        }
        
        [SerializeField]
        private int ColNum = DefaultColNum;
        public int NumColumns 
        {
            get => ColNum;
            set => ColNum = value;
        }
    }
}