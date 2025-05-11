using System;
using System.Linq;
using DeeDeeR.BrickBreaker.Bricks;
using DeeDeeR.BrickBreaker.HitPointStrategies;
using Editor.FileSystemInitializers;
using Editor.GameDataInitializers;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class BricksInitializer
    {
        private static HitPointStrategy[] HitPointStrategies = HitPointStrategiesInitializer.GetAllHitPointStrategies();
        
        public static BrickType[] GetAllBrickTypes()
        {
            return ScriptableObjectHelper.GetAllScriptableObjects<BrickType>(PathHelper.BrickTypesPath);
        }
        
        [MenuItem("DeeDeeR/BrickBreaker/Game Data/Initializers/Initialize Bricks")]
        public static void InitializeBricks()
        {
            var decreaseHitPointInitializer = HitPointStrategies.Single(x => x.name == NameHelper.HitPointStrategies.DecreaseHitPointStrategy);
            var constantHitPointInitializer = HitPointStrategies.Single(x => x.name == NameHelper.HitPointStrategies.ConstantHitPointStrategy);
            
            FileSystemHelper.EnsureFolderExists(PathHelper.BrickTypesPath);
            
            try
            {
                AssetDatabase.StartAssetEditing();
                
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e467", NameHelper.BrickTypes.Blue, Color.blue, 100, decreaseHitPointInitializer);
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e468", NameHelper.BrickTypes.Cyan, Color.cyan, 70, decreaseHitPointInitializer);
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e469", NameHelper.BrickTypes.Gold, Color.lightCoral, 0, constantHitPointInitializer);
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e470", NameHelper.BrickTypes.Green, Color.green, 80, decreaseHitPointInitializer);
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e471", NameHelper.BrickTypes.Magenta, Color.magenta, 110, decreaseHitPointInitializer);
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e472", NameHelper.BrickTypes.Orange, Color.darkOrange, 60, decreaseHitPointInitializer); 
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e473", NameHelper.BrickTypes.Red, Color.red, 90, decreaseHitPointInitializer);
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e474", NameHelper.BrickTypes.Silver, Color.silver, 0, decreaseHitPointInitializer);
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e475", NameHelper.BrickTypes.White, Color.white, 50, decreaseHitPointInitializer);
                CreateBrickType("08d9c510-3c59-4ce1-7f98-e3230158e476", NameHelper.BrickTypes.Yellow, Color.yellow, 120, decreaseHitPointInitializer);
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }
            
        private static void CreateBrickType(string id, string name, Color color, int value, HitPointStrategy strategy)
        {
            var brick = ScriptableObjectHelper.CreateScriptableObject<BrickType>(name, PathHelper.BrickTypesPath);
            brick.Id = id;
            brick.Color = color;
            brick.Value = value;
            brick.HitPointStrategy = strategy;
            EditorUtility.SetDirty(brick);
        }
    }
}