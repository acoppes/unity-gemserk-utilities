using UnityEngine;

namespace Gemserk.Utilities
{
    public static class Vector2Extensions
    {
        /// <summary>
        /// Returns v rotated by the specified angle in radians.
        /// </summary>
        /// <param name="v">The Vector2 to be rotated.</param>
        /// <param name="angle">The angle in radians.</param>
        public static Vector2 Rotate(this Vector2 v, float angle)
        {
            var a_cos = Mathf.Cos(angle);
            var a_sin = Mathf.Sin(angle);
            return new Vector2(v.x * a_cos - v.y * a_sin, 
                v.x * a_sin + v.y * a_cos);
        }

        public static Vector3 ToVector3(this Vector2 v)
        {
            return new Vector3(v.x, v.y, 0);
        }
    }
}