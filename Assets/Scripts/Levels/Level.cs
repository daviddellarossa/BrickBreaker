using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DeeDeeR.BrickBreaker.Levels
{
    [CreateAssetMenu(fileName = "New Level", menuName = "BrickBreaker/Levels/New Level", order = 1)]
    public class Level : ScriptableObject
    {
        private static readonly int DefaultRowNum = 14;
        private static readonly int DefaultColNum = 14;
        
        
        [FormerlySerializedAs("_cells")] [SerializeField]
        private List<Cell> cells = new List<Cell>();
        
        public List<Cell> Cells
        {
            get => cells;
            set => cells = value;
        }

        
        
        [SerializeField]
        private int[] intValues = { 1, 2, 3 };
        public int[] IntValues
        {
            get => intValues;
            set => intValues = value;
        }
        
        [SerializeField]
        private int numRows;
        public int NumRows
        {
            get => numRows;
            set => numRows = value;
        }
        
        [SerializeField]
        private int numColumns;
        public int NumColumns
        {
            get => numColumns;
            set => numColumns = value;
        }
        
        [SerializeField]
        private string levelName;
        public string LevelName
        {
            get => levelName;
            set => levelName = value;
        }
        
        [SerializeField]
        private int sortingOrder;
        public int SortingOrder
        {
            get => sortingOrder;
            set => sortingOrder = value;
        }
        
    }
}