using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class EcsWorldEntitiesWindowDebugSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<EcsWorldEntitiesDebugComponent>> filter = default;

        // private Type[] typesCache;
        
        public void OnEntityCreated(World world, Entity entity)
        {
#if UNITY_EDITOR
            world.AddComponent(entity, new EcsWorldEntitiesDebugComponent()
            {
                // name = entity.ToString()
                name = string.Empty
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
                // TODO: use world entity listener and on entity changed to recalculate stuff

                var entity = world.GetEntity(e);
                
                ref var debug = ref filter.Pools.Inc1.Get(e);
                debug.componentTypeCount = world.EcsWorld.GetComponentTypes(e, ref debug.componentTypes);

                if (world.HasComponent<NameComponent>(entity))
                {
                    debug.name = world.GetComponent<NameComponent>(entity).name;
                }
                
                // update debug stuff
                // debug.name = $"{}";
            }
#endif
        }
    }
}