using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesTransitionControllerCallbacksSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<ControllerComponent, StatesComponent>, Exc<DisabledComponent>> filter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in filter.Value)
            {
                var controllerComponent = filter.Pools.Inc1.Get(entity);
                var statesComponent = filter.Pools.Inc2.Get(entity);
                
                var worldEntity = world.GetEntity(entity);
                
                if (statesComponent.statesExited.Count > 0)
                {
                    foreach (var stateChanged in controllerComponent.stateChangedListeners)
                    {
                        stateChanged.OnExitState(world, worldEntity);
                    }
                }
                
                if (statesComponent.statesEntered.Count > 0)
                {
                    foreach (var stateChanged in controllerComponent.stateChangedListeners)
                    {
                        stateChanged.OnEnterState(world, worldEntity);
                    }
                }
            }
        }
    }
}