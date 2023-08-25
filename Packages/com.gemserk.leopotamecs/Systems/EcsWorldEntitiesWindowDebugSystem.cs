using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Systems
{
    public class EcsWorldEntitiesWindowDebugSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        public static int windowOpenCount = 0;
        
        readonly EcsFilterInject<Inc<EcsWorldEntitiesDebugComponent>> filter = default;

        public void OnEntityCreated(World world, Entity entity)
        {
#if UNITY_EDITOR
            world.AddComponent(entity, new EcsWorldEntitiesDebugComponent()
            {
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
            if (windowOpenCount <= 0)
            {
                return;
            }
            
            foreach (var e in filter.Value)
            {
                var entity = world.GetEntity(e);
                
                ref var debug = ref filter.Pools.Inc1.Get(e);
                // debug.componentTypeCount = world.EcsWorld.GetComponentTypes(e, ref debug.componentTypes);

                if (world.HasComponent<NameComponent>(entity))
                {
                    var nameComponent = world.GetComponent<NameComponent>(entity);
                    debug.name = nameComponent.name;
                    debug.isSingletonByName = nameComponent.singleton;
                }
            }
#endif
        }
    }
}