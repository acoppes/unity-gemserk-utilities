using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public abstract class BaseSystem : MonoBehaviour
    {
        // public World world;

        // protected World.Time time => world.time;
        
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