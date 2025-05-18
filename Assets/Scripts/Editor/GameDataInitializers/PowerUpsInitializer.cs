using DeeDeeR.BrickBreaker.PowerUps;
using Editor.FileSystemInitializers;
using UnityEditor;
using UnityEngine;

namespace Editor.GameDataInitializers
{
    public static class PowerUpsInitializer
    {
        [MenuItem("DeeDeeR/BrickBreaker/Game Data/Initializers/Initialize PowerUps")]
        public static void InitializePowerUps()
        {
            FileSystemHelper.EnsureFolderExists(PathHelper.PowerUpsPath);
            
            try
            {
                AssetDatabase.StartAssetEditing();
               
               // Process all powerups using the same pattern
               CreatePowerUp<ExtraLifePowerUp>(NameHelper.PowerUpNames.ExtraLifePowerUp, PathHelper.PowerUpsPath);
               CreatePowerUp<MultiBallPowerUp>(NameHelper.PowerUpNames.MultiBallPowerUp, PathHelper.PowerUpsPath);
               CreatePowerUp<WidePaddlePowerUp>(NameHelper.PowerUpNames.WidePaddlePowerUp, PathHelper.PowerUpsPath);
               CreatePowerUp<LaserPowerUp>(NameHelper.PowerUpNames.LaserPowerUp, PathHelper.PowerUpsPath);
               CreatePowerUp<MagnetPowerUp>(NameHelper.PowerUpNames.MagnetPowerUp, PathHelper.PowerUpsPath);
               CreatePowerUp<SpeedUpPowerUp>(NameHelper.PowerUpNames.SpeedUpPowerUp, PathHelper.PowerUpsPath);
               CreatePowerUp<SlowDownPowerUp>(NameHelper.PowerUpNames.SlowDownPowerUp, PathHelper.PowerUpsPath);
               CreatePowerUp<InvincibilityPowerUp>(NameHelper.PowerUpNames.InvincibilityPowerUp, PathHelper.PowerUpsPath);
               
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
            }
        }
        
        // Helper method to create power-ups consistently
        private static T CreatePowerUp<T>(string name, string path) where T : PowerUp
        {
            var powerUp = ScriptableObjectHelper.CreateScriptableObject<T>(name, path);
            powerUp.PowerUpName = name;
            EditorUtility.SetDirty(powerUp);
            return powerUp;
        }
    }
}