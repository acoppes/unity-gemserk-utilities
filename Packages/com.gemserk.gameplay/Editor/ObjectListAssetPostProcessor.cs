using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Gameplay.Editor
{
    public class ObjectListAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var objectListAssets = AssetDatabase.FindAssets("t:ObjectListAsset")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<ObjectListAsset>).ToList();
            
            foreach (var objectListAsset in objectListAssets)
            {
                if (string.IsNullOrEmpty(objectListAsset.path))
                    continue;

                var shouldRegenerate = false;

                foreach (var asset in importedAssets)
                {
                    shouldRegenerate = asset.StartsWith(objectListAsset.path); // && asset.EndsWith(".prefab");
                }
                
                foreach (var asset in deletedAssets)
                {
                    shouldRegenerate = asset.StartsWith(objectListAsset.path); // && asset.EndsWith(".prefab");
                }

                if (shouldRegenerate)
                {
                    objectListAsset.assets.Clear();

                    objectListAsset.assets = AssetDatabase.FindAssets("t:Object", new[]
                        {
                            objectListAsset.path
                        })
                        .Select(AssetDatabase.GUIDToAssetPath)
                        .Select(AssetDatabase.LoadAssetAtPath<Object>).ToList();

                    EditorUtility.SetDirty(objectListAsset);
                }
                
                AssetDatabase.SaveAssetIfDirty(objectListAsset);    
            }
        }
    }
}