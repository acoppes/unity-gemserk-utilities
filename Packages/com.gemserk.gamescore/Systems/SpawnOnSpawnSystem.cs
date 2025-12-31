using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class SpawnOnSpawnSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<SpawnOnSpawnComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                var spawnOnSpawn = filter.Pools.Inc1.Get(e);
                var position = filter.Pools.Inc2.Get(e);

                var entity = world.GetEntity(e);

                foreach (var spawnDefinition in spawnOnSpawn.definitions)
                {
                    var spawnedEntity = world.CreateEntity(spawnDefinition, new IEntityInstanceParameter[]
                    {
                        new SourceEntityParameter(entity)
                    });
                    
                    spawnedEntity.Get<PositionComponent>().value = position.value;
                    
                    // if (entity.Has<PlayerComponent>() && spawnedEntity.Has<PlayerComponent>())
                    // {
                    //     spawnedEntity.Get<PlayerComponent>().player = entity.Get<PlayerComponent>().player;
                    // }
                }
               
                filter.Pools.Inc1.Del(e);
                // world.RemoveComponent<SpawnOnSpawnComponent>(e);
            }
        }
    }
}