using System;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class PixelartFontAssetPostprocessor : AssetPostprocessor
    {
        private const string PixelArtSuffix = "-pixelart";
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
        {
            if (didDomainReload)
                return;
            
            foreach (var importedAssetPath in importedAssets)
            {
                if (!importedAssetPath.Contains(PixelArtSuffix, StringComparison.OrdinalIgnoreCase)) 
                    continue;
                
                var importer = AssetImporter.GetAtPath(importedAssetPath);
                if (importer is TrueTypeFontImporter)
                {
                    Debug.Log($"PIXELART FONT DETECTED: {importedAssetPath}");
                    var allAssets = AssetDatabase.LoadAllAssetsAtPath(importedAssetPath);
                    foreach (var asset in allAssets)
                    {
                        if (asset is Texture2D texture)
                        {
                            texture.filterMode = FilterMode.Point;
                            EditorUtility.SetDirty(texture);
                            AssetDatabase.SaveAssetIfDirty(asset);
                        }
                    }
                }
            }
        }
    }
}