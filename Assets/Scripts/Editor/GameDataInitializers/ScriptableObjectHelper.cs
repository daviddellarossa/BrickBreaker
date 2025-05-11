using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.GameDataInitializers
{
    public static class ScriptableObjectHelper
    {
        public static T CreateScriptableObject<T>(string fileName, string folderPath) where T : ScriptableObject
        {
            string assetPath = Path.Combine(folderPath, fileName + ".asset");

            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, assetPath);
                Debug.Log($"{fileName} created.");
            }
            else
            {
                Debug.LogWarning($"{fileName} already exists.");
            }

            return asset;
        }
        
        public static T[] GetAllScriptableObjects<T>(string folderPath) where T : ScriptableObject
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                {
                    assets.Add(asset);
                }
            }

            return assets.ToArray();
        }

    }
}