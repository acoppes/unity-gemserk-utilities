using UnityEngine;

namespace Gemserk.Utilities
{
    public static class ObjectListExtensions
    {
        public static T FindObjectByNameExt<T>(this IObjectList objectList, string name, 
            bool includeChildren = false) where T : Object
        {
            var obj = objectList.FindByName<Object>(name);

            if (obj is ScriptableObject)
            {
                return obj as T;
            }

            if (obj is GameObject gameObject)
            {
                if (typeof(T) == typeof(GameObject))
                {
                    return obj as T;
                }
                
                if (includeChildren)
                {
                    return gameObject.GetComponentInChildren<T>();
                }
                return gameObject.GetComponent<T>();
            }
            
            return null;
        }
    }
}