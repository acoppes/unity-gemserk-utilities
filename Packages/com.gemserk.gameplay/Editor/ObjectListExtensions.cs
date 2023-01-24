using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Gameplay.Editor
{
    public static class ObjectListExtensions
    {
        public static void Reload(this ObjectListAsset objectListAsset)
        {
            objectListAsset.assets.Clear();
            
            var paths = AssetDatabase.FindAssets("t:Object", new[]
                {
                    objectListAsset.path
                })
                .Select(AssetDatabase.GUIDToAssetPath);

            if (!string.IsNullOrEmpty(objectListAsset.validExtension))
            {
                paths = paths.Where(p => p.EndsWith(objectListAsset.validExtension));
            }

            objectListAsset.assets = paths
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .ToList();
            EditorUtility.SetDirty(objectListAsset);
        }
    }
}