using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class EcsWorldEntitiesWindowDebugSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<EcsWorldEntitiesDebugComponent>> filter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
#if UNITY_EDITOR
            world.AddComponent(entity, new EcsWorldEntitiesDebugComponent()
            {
                name = entity.ToString()
            });
#endif
        }

        public void OnEntityDestroyed(World world, Entity entity)
        {
#if UNITY_EDITOR
            // clear references and stuff?
#endif
        }

        public void Run(EcsSystems systems)
        {
#if UNITY_EDITOR
            foreach (var e in filter.Value)
            {
                ref var debug = ref filter.Pools.Inc1.Get(e);
                // update debug stuff
                // debug.name = $"{}";
            }
#endif
        }
    }
}