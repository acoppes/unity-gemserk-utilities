using Game.Components;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class HealthBarModelSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsInitSystem
    {
        public GameObject[] healthBarPrefabs;

        private GameObjectPoolMap poolMap;
        
        public void Init(EcsSystems systems)
        {
            poolMap = new GameObjectPoolMap("~HealthBarModels");
        }
        
        public void OnEntityCreated(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            // create model if model component
            var healthBarComponents = world.GetComponents<HealthBarComponent>();
            if (healthBarComponents.Has(entity))
            {
                ref var healthBarComponent = ref healthBarComponents.Get(entity);

                if (healthBarComponent.instance == null)
                {
                    var modelInstance = poolMap.Get(healthBarPrefabs[healthBarComponent.size]);

                    if (!modelInstance.HasComponent<EntityReference>())
                    {
                        modelInstance.AddComponent<EntityReference>();
                    }
                    
                    var entityReference = modelInstance.GetComponent<EntityReference>();
                    entityReference.entity = entity;
                
                    healthBarComponent.instance = modelInstance.GetComponent<HealthBarModel>();
                    healthBarComponent.instance.visible = false;
                }
            }
        }

        public void OnEntityDestroyed(Gemserk.Leopotam.Ecs.World world, Entity entity)
        {
            // destroy model if model component
            var healthBarComponents = world.GetComponents<HealthBarComponent>();
            if (healthBarComponents.Has(entity))
            {
                ref var healthBarComponent = ref healthBarComponents.Get(entity);
                if (healthBarComponent.instance != null)
                {
                    poolMap.Release(healthBarComponent.instance.gameObject);
                }
                healthBarComponent.instance = null;
            }
        }

        public void Run(EcsSystems systems)
        {
            var healthBarComponents = world.GetComponents<HealthBarComponent>();
            var healthComponents = world.GetComponents<HealthComponent>();
            var positionsComponent = world.GetComponents<PositionComponent>();
            var playerComponents = world.GetComponents<PlayerComponent>();
            var targetComponents = world.GetComponents<TargetComponent>();

            var mainPlayer = 0;

            if (world.TryGetSingletonEntity<MainPlayerComponent, PlayerComponent>(out Entity singletonEntity))
            {
                mainPlayer = singletonEntity.Get<PlayerComponent>().player;
            }
            
            foreach (var entity in world.GetFilter<HealthBarComponent>()
                         .Inc<HealthComponent>()
                         .End())
            {
                ref var healthBarComponent = ref healthBarComponents.Get(entity);
                var healthComponent = healthComponents.Get(entity);

                healthBarComponent.factor = healthComponent.current / healthComponent.total;
                healthBarComponent.visible = healthComponent.current < healthComponent.total &&
                                             healthComponent.current > 0;
            }

            foreach (var entity in world.GetFilter<HealthBarComponent>().Inc<PositionComponent>().End())
            {
                ref var healthBarComponent = ref healthBarComponents.Get(entity);
                var positionComponent = positionsComponent.Get(entity);

                healthBarComponent.position = positionComponent.value + healthBarComponent.offset;
            }
            
            foreach (var entity in world.GetFilter<HealthBarComponent>().Inc<PlayerComponent>().End())
            {
                ref var healthBarComponent = ref healthBarComponents.Get(entity);
                var playerComponent = playerComponents.Get(entity);

                healthBarComponent.player = playerComponent.player;
            }
            
            foreach (var entity in world.GetFilter<HealthBarComponent>().Inc<TargetComponent>().End())
            {
                ref var healthBarComponent = ref healthBarComponents.Get(entity);
                var targetComponent = targetComponents.Get(entity);
                healthBarComponent.highlighted = targetComponent.target.targeted;
            }
            
            foreach (var entity in world.GetFilter<HealthBarComponent>().End())
            {
                ref var healthBarComponent = ref healthBarComponents.Get(entity);

                if (healthBarComponent.instance != null)
                {
                    healthBarComponent.instance.position = healthBarComponent.position;
                    healthBarComponent.instance.visible = (healthBarComponent.visible || healthBarComponent.highlighted) && healthBarComponent.factor > 0;
                    healthBarComponent.instance.highlighted = healthBarComponent.highlighted;
                    healthBarComponent.instance.fillAmount = healthBarComponent.factor;
                    healthBarComponent.instance.isMainPlayer = healthBarComponent.player == mainPlayer;
                }
            }
            

        }
    }
}