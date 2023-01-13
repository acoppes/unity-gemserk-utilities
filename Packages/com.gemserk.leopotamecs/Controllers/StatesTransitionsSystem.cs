using System.Runtime.CompilerServices;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesTransitionsSystem : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<StatesComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponent> stateComponents = default;

        public void OnEntityDestroyed(World world, Entity entity)
        {
            if (stateComponents.Value.Has(entity))
            {
                ref var statesComponent = ref stateComponents.Value.Get(entity);
                statesComponent.ClearCallbacks();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdateStatesTransitions(StatesComponent statesComponent)
        {
            statesComponent.statesEntered.Clear();
            statesComponent.statesEntered.UnionWith(statesComponent.activeStates);
            statesComponent.statesEntered.ExceptWith(statesComponent.previousStates);
            
            statesComponent.statesExited.Clear();
            statesComponent.statesExited.UnionWith(statesComponent.previousStates);
            statesComponent.statesExited.ExceptWith(statesComponent.activeStates);
            
            statesComponent.previousStates.Clear();
            statesComponent.previousStates.UnionWith(statesComponent.activeStates);
            
#if UNITY_EDITOR
            if (statesComponent.debugTransitions)
            {
                if (statesComponent.statesExited.Count > 0)
                    Debug.Log($"EXIT: {string.Join(",", statesComponent.statesExited)}");
                
                if (statesComponent.statesEntered.Count > 0)
                    Debug.Log($"ENTER: {string.Join(",", statesComponent.statesEntered)}");
            }
#endif
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeStatesCallbacks(StatesComponent statesComponent)
        {
            if (statesComponent.statesExited.Count > 0)
            {
                statesComponent.OnStatesExit();
            }
            
            if (statesComponent.statesEntered.Count > 0)
            {
                statesComponent.OnStatesEnter();
            }
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in statesFilter.Value)
            {
                var statesComponent = stateComponents.Value.Get(entity);
                UpdateStatesTransitions(statesComponent);
                InvokeStatesCallbacks(statesComponent);
            }
        }
    }
}