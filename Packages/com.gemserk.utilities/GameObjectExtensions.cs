using System.Collections.Generic;
using UnityEngine;

namespace Gemserk.Utilities
{
    public static class GameObjectExtensions
    {
        public static Transform FindOrCreateFolder(this Transform t, string name)
        {
            var folderTransform = t.Find(name);
            if (folderTransform == null)
            {
                var actionsGameObject = new GameObject(name);
                actionsGameObject.transform.SetParent(t);
            }

            return folderTransform;
        }

        public static void GetComponentsInChildrenDepth1<T>(this GameObject gameObject, bool includeInactive,
            bool excludeSelf,
            List<T> results) where T : class
        {
            GetComponentsInChildren(gameObject, includeInactive, excludeSelf, 1, results);
        }

        public static void GetComponentsInChildren<T>(this GameObject gameObject, bool includeInactive,
            bool excludeSelf, int depth,
            List<T> results) where T : class
        {
            if (!excludeSelf)
            {
                results.AddRange(gameObject.GetComponents<T>());
            }

            if (depth == 0)
            {
                return;
            }

            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (!child.gameObject.activeInHierarchy && !includeInactive)
                {
                    continue;
                }

                child.gameObject.GetComponentsInChildren(includeInactive, false, depth - 1, results);
            }
        }
        
        public static bool IsSafeToModifyName(this GameObject gameObject)
        {
#if !UNITY_EDITOR
                return false;
#else 
            
#if UNITY_2021_1_OR_NEWER
            if ( UnityEditor.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null)
                return false;
#elif UNITY_2019_1_OR_NEWER
            if (UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject) != null)
                return false;
#endif

            if (!gameObject.scene.IsValid())
            {
                return false;
            }

            return true;
            
#endif
        }
    }
}