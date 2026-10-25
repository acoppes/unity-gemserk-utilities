using UnityEngine;

namespace Gemserk.Utilities
{
    public static class ObjectListExtensions
    {
        public static T FindComponentByName<T>(this IObjectList objectList, string name, 
            bool includeChildren = false) where T : Object
        {
            var obj = objectList.FindByName<Object>(name);

            if (obj is GameObject gameObject)
            {
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