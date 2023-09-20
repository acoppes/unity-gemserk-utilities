using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Game.Systems
{
    public class ModelInterpolationUpdatePositionSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler
    {
        // this one runs in the fixed update to store previous position.
        
        readonly EcsFilterInject<Inc<ModelInterpolationComponent, PositionComponent>, Exc<DisabledComponent>> filter = default;
        
        public void OnEntityCreated(World world, Entity entity)
        {
            if (world.HasComponent<ModelInterpolationComponent>(entity) && world.HasComponent<PositionComponent>(entity))
            {
                ref var interpolationComponent = ref world.GetComponent<ModelInterpolationComponent>(entity);
                var positionComponent = world.GetComponent<PositionComponent>(entity);
                
                interpolationComponent.previousPosition = positionComponent.value;
                interpolationComponent.currentPosition = positionComponent.value;
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                ref var interpolationComponent = ref filter.Pools.Inc1.Get(entity);
                var positionComponent = filter.Pools.Inc2.Get(entity);

                if (!interpolationComponent.disabled)
                {
                    interpolationComponent.previousPosition = interpolationComponent.currentPosition;
                    interpolationComponent.currentPosition = positionComponent.value;
                    interpolationComponent.t = 0;
                    interpolationComponent.time = 0;
                }
                else
                {
                    interpolationComponent.previousPosition = positionComponent.value;
                    interpolationComponent.currentPosition = positionComponent.value;
                    interpolationComponent.t = 1;
                    interpolationComponent.time = 0;
                }
            }
        }


    }
}