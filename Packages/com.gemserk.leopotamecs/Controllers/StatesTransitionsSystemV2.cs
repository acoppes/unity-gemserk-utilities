using System.Runtime.CompilerServices;
using System.Text;
using Gemserk.Leopotam.Ecs.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Controllers
{
    public class StatesTransitionsSystemV2 : BaseSystem, IEcsRunSystem, IEntityDestroyedHandler
    {
        readonly EcsFilterInject<Inc<StatesComponentV2>, Exc<DisabledComponent>> statesFilter = default;
        readonly EcsPoolInject<StatesComponentV2> stateComponents = default;

        private static readonly StringBuilder _stringBuilder = new StringBuilder();

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
            
#if UNITY_EDITOR
            if (statesComponent.debugTransitions)
            {
                if (statesComponent.statesExitedLastFrame != 0)
                {
                    _stringBuilder.Clear();

                    for (var i = 0; i < statesComponent.states.Length; i++)
                    {
                        if (statesComponent.HasExitLastFrame(i))
                        {
                            var name = string.Empty;
                            
                            if (statesComponent.typesAsset != null)
                            {
                                name = statesComponent.typesAsset.GetTypeName(i);
                            }

                            if (string.IsNullOrEmpty(name))
                            {
                                _stringBuilder.Append(_stringBuilder.Length == 0 ? $"{i}" : $",{i}");
                            }
                            else
                            {
                                _stringBuilder.Append(_stringBuilder.Length == 0 ? $"{name}" : $",{name}");
                            }
                           
                        }
                    }
                    Debug.Log($"EXIT: {_stringBuilder}");
                }
                
                if (statesComponent.statesEnteredLastFrame != 0)
                {
                    _stringBuilder.Clear();
                    
                    for (var i = 0; i < statesComponent.states.Length; i++)
                    {
                        if (statesComponent.HasEnteredInLastFrame(i))
                        {
                            var name = string.Empty;
                            
                            if (statesComponent.typesAsset != null)
                            {
                                name = statesComponent.typesAsset.GetTypeName(i);
                            }

                            if (string.IsNullOrEmpty(name))
                            {
                                _stringBuilder.Append(_stringBuilder.Length == 0 ? $"{i}" : $",{i}");
                            }
                            else
                            {
                                _stringBuilder.Append(_stringBuilder.Length == 0 ? $"{name}" : $",{name}");
                            }
                        }
                    }
                    Debug.Log($"ENTER: {_stringBuilder}");
                }
            }
#endif
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