using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesTransitionControllerCallbacksSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var controllers = world.GetComponents<ControllerComponent>();
            var stateComponents = world.GetComponents<StatesComponent>();
            
            foreach (var entity in world.GetFilter<StatesComponent>()
                         .Inc<ControllerComponent>()
                         .Exc<DisabledComponent>()
                         .End())
            {
                var statesComponent = stateComponents.Get(entity);
                var controllerComponent = controllers.Get(entity);
                
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