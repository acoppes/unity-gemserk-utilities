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
            
            var paths = AssetDatabase.FindAssets("t:Object", new[]
                {
                    objectList.path
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