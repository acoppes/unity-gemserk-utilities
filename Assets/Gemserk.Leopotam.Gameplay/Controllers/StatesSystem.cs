using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Gameplay.Events;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Gameplay.Controllers
{
    public class StatesSystem : BaseSystem, IEcsRunSystem, IEntityCreatedHandler, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<StatesComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponent> stateComponents = default;

        public void OnEntityCreated(World world, Entity entity)
        {
            if (stateComponents.Value.Has(entity))
            {
                ref var statesComponent = ref stateComponents.Value.Get(entity);
                statesComponent.states = new Dictionary<string, State>();
            }
        }
        
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
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeStatesCallbacks(StatesComponent statesComponent)
        {
            if (statesComponent.statesEntered.Count > 0)
            {
                statesComponent.OnStatesEnter();
            }
            
            if (statesComponent.statesExited.Count > 0)
            {
                statesComponent.OnStatesExit();
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
                    state.time += Time.deltaTime;
                }
                
                UpdateStatesTransitions(statesComponent);
                InvokeStatesCallbacks(statesComponent);
            }

            var controllers = world.GetComponents<ControllerComponent>();
            
            foreach (var entity in world.GetFilter<StatesComponent>().Inc<ControllerComponent>().End())
            {
                var statesComponent = stateComponents.Value.Get(entity);
                var controllerComponent = controllers.Get(entity);

                foreach (var controller in controllerComponent.controllers)
                {
                    if (controller is IStateChanged onStateChanged)
                    {
                        if (statesComponent.statesExited.Count > 0)
                        {
                            onStateChanged.OnExitState();
                        }

                        if (statesComponent.statesEntered.Count > 0)
                        {
                            onStateChanged.OnEnterState();
                        }
                    }
                }
            }
        }


    }
}