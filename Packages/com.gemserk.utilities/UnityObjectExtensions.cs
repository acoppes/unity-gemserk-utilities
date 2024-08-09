using UnityEngine;

namespace Gemserk.Utilities
{
    public static class UnityObjectExtensions
    {
        public static T GetInterface<T>(this UnityEngine.Object obj) where T : class
        {
            if (!obj)
                return null;
            
            if (obj is T t)
            {
                return t;
            }
            
            if (obj is GameObject go)
            {
                return go.GetComponentInChildren<T>();
            }

            if (obj is Component component)
            {
                return component.GetComponentInChildren<T>();
            }

            return null;
        }
        
        public static GameObject GetGameObjectFromInterface(object something)
        {
            if (something is Component t)
            {
                return t.gameObject;
            }

            return null;
        }
    }
}