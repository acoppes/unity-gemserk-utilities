using System.Runtime.CompilerServices;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using MyGame;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesTransitionsSystemV2 : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<StatesComponentV2>, Exc<DisabledComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponentV2> stateComponents = default;

        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (stateComponents.Value.Has(entity))
            {
                ref var statesComponent = ref stateComponents.Value.Get(entity);
                statesComponent.ClearCallbacks();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateStatesTransitions(ref StatesComponentV2 statesComponent)
        {
            statesComponent.statesEnteredLastFrame = 0;
            statesComponent.statesEnteredLastFrame = statesComponent.statesBitmask & ~statesComponent.previousStatesBitmask;
            
            statesComponent.statesExitedLastFrame = 0;
            statesComponent.statesExitedLastFrame = statesComponent.previousStatesBitmask & ~statesComponent.statesBitmask;

            statesComponent.previousStatesBitmask = statesComponent.statesBitmask;
//             statesComponent.statesEntered.Clear();
//             statesComponent.statesEntered.UnionWith(statesComponent.activeStates);
//             statesComponent.statesEntered.ExceptWith(statesComponent.previousStates);
//             
//             statesComponent.statesExited.Clear();
//             statesComponent.statesExited.UnionWith(statesComponent.previousStates);
//             statesComponent.statesExited.ExceptWith(statesComponent.activeStates);
//             
//             statesComponent.previousStates.Clear();
//             statesComponent.previousStates.UnionWith(statesComponent.activeStates);
//             
// #if UNITY_EDITOR
//             if (statesComponent.debugTransitions)
//             {
//                 if (statesComponent.statesExited.Count > 0)
//                     Debug.Log($"EXIT: {string.Join(",", statesComponent.statesExited)}");
//                 
//                 if (statesComponent.statesEntered.Count > 0)
//                     Debug.Log($"ENTER: {string.Join(",", statesComponent.statesEntered)}");
//             }
// #endif
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeStatesCallbacks(ref StatesComponentV2 statesComponent)
        {
            if (statesComponent.statesExitedLastFrame != 0)
            {
                statesComponent.OnStatesExit();
            }
            
            if (statesComponent.statesEnteredLastFrame != 0)
            {
                statesComponent.OnStatesEnter();
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in statesFilter.Value)
            {
                var statesComponent = stateComponents.Value.Get(entity);
                UpdateStatesTransitions(ref statesComponent);
                InvokeStatesCallbacks(ref statesComponent);
            }
        }
    }
}