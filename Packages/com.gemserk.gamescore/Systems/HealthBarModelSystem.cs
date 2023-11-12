using Game.Components;
using Game.Models;
using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities.Pooling;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyBox;
using UnityEngine;

namespace Game.Systems
{
    public class HealthBarModelSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<HealthBarComponent, HealthComponent>, Exc<DisabledComponent>> filter = default;
        
        readonly EcsFilterInject<Inc<HealthBarComponent, PositionComponent>, Exc<DisabledComponent>> positionsFilter = default;
        readonly EcsFilterInject<Inc<HealthBarComponent, PlayerComponent>, Exc<DisabledComponent>> playersFilter = default;
        readonly EcsFilterInject<Inc<HealthBarComponent, TargetComponent>, Exc<DisabledComponent>> targetsFilter = default;
        
        readonly EcsFilterInject<Inc<HealthBarComponent>, Exc<DisabledComponent>> healthbarFilter = default;
        
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
            var mainPlayer = 0;

            if (world.TryGetSingletonEntity<MainPlayerComponent, PlayerComponent>(out Entity singletonEntity))
            {
                mainPlayer = singletonEntity.Get<PlayerComponent>().player;
            }
            
            foreach (var entity in filter.Value)
            {
                ref var healthBarComponent = ref filter.Pools.Inc1.Get(entity);
                var healthComponent = filter.Pools.Inc2.Get(entity);

                healthBarComponent.factor = healthComponent.current / healthComponent.total;
                healthBarComponent.visible = healthComponent.current < healthComponent.total &&
                                             healthComponent.current > 0;
            }

            foreach (var entity in positionsFilter.Value)
            {
                ref var healthBarComponent = ref positionsFilter.Pools.Inc1.Get(entity);
                var positionComponent = positionsFilter.Pools.Inc2.Get(entity);

                healthBarComponent.position = positionComponent.value + healthBarComponent.offset;
            }
            
            foreach (var entity in playersFilter.Value)
            {
                ref var healthBarComponent = ref playersFilter.Pools.Inc1.Get(entity);
                var playerComponent = playersFilter.Pools.Inc2.Get(entity);

                healthBarComponent.player = playerComponent.player;
            }
            
            foreach (var entity in targetsFilter.Value)
            {
                ref var healthBarComponent = ref targetsFilter.Pools.Inc1.Get(entity);
                var targetComponent = targetsFilter.Pools.Inc2.Get(entity);
                healthBarComponent.highlighted = targetComponent.target.targeted;
            }
            
            foreach (var entity in healthbarFilter.Value)
            {
                ref var healthBarComponent = ref healthbarFilter.Pools.Inc1.Get(entity);

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