using System.IO;
using UnityEditor;
using UnityEngine;

namespace Gemserk.Utilities.Editor
{
    public static class PrefabUtils
    {
        [MenuItem("Assets/Create/Empty Prefab")]
        public static void CreateEmptyPrefabInCurrentFolder()
        {
            var go = new GameObject("EmptyGameObject");
            try
            {
                var projectWindowPath = ProjectWindowUtils.GetProjectWindowCurrentPath();
                PrefabUtility.SaveAsPrefabAsset(go, Path.Combine(projectWindowPath, $"{go.name}.prefab"));
            } finally {
                Object.DestroyImmediate(go);
            }
        }
    }
}