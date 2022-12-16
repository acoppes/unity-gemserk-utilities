using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public abstract class ControllerBase : MonoBehaviour, IController
    {
        protected World world;
        protected Entity entity;

        public void Bind(World world, Entity entity)
        {
            this.world = world;
            this.entity = entity;
        }
        
        public ref T Get<T>() where T : struct
        {
            return ref world.GetComponent<T>(entity);
        }

        public bool Has<T>() where T : struct
        {
            return world.HasComponent<T>(entity);
        }
    }
}