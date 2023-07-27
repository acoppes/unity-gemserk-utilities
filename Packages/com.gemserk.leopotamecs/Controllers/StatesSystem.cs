using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<StatesComponent>, Exc<DisabledComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponent> stateComponents = default;

        private static readonly List<State> statesToRemove = new List<State>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateStateTime(StatesComponent statesComponent, float dt)
        {
            var keys = statesComponent.states.Keys;
            foreach (var stateName in keys)
            {
                var state = statesComponent.states[stateName];
                state.time += dt;
                state.updateCount++;

                if (state.duration > 0 && state.time > state.duration)
                {
                    // statesComponent.ExitState(stateName);
                    statesToRemove.Add(state);
                }
            }

            foreach (var state in statesToRemove)
            {
                statesComponent.ExitState(state.name);
            }
            
            statesToRemove.Clear();
        }

        public void Run(EcsSystems systems)
        {
            var deltaTime = Time.deltaTime;
            foreach (var entity in statesFilter.Value)
            {
                var statesComponent = stateComponents.Value.Get(entity);
                UpdateStateTime(statesComponent, deltaTime);
            }
        }
    }
}