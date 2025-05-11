using System;
using Editor.FileSystemInitializers;
using Editor.GameDataInitializers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class GameDataInitializer
    {
        [MenuItem("DeeDeeR/BrickBreaker/Game Data/Initialize")]
        public static void InitializeGameData()
        {
            Debug.Log("Initializing Game Data");
            try 
            {
                FileSystemHelper.EnsureFolderExists(PathHelper.FolderPath, true);

                HitPointStrategiesInitializer.InitializeHitPointStrategies();
                
                PowerUpsInitializer.InitializePowerUps();
                
                BricksInitializer.InitializeBricks();
                
                LevelInitializerTest.InitializeEmptyLevel();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            Debug.Log("Game Data initialization completed");
        }
    }
}