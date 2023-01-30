using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace Gemserk.Utilities.Editor
{
    public static class RefactorTools
    {
        [MenuItem("Refactor/Dirtify Current Selection")]
        public static void DirtifySelection()
        {
            EditorUtility.SetDirty(Selection.activeObject);
        }
        
        [MenuItem("Refactor/Reserialize All Assets")]
        public static void ReserializeAssets()
        {
            if (EditorUtility.DisplayDialog("Reserialize", "Force reserialize all assets?", "Ok", "Cancel"))
            {
                AssetDatabase.ForceReserializeAssets();
            }
        }
        
        [MenuItem("Refactor/Reserialize Selected Assets")]
        public static void ReserializeSelectedAssets()
        {
            var selectedObjects = Selection.objects;
            if (selectedObjects.Length == 0)
            {
                return;
            }

            var assetPaths = selectedObjects
                .Where(obj => AssetDatabase.GetAssetPath(obj) != null)
                .Select(obj => AssetDatabase.GetAssetPath(obj));

            var allPaths = new List<string>();

            foreach (var assetPath in assetPaths)
            {
                allPaths.Add(assetPath);
                
                if (Directory.Exists(assetPath))
                {
                    var objectsInside = AssetDatabase.FindAssets("t:Object", new []{assetPath});
                    var paths = objectsInside.Select(obj => AssetDatabase.GUIDToAssetPath(obj));

                    allPaths.AddRange(paths);
                }
            }

            if (EditorUtility.DisplayDialog("Reserialize", "Force reserialize selected assets?", "Ok", "Cancel"))
            {
                AssetDatabase.ForceReserializeAssets(allPaths);
                // allPaths.ForEach(p => Debug.Log(p));
            }
        }
    }
}