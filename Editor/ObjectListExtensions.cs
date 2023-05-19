using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    public static class ObjectListExtensions
    {
        public static void Reload(this ObjectList objectList)
        {
            objectList.assets.Clear();

            var filter = objectList.assetDatabaseFilter;
            if (string.IsNullOrEmpty(filter))
            {
                filter = "t:Object";
            }
            
            var paths = AssetDatabase.FindAssets(filter, new[]
                {
                    objectList.normalizedAssetPath
                })
                .Select(AssetDatabase.GUIDToAssetPath);

            if (!string.IsNullOrEmpty(objectList.pattern))
            {
                paths = paths.Where(p => objectList.regex.IsMatch(p));
            }

            objectList.assets = paths
                .Select(AssetDatabase.LoadAssetAtPath<Object>)
                .ToList();
        }
    }
}