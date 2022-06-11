using System;
using Leopotam.EcsLite;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public abstract class BaseSystem : MonoBehaviour, IEcsSystem
    {
        [NonSerialized]
        public World world;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameObject.GetComponents<Component>().Length == 2)
            {
                gameObject.name = $"{GetType().Name}";
            }
        }
#endif
    }
}