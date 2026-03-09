using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class ControllerSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent>, Exc<DisabledComponent>> controllerFilter = default;
        
        public void Run(EcsSystems systems)
        {
            var deltaTime = dt;
            
            foreach (var entity in controllerFilter.Value)
            {
                ref var controllerComponent = ref controllerFilter.Pools.Inc1.Get(entity);
                
                var worldEntity = world.GetEntity(entity);
                
                foreach (var updateable in controllerComponent.updateListeners)
                {
                    updateable.OnUpdate(world, worldEntity, deltaTime);
                }
            }
        }
    }
}