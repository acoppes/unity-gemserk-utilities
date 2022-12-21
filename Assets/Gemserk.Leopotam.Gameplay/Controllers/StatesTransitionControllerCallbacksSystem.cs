using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public class StatesTransitionControllerCallbacksSystem : BaseSystem, IEcsRunSystem
    {
        public void Run(EcsSystems systems)
        {
            var controllers = world.GetComponents<ControllerComponent>();
            var stateComponents = world.GetComponents<StatesComponent>();
            
            foreach (var entity in world.GetFilter<StatesComponent>().Inc<ControllerComponent>().End())
            {
                var statesComponent = stateComponents.Get(entity);
                var controllerComponent = controllers.Get(entity);

                foreach (var stateChanged in controllerComponent.stateChangedListeners)
                {
                    if (statesComponent.statesExited.Count > 0)
                    {
                        stateChanged.OnExitState();
                    }

                    if (statesComponent.statesEntered.Count > 0)
                    {
                        stateChanged.OnEnterState();
                    }
                }
            }
        }


    }
}