﻿using System.Collections.Generic;
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
        
        public static T GetComponentInChildrenDepth1<T>(this GameObject gameObject, bool includeInactive,
            bool excludeSelf) where T : class
        {
            return GetComponentInChildren<T>(gameObject, includeInactive, excludeSelf, 1);
        }
        
        public static T GetComponentInChildren<T>(this GameObject gameObject, bool includeInactive,
            bool excludeSelf, int depth) where T : class
        {
            if (!excludeSelf)
            {
                var t = gameObject.GetComponent<T>();
                if (t != null)
                {
                    return t;
                }
            }

            if (depth == 0)
            {
                return null;
            }

            for (var i = 0; i < gameObject.transform.childCount; i++)
            {
                var child = gameObject.transform.GetChild(i);
                if (!child.gameObject.activeInHierarchy && !includeInactive)
                {
                    continue;
                }

                var t = child.gameObject.GetComponentInChildren<T>(includeInactive, false, depth - 1);
                if (t != null)
                {
                    return t;
                }
            }

            return null;
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

        public static T GetInstanceFromRoot<T>(this Transform transform) where T : Component
        {
            return transform.root.GetComponentInChildren<T>();
        }
        
        public static T GetInstanceFromRoot<T>(this GameObject gameObject) where T : Component
        {
            var rootObjects = gameObject.scene.GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                
            }
            return gameObject.transform.GetInstanceFromRoot<T>();
        }
    }
}