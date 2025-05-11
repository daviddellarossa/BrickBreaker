using System.Collections.Generic;
using System.Linq;
using DeeDeeR.BrickBreaker.Bricks;
using DeeDeeR.BrickBreaker.Levels;
using Editor.FileSystemInitializers;
using UnityEditor;
using UnityEngine;

namespace Editor.GameDataInitializers
{
    public class LevelInitializerTest
    {
        private static BrickType[] BrickTypes = BricksInitializer.GetAllBrickTypes();
        
        [MenuItem("DeeDeeR/BrickBreaker/Game Data/Initializers/Initialize Empty level")]
        public static void InitializeEmptyLevel()
        {
            try
            {
                var blueBrick = BrickTypes.Single(x => x.Color == Color.blue);
                AssetDatabase.StartAssetEditing();
    
                FileSystemHelper.EnsureFolderExists(PathHelper.LevelsPath);
                var numColumns = 14;
                var numRows = 14;
                var totalNumberOfCells = numColumns * numRows;
                
                var levelTest = ScriptableObjectHelper.CreateScriptableObject<Level>($"Level_test", PathHelper.LevelsPath);
                levelTest.NumColumns = numColumns;
                levelTest.NumRows = numRows;
                
                levelTest.Cells = new List<Cell>(totalNumberOfCells);
                
                for (int i = 0; i < totalNumberOfCells; i++)
                {
                    levelTest.Cells.Add(new Cell()
                    {
                        Brick = new Brick()
                        {
                            BrickType = blueBrick,
                            HitPoints = 4,
                            InitialHitPoints = 4
                        }
                    });
                }

                EditorUtility.SetDirty(levelTest);
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }
    }
}