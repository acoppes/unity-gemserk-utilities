using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<StatesComponent>, Exc<DisabledComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponent> stateComponents = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in statesFilter.Value)
            {
                var statesComponent = stateComponents.Value.Get(entity);

                var keys = statesComponent.states.Keys;
                foreach (var stateName in keys)
                {
                    var state = statesComponent.states[stateName];
                    state.time += Time.deltaTime;
                    state.updateCount++;
                }
            }
        }
    }
}