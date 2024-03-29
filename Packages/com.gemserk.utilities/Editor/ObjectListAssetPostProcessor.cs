﻿using System.Linq;
using UnityEditor;

namespace Gemserk.Utilities.Editor
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
                foreach (var normalizedAssetPath in objectListAsset.objectList.normalizedAssetPaths)
                {
                    if (string.IsNullOrEmpty(normalizedAssetPath))
                        continue;

                    var shouldRegenerate = false;

                    var regex = objectListAsset.objectList.regex;
                
                    foreach (var assetPath in importedAssets)
                    {
                        shouldRegenerate = assetPath.StartsWith(normalizedAssetPath);
                    
                        if (!string.IsNullOrEmpty(objectListAsset.objectList.pattern))
                        {
                            shouldRegenerate = shouldRegenerate 
                                               && regex.IsMatch(assetPath);
                        }
                    }
                
                    foreach (var assetPath in deletedAssets)
                    {
                        shouldRegenerate = assetPath.StartsWith(normalizedAssetPath);

                        if (!string.IsNullOrEmpty(objectListAsset.objectList.pattern))
                        {
                            shouldRegenerate = shouldRegenerate 
                                               && regex.IsMatch(assetPath);
                        }
                    }

                    if (shouldRegenerate)
                    {
                        objectListAsset.objectList?.Reload();
                        EditorUtility.SetDirty(objectListAsset);
                    }
                
                    AssetDatabase.SaveAssetIfDirty(objectListAsset);

                    // already detected regeneration, stop paths iteration
                    if (shouldRegenerate)
                    {
                        break;
                    }
                }
                
            }
        }
    }
}