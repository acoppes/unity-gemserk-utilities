using UnityEngine;

namespace Gemserk.Utilities
{
    public static class UnityObjectExtensions
    {
        public static T GetInterface<T>(this UnityEngine.Object obj) where T : class
        {
            if (obj is T t)
                return t;

            if (obj is GameObject go)
            {
                return go.GetComponentInChildren<T>();
            }

            if (obj is MonoBehaviour monoBehaviour)
            {
                return monoBehaviour.GetComponentInChildren<T>();
            }

            return null;
        }
    }
}