using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Gemserk.Utilities.Editor
{
    public static class ObjectListExtensions
    {
        public static void Reload(this ObjectList objectList)
        {
            objectList.assets.Clear();

            var typeFilters = objectList.typeFilters;

            var filter = string.Join(" ", typeFilters.Select(typeFilter => $"t:{typeFilter}").ToArray());

            // var filter = objectList.assetDatabaseFilter;
            
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

            foreach (var assetsPath in paths)
            {
             
                // filter assets by type?
                
                if (typeFilters.Count > 0)
                {
                    var assets = AssetDatabase.LoadAllAssetsAtPath(assetsPath);
                    objectList.assets.AddRange(assets.Where(a => typeFilters.Contains(a.GetType().Name,
                        StringComparer.OrdinalIgnoreCase)).ToList());
                }
                else
                {
                    var asset = AssetDatabase.LoadAssetAtPath<Object>(assetsPath);
                    objectList.assets.Add(asset);
                }
            }
        }
    }
}