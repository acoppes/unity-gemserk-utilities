using System.Runtime.CompilerServices;
using UnityEngine;

namespace Game.Systems
{
    public interface IGamePerspective
    {
        Vector3 ConvertToWorld(Vector3 v);

        Vector3 ConvertFromWorld(Vector3 v);

        Vector2 ProjectFromWorld(Vector3 v);
    }

    public class FixedGamePerspective : IGamePerspective
    {
        private static readonly float GamePerspectiveY = 0.75f;
        
        public Vector3 ConvertToWorld(Vector3 v)
        {
            return new Vector3(v.x, v.z, v.y / GamePerspectiveY);
        }

        public Vector3 ConvertFromWorld(Vector3 v)
        {
            return new Vector3(v.x, v.z * GamePerspectiveY, v.y);
        }

        public Vector2 ProjectFromWorld(Vector3 v)
        {
            return new Vector2(v.x, v.y + v.z * GamePerspectiveY);
        }
    }
    
    public class NullGamePerspective : IGamePerspective
    {
        public Vector3 ConvertToWorld(Vector3 v)
        {
            return v;
        }

        public Vector3 ConvertFromWorld(Vector3 v)
        {
            return v;
        }

        public Vector2 ProjectFromWorld(Vector3 v)
        {
            return new Vector2(v.x, v.y + v.z);
        }
    }
    
    public static class GamePerspective
    {
        public static IGamePerspective gamePerspective = new FixedGamePerspective();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ConvertToWorld(Vector3 v)
        {
            return gamePerspective.ConvertToWorld(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 ConvertFromWorld(Vector3 v)
        {
            return gamePerspective.ConvertFromWorld(v);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 ProjectFromWorld(Vector3 v)
        {
            return gamePerspective.ProjectFromWorld(v);
        }
    }
}