﻿using Leopotam.EcsLite;
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
                var entity = world.GetEntity(e);
                
                ref var debug = ref filter.Pools.Inc1.Get(e);
                // debug.componentTypeCount = world.EcsWorld.GetComponentTypes(e, ref debug.componentTypes);

                if (world.HasComponent<NameComponent>(entity))
                {
                    var nameComponent = world.GetComponent<NameComponent>(entity);
                    
                    if (nameComponent.singleton)
                    {
                        debug.name = $"{nameComponent.name} - <UNIQUE>";
                    }
                    else
                    {
                        debug.name = nameComponent.name;
                    }
                }
                
                // update debug stuff
                // debug.name = $"{}";
            }
#endif
        }
    }
}