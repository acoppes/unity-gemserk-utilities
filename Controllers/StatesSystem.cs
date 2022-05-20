using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesSystem : BaseSystem, IEcsRunSystem, IEcsInitSystem
    {
        readonly EcsFilterInject<Inc<StatesComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponent> stateComponents = default;

        public void Init(EcsSystems systems)
        {
            world.onEntityCreated += OnEntityCreated;
        }

        private void OnEntityCreated(World world, int entity)
        {
            if (stateComponents.Value.Has(entity))
            {
                ref var statesComponent = ref stateComponents.Value.Get(entity);
                statesComponent.states = new Dictionary<string, State>();
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in statesFilter.Value)
            {
                var statesComponent = stateComponents.Value.Get(entity);

                var keys = statesComponent.states.Keys;
                foreach (var stateName in keys)
                {
                    var state = statesComponent.states[stateName];
                    
                    if (state.usesDuration && !state.isCompleted)
                    {
                        state.duration -= Time.deltaTime;
                        state.isCompleted = state.duration < 0;
                    }
                }
            }
        }

    }
}