using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Signals;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class SpawnerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<SpawnerComponent, PositionComponent, PlayerComponent>, Exc<DisabledComponent>> filter = default;

        public SignalAsset onSpawnPackCompletedSignal;

        public void Run(EcsSystems systems)
        {
            foreach (var e in filter.Value)
            {
                ref var spawner = ref filter.Pools.Inc1.Get(e);
                var position = filter.Pools.Inc2.Get(e);
                var player = filter.Pools.Inc3.Get(e);

                if (spawner.paused)
                {
                    continue;
                }

                spawner.lastFrameSpawnedEntity = Entity.NullEntity;
                spawner.cooldown.Increase(dt);

                if (spawner.currentSpawnPack == Entity.NullEntity)
                {
                    if (spawner.pending.Count == 0)
                    {
                        continue;
                    }

                    if (spawner.cooldown.IsReady)
                    {
                        if (spawner.currentSpawnPack == Entity.NullEntity)
                        {
                            // create spawn pack entity
                            spawner.currentSpawnPack = world.CreateEntity(spawner.spawnPackDefinition);
                            ref var spawnPack = ref spawner.currentSpawnPack.Get<SpawnPackComponent>();

                            spawnPack.spawnPackData = spawner.pending[0];
                            spawnPack.currentSpawnIndex = 0;

                            if (!string.IsNullOrEmpty(spawnPack.spawnPackData.name) && spawner.currentSpawnPack.Has<NameComponent>())
                            {
                                spawner.currentSpawnPack.Get<NameComponent>().name = spawnPack.spawnPackData.name;
                            }

                            if (spawner.currentSpawnPack.Has<PlayerComponent>())
                            {
                                spawner.currentSpawnPack.Get<PlayerComponent>().player = player.player;
                            }
                            
                            spawner.pending.RemoveAt(0);
                        }
                    }
                }

                if (spawner.currentSpawnPack != Entity.NullEntity)
                {
                    if (spawner.cooldown.IsReady)
                    {
                        ref var spawnPack = ref spawner.currentSpawnPack.Get<SpawnPackComponent>();
                        var definition = spawnPack.spawnPackDefinitions[spawnPack.currentSpawnIndex];
                        
                        var spawnedEntity = world.CreateEntity(definition);

                        var randomPosition = UnityEngine.Random.insideUnitCircle * spawner.randomRadius;
                        
                        ref var spawnedEntityPosition = ref world.GetComponent<PositionComponent>(spawnedEntity);
                        
                        if (spawnedEntityPosition.type == 0)
                        {
                            spawnedEntityPosition.value =
                                position.value + new Vector3(randomPosition.x, 0, randomPosition.y);
                        } else if (spawnedEntityPosition.type == 1)
                        {
                            spawnedEntityPosition.value =
                                position.value + new Vector3(randomPosition.x,  randomPosition.y, 0);
                        }
                        
                        if (world.HasComponent<PlayerComponent>(spawnedEntity))
                        {
                            ref var spawnedEntityPlayer = ref world.GetComponent<PlayerComponent>(spawnedEntity);
                            spawnedEntityPlayer.player = player.player;
                        }

                        spawner.cooldown.Reset();
                        spawner.lastFrameSpawnedEntity = spawnedEntity;

                        if (spawner.currentSpawnPack.Has<SelectionComponent>())
                        {
                            spawner.currentSpawnPack.Get<SelectionComponent>().selectedEntities.Add(spawnedEntity);
                        }

                        spawnPack.currentSpawnIndex++;

                        if (spawnPack.spawnPackCompleted)
                        {
                            // signal spawn completed
                            if (onSpawnPackCompletedSignal != null)
                            {
                                onSpawnPackCompletedSignal.Signal(spawner.currentSpawnPack);
                            }
                            spawner.currentSpawnPack = Entity.NullEntity;
                        }
                    }
                }

            }
        }


    }
}