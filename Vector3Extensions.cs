using UnityEngine;

namespace Gemserk.Utilities
{
    public static class Vector3Extensions
    {
        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }
    }
}