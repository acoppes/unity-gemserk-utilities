using System;
using UnityEngine;

namespace Game
{
    public static class GameObjectExtensions
    {
        public static void SetActiveInParent(this GameObject gameObject)
        {
            var parent = gameObject.transform.parent;
            for (var i = 0; i < parent.childCount; i++)
            {
                parent.GetChild(i).gameObject.SetActive(false);
            }
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Lookup for an object in hierarchy named name and with the component. 
        /// </summary>
        public static T GetComponentWithName<T>(this GameObject gameObject, string name, bool includeInactive = false) where T: Component
        {
            var components = gameObject.GetComponentsInChildren<T>(includeInactive);
            foreach (var component in components)
            {
                if (component.gameObject.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return component;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Lookup for an object in hierarchy named name and with the component. 
        /// </summary>
        public static GameObject FirstChildWithComponent<T>(this GameObject gameObject, string name) where T: Component
        {
            var components = gameObject.GetComponentsInChildren<T>();
            foreach (var component in components)
            {
                if (component.gameObject.name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    return component.gameObject;
                }
            }
            return null;
        }
    }
}