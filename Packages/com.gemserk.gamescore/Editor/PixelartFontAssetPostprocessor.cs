using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class PixelartFontAssetPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var importedAsset in importedAssets)
            {
                if (importedAsset.EndsWith("ttf") || importedAsset.EndsWith("otf"))
                {
                    Debug.Log("font!");
                    if (importedAsset.ToLower().Contains("pixelart"))
                    {
                        Debug.Log("pixelart font!");
                        var fontTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(importedAsset);
                        if (fontTexture != null)
                        {
                            fontTexture.filterMode = FilterMode.Point;
                        }
                    }
                

                
                }
            }
        }
    }
}