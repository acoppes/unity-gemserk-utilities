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

            if (!string.IsNullOrEmpty(objectListAsset.pattern))
            {
                paths = paths.Where(p => objectListAsset.regex.IsMatch(p));
            }

            objectListAsset.assets = paths
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .ToList();
            EditorUtility.SetDirty(objectListAsset);
        }
    }
}