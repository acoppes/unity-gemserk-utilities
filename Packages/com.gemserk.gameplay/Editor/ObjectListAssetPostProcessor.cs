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

                var regex = objectListAsset.regex;
                
                foreach (var assetPath in importedAssets)
                {
                    shouldRegenerate = assetPath.StartsWith(objectListAsset.path);
                    
                    if (!string.IsNullOrEmpty(objectListAsset.pattern))
                    {
                        shouldRegenerate = shouldRegenerate 
                                           && regex.IsMatch(assetPath);
                    }
                }
                
                foreach (var assetPath in deletedAssets)
                {
                    shouldRegenerate = assetPath.StartsWith(objectListAsset.path);

                    if (!string.IsNullOrEmpty(objectListAsset.pattern))
                    {
                        shouldRegenerate = shouldRegenerate 
                                           && regex.IsMatch(assetPath);
                    }
                }

                if (shouldRegenerate)
                {
                    objectListAsset.Reload();
                }
                
                AssetDatabase.SaveAssetIfDirty(objectListAsset);    
            }
        }
    }
}