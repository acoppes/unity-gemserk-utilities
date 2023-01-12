using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public static class UnityObjectExtensions
    {
        public static T GetInterface<T>(this UnityEngine.Object unityObject) where T : class
        {
            if (unityObject is T t)
                return t;

            if (unityObject is GameObject go)
            {
                return go.GetComponentInChildren<T>();
            }

            if (unityObject is MonoBehaviour monoBehaviour)
            {
                return monoBehaviour.GetComponentInChildren<T>();
            }

            return null;
        }
    }
}