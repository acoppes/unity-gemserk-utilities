using System.Linq;
using Game.Components;
using Game.Definitions;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public class AnimationAssetPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var animationAssetsList = AssetDatabaseExt.FindAssets<AnimationsAsset>();

            foreach (var animationAsset in animationAssetsList)
            {
                if (string.IsNullOrEmpty(animationAsset.sourceFolder))
                {
                    continue;
                }

                var importedCount = importedAssets.Count(asset => asset.StartsWith(animationAsset.sourceFolder));
                var deletedCount = deletedAssets.Count(asset => asset.StartsWith(animationAsset.sourceFolder));
                var movedCount = movedAssets.Count(asset => asset.StartsWith(animationAsset.sourceFolder));
                
                if (importedCount > 0 || deletedCount > 0 || movedCount > 0)
                {
                    Debug.Log($"{animationAsset.name} sprites changed, regenerating animation.");
                    var sprites = AssetDatabaseSpriteExtensions.GetSpritesFromFolder(animationAsset.sourceFolder);
                    AnimationCreatorTool.ConfigureAnimationsAsset(animationAsset, AnimationDefinition.DefaultFrameRate, sprites);
                }
            }
            
        }
    }
}