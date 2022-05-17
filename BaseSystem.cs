using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public abstract class BaseSystem : MonoBehaviour
    {
        [NonSerialized]
        public World world;

        // protected World.Time time => world.time;

        protected new EcsPool<T> GetComponents<T>() where T : struct
        {
            return world.world.GetPool<T>();
        }

        protected EcsWorld.Mask GetFilter<T>() where T : struct
        {
            return world.world.Filter<T>();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameObject.GetComponents<Component>().Length == 2)
            {
                gameObject.name = GetType().Name;
            }
        }
#endif
    }
}