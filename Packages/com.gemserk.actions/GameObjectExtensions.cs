using UnityEngine;

namespace Gemserk.Actions
{
    public static class GameObjectExtensions
    {
        public static Transform FindOrCreateFolder(this Transform t, string name)
        {
            var folderTransform= t.Find(name);
            if (folderTransform == null)
            {
                var actionsGameObject = new GameObject(name);
                actionsGameObject.transform.SetParent(t);
            }
            return folderTransform;
        } 
    }
}