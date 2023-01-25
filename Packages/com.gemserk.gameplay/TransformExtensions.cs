using UnityEngine;

namespace Gemserk.Gameplay
{
    public static class TransformExtensions
    {
        public static Transform FindInHierarchy(this Transform t, string name)
        {
            if (t.gameObject.name.Equals(name))
                return t;

            for (var i = 0; i < t.childCount; i++) {
                var c = t.GetChild(i);
                if (c == null)
                    continue;
                var result = c.FindInHierarchy(name);

                if (result != null)
                    return result;
            }

            return null;
        }
        
        public static void DestroyAllChildren(this Transform t)
        {
            while (t.childCount > 0)
            {
                var c = t.GetChild(0);
                Object.DestroyImmediate(c.gameObject);
            }
        }
    }
}