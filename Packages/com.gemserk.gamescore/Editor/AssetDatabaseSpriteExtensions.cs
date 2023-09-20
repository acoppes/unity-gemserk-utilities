using System.Collections.Generic;
using System.Linq;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Editor
{
    public static class AssetDatabaseSpriteExtensions
    {
        public static List<Sprite> GetSpritesFromFolder(string path)
        {
            var textures = AssetDatabaseExt.FindAssets<Texture2D>(new []
            {
                path
            });

            var sprites = new List<Sprite>();

            foreach (var texture in textures)
            {
                var texturePath = AssetDatabase.GetAssetPath(texture);
                var textureSprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(texturePath)
                    .OfType<Sprite>();
                sprites.AddRange(textureSprites);
            }

            return sprites;
        }
    }
}