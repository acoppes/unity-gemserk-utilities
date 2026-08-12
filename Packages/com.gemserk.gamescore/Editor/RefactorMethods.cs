using System.Collections.Generic;
using System.Linq;
using Game.Utilities;
using Gemserk.RefactorTools.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RefactorMethods
    {
        public static List<string> FindAllScenesInFolder(params string[] folders)
        {
            var guidList = new List<string>();
            guidList.AddRange(AssetDatabase.FindAssets("t:scene", folders));
            return guidList.Select(AssetDatabase.GUIDToAssetPath).ToList();
        }
        
        // [MenuItem("Refactor/Pixel Core/Refactor Targetings")]
        // public static void FindCollectableWithMineral()
        // {
        //     var prefabs = AssetDatabaseExt.FindPrefabs<Targeting>(AssetDatabaseExt.FindOptions.ConsiderInactiveChildren);
        //     
        //     RefactorTools.RefactorMonoBehaviour<Targeting>(new RefactorTools.RefactorParameters()
        //     {
        //         prefabs = prefabs,
        //         scenes = FindAllScenesInFolder("Assets")
        //     }, delegate(GameObject gameObject, RefactorTools.RefactorData data)
        //     {
        //         var result = new RefactorTools.RefactorResult
        //         {
        //             completed = false
        //         };
        //
        //         var targetings = 
        //             gameObject.GetComponentsInChildren<Targeting>(true);
        //
        //         foreach (var targeting in targetings)
        //         {
        //             targeting.customFilter = targeting.targetingFilter.customFilter;
        //             targeting.sorter = targeting.targetingFilter.sorter;
        //             targeting.targetTypeMask = targeting.targetingFilter.targetTypeMask;
        //             result.completed = true;
        //         }
        //         
        //         return result;
        //     });
        // }
    }
}