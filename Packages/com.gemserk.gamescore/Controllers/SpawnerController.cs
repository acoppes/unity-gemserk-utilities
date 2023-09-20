using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Controllers;
using Gemserk.Leopotam.Ecs.Events;
using Gemserk.Utilities;
using UnityEngine;

namespace Game.Controllers
{
    public class SpawnerController : ControllerBase, IUpdate
    {
        public void OnUpdate(World world, Entity entity, float dt)
        {
            ref var spawnerComponent = ref entity.Get<SpawnerComponent>();
            spawnerComponent.lastFrameSpawnedEntity = Entity.NullEntity;
            
            spawnerComponent.cooldown.Increase(dt);
            
            if (spawnerComponent.pending.Count == 0)
            {
                return;
            }

            var spawnPosition = entity.Get<PositionComponent>().value;

            if (spawnerComponent.area != null)
            {
                 var area = spawnerComponent.area.GetInterface<BoxCollider2D>();
                 spawnPosition = new Vector3(UnityEngine.Random.Range(area.bounds.min.x, area.bounds.max.x), 
                    entity.Get<PositionComponent>().value.y, UnityEngine.Random.Range(area.bounds.min.y, area.bounds.max.y));
            }
            

            if (spawnerComponent.cooldown.IsReady)
            {
                var spawnPack = spawnerComponent.pending[0];

                var definitions = spawnPack.definitions;

                foreach (var definition in definitions)
                {
                    var spawnedEntity = world.CreateEntity(definition);
                
                    ref var spawnedEntityPosition = ref world.GetComponent<PositionComponent>(spawnedEntity);
                    spawnedEntityPosition.value = spawnPosition;
                
                    if (world.HasComponent<Physics2dComponent>(spawnedEntity))
                    {
                        var physicsComponent = world.GetComponent<Physics2dComponent>(spawnedEntity);
                        if (physicsComponent.body != null)
                        {
                            physicsComponent.body.position = spawnPosition;
                        }
                    }
            
                    if (world.HasComponent<PlayerComponent>(spawnedEntity))
                    {
                        ref var spawnedEntityPlayer = ref world.GetComponent<PlayerComponent>(spawnedEntity);
                        spawnedEntityPlayer.player = entity.Get<PlayerComponent>().player;
                    }
                
                    // if (randomLookingDirection)
                    // {
                    //     ref var lookingDirectionComponent = ref world.GetComponent<LookingDirection>(spawnedEntity);
                    //     var lookingDirection = Vector2.right.Rotate(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad);
                    //     lookingDirectionComponent.value = new Vector3(lookingDirection.x, 0, lookingDirection.y);
                    // }
                
                    spawnerComponent.pending.RemoveAt(0);
                    spawnerComponent.cooldown.Reset();

                    spawnerComponent.lastFrameSpawnedEntity = spawnedEntity;
                }
            }
        }

       
    }
}