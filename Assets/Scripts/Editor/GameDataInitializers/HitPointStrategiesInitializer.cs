using DeeDeeR.BrickBreaker.HitPointStrategies;
using Editor.FileSystemInitializers;
using UnityEditor;

namespace Editor.GameDataInitializers
{
    public class HitPointStrategiesInitializer
    {
        public static HitPointStrategy[] GetAllHitPointStrategies()
        {
            return ScriptableObjectHelper.GetAllScriptableObjects<HitPointStrategy>(PathHelper.HitPointStrategiesPath);
        }

        [MenuItem("DeeDeeR/BrickBreaker/Game Data/Initializers/Initialize HitPoint Strategies")]
        public static void InitializeHitPointStrategies()
        {
            try
            {
                AssetDatabase.StartAssetEditing();
    
                FileSystemHelper.EnsureFolderExists(PathHelper.HitPointStrategiesPath);
                
                var constantHitPointStrategy = ScriptableObjectHelper.CreateScriptableObject<DecreaseHitPointStrategy>(NameHelper.HitPointStrategies.ConstantHitPointStrategy, PathHelper.HitPointStrategiesPath);
                constantHitPointStrategy.DecreaseBy = 0;
                EditorUtility.SetDirty(constantHitPointStrategy);
                
                var decreaseHitPointStrategy = ScriptableObjectHelper.CreateScriptableObject<DecreaseHitPointStrategy>(NameHelper.HitPointStrategies.DecreaseHitPointStrategy, PathHelper.HitPointStrategiesPath);
                decreaseHitPointStrategy.DecreaseBy = 1;
                EditorUtility.SetDirty(decreaseHitPointStrategy);
                
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