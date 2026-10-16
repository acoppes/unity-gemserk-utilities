using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Game.Systems
{
    public class TargetSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<TargetComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<TargetComponent, PositionComponent>, Exc<DisabledComponent>> positionFilter = default;
        readonly EcsFilterInject<Inc<TargetComponent, PlayerComponent>, Exc<DisabledComponent>> playerFilter = default;
        readonly EcsFilterInject<Inc<TargetComponent, HealthComponent>, Exc<DisabledComponent>> healthFilter = default;
        readonly EcsFilterInject<Inc<TargetComponent, MovementComponent>, Exc<DisabledComponent>> movementFilter = default;
        
        readonly EcsFilterInject<Inc<TargetComponent, DisabledComponent>> disabledEntities = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in disabledEntities.Value)
            {
                ref var target = ref disabledEntities.Pools.Inc1.Get(entity);
                target.target.Clear();
            }
            
            foreach (var entity in filter.Value)
            {
                ref var target = ref filter.Pools.Inc1.Get(entity);
                target.target.targeted = false;
                target.target.entity = world.GetEntity(entity);
            }

            foreach (var entity in positionFilter.Value)
            {
                ref var target = ref positionFilter.Pools.Inc1.Get(entity);
                var position = positionFilter.Pools.Inc2.Get(entity);
                target.target.position = position.value;
            }
            
            foreach (var entity in playerFilter.Value)
            {
                ref var targetComponent = ref playerFilter.Pools.Inc1.Get(entity);
                var playerComponent = playerFilter.Pools.Inc2.Get(entity);

                targetComponent.target.player = playerComponent.player;
            }

            // var nameComponents = world.GetComponents<NameComponent>();
            // foreach (var entity in world.GetFilter<TargetComponent>().Inc<NameComponent>().End())
            // {
            //     ref var targetComponent = ref targetComponents.Get(entity);
            //     var nameComponent = nameComponents.Get(entity);
            //     targetComponent.target.name = nameComponent.name;
            // }
            
            foreach (var entity in healthFilter.Value)
            {
                ref var targetComponent = ref healthFilter.Pools.Inc1.Get(entity);
                var healthComponent = healthFilter.Pools.Inc2.Get(entity);

                targetComponent.target.aliveType = healthComponent.aliveType;
                targetComponent.target.healthFactor = healthComponent.factor;
            }

            foreach (var entity in movementFilter.Value)
            {
                ref var targetComponent = ref movementFilter.Pools.Inc1.Get(entity);
                var movementComponent = movementFilter.Pools.Inc2.Get(entity);

                targetComponent.target.velocity = movementComponent.currentVelocity;
            }
        }
    }
}