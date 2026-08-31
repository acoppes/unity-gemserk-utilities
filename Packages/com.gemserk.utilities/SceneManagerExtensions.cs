using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Gemserk.Utilities
{
    public static class SceneManagerExtensions
    {
        public static List<T> FindObjectsInAllScenes<T>(bool includeInactive)
        {
            var collected = new List<T>();
            
            var sceneCount = SceneManager.sceneCount;

            for (var i = 0; i < sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                var rootGameObjects = scene.GetRootGameObjects();
                foreach (var root in rootGameObjects)
                {
                    var values = root.GetComponentsInChildren<T>(includeInactive);
                    collected.AddRange(values);
                }
            }

            return collected;
        }
    }
}