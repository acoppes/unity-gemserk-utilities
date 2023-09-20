using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class TargetSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<TargetComponent>, Exc<DisabledComponent>> filter = default;
        readonly EcsFilterInject<Inc<TargetComponent, PositionComponent>, Exc<DisabledComponent>> positionFilter = default;
        readonly EcsFilterInject<Inc<TargetComponent, PlayerComponent>, Exc<DisabledComponent>> playerFilter = default;
        readonly EcsFilterInject<Inc<TargetComponent, HealthComponent>, Exc<DisabledComponent>> healthFilter = default;
        readonly EcsFilterInject<Inc<TargetComponent, MovementComponent>, Exc<DisabledComponent>> movementFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var targetComponent = ref filter.Pools.Inc1.Get(entity);
                targetComponent.target.targeted = false;
            }

            foreach (var entity in positionFilter.Value)
            {
                ref var targetComponent = ref positionFilter.Pools.Inc1.Get(entity);
                var positionComponent = positionFilter.Pools.Inc2.Get(entity);

                ref var target = ref targetComponent.target;
                target.entity = world.GetEntity(entity);
                target.position = positionComponent.value;
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