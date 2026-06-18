using UnityEngine;

namespace Gemserk.Utilities
{
    public static class Vector3Extensions
    {
        public static Vector2 XZ(this Vector3 v)
        {
            return new Vector2(v.x, v.z);
        }

        public static Vector3 RoundToDpi(this Vector3 v, float dpi = 1f)
        {
            return new Vector3(Mathf.RoundToInt(v.x * dpi) / dpi, Mathf.RoundToInt(v.y * dpi) / dpi, Mathf.RoundToInt(v.z * dpi) / dpi);
        }
    }
}