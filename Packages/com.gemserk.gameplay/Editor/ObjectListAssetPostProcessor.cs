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
                    shouldRegenerate = asset.StartsWith(objectListAsset.path);
                    
                    if (!string.IsNullOrEmpty(objectListAsset.validExtension))
                    {
                        shouldRegenerate = shouldRegenerate 
                                           && asset.EndsWith(objectListAsset.validExtension);
                    }
                }
                
                foreach (var asset in deletedAssets)
                {
                    shouldRegenerate = asset.StartsWith(objectListAsset.path);

                    if (!string.IsNullOrEmpty(objectListAsset.validExtension))
                    {
                        shouldRegenerate = shouldRegenerate 
                                           && asset.EndsWith(objectListAsset.validExtension);
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