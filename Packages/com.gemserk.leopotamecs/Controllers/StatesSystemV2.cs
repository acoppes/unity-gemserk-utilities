using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyGame;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesSystemV2 : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<StatesComponentV2>, Exc<DisabledComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponentV2> stateComponents = default;

        private static readonly List<State> statesToRemove = new List<State>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateStateTime(ref StatesComponentV2 statesComponent, float dt)
        {
            for (var i = 0; i < statesComponent.states.Length; i++)
            {
                if (statesComponent.HasState(i))
                {
                    var state = statesComponent.states[i];
                    state.time += dt;
                    statesComponent.states[i] = state;

                    if (state.duration > 0 && state.time > state.duration)
                    {
                        statesComponent.Exit(i);
                    }
                }
            }
            // var keys = statesComponent.states.Keys;
            // foreach (var stateName in keys)
            // {
            //     var state = statesComponent.states[stateName];
            //     state.time += dt;
            //     state.updateCount++;
            //
            //     if (state.duration > 0 && state.time > state.duration)
            //     {
            //         // statesComponent.ExitState(stateName);
            //         statesToRemove.Add(state);
            //     }
            // }
            //
            // foreach (var state in statesToRemove)
            // {
            //     statesComponent.ExitState(state.name);
            // }
            //
            // statesToRemove.Clear();
        }

        public void Run(EcsSystems systems)
        {
            // var deltaTime = Time.deltaTime;
            // foreach (var entity in statesFilter.Value)
            // {
            //     var statesComponent = stateComponents.Value.Get(entity);
            //     UpdateStateTime(statesComponent, deltaTime);
            // }
        }
    }
}