using System.Runtime.CompilerServices;
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float DistanceSqr(this Vector2 v, Vector2 otherVector)
        {
            return (v - otherVector).SqrMagnitude();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 FixToAngles(this Vector2 direction, int angles)
        {
            var angleInDegrees = Vector2.SignedAngle(Vector2.right, direction);
            var newAngle = Mathf.RoundToInt(angleInDegrees / angles) * angles;
            return Vector2.right.Rotate(newAngle * Mathf.Deg2Rad);
        }
    }
}