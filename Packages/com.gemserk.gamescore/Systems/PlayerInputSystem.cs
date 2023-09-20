using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Systems
{
    public class PlayerInputSystem : BaseSystem, IEcsRunSystem
    {
        readonly EcsFilterInject<Inc<PlayerInputComponent, InputComponent>> playerInputFilter = default;

        readonly EcsFilterInject<Inc<PlayerInputComponent, ControllableByComponent>> controllableFilter = default;
        readonly EcsFilterInject<Inc<InputComponent, LookingDirection>> controlLookingDirectionFilter = default;
        
        // readonly EcsPoolInject<ControllerComponent> controllerComponents = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var entity in playerInputFilter.Value)
            {
                // TODO: could add new component for this to avoid iterating multiple times..
                // maybe in an PlayerInputInitializationSystem that runs before other systems and only when
                // no component PlayerInputInitialzed

                ref var playerInputComponent = ref playerInputFilter.Pools.Inc1.Get(entity);
                var controlComponent = playerInputFilter.Pools.Inc2.Get(entity);
                
                // ref var playerInputComponent = ref playerInputComponents.Get(entity);
                // var controlComponent = controlComponents.Get(entity);
                
                if (playerInputComponent.inputActions != null)
                {
                    continue;
                }

                var playerInput = PlayerInput.GetPlayerByIndex(playerInputComponent.playerInput);

                if (playerInput == null)
                {
                    continue;
                }
                
                playerInputComponent.inputActions = new Dictionary<string, InputAction>();
                
                foreach (var buttonName in controlComponent.actions.Keys)
                {
                    var inputAction = playerInput.actions.FindAction(buttonName);
            
                    if (inputAction == null)
                        continue;

                    playerInputComponent.inputActions[buttonName] = inputAction;
                }
            }

            foreach (var entity in controllableFilter.Value)
            {
                ref var playerInputComponent = ref controllableFilter.Pools.Inc1.Get(entity);
                var controllableComponent = controllableFilter.Pools.Inc2.Get(entity);

                playerInputComponent.disabledByControllable = !controllableComponent.IsControllableByPlayer();
            }

            foreach (var entity in playerInputFilter.Value)
            {
                ref var playerInputComponent = ref playerInputFilter.Pools.Inc1.Get(entity);
                ref var inputComponent = ref playerInputFilter.Pools.Inc2.Get(entity);
                
                var playerInput = PlayerInput.GetPlayerByIndex(playerInputComponent.playerInput);
                
                playerInputComponent.isControlled = false;

                if (playerInput == null || playerInputComponent.disabledByControllable)
                {
                    continue;
                }

                playerInputComponent.isControlled = true;
                
                foreach (var buttonName in inputComponent.actions.Keys)
                {
                    if (playerInputComponent.inputActions.TryGetValue(buttonName, out var inputAction))
                    {
                        var button = inputComponent.actions[buttonName];
                        button.type = (int) inputAction.type;
                        
                        if (inputAction.type == InputActionType.Button)
                        {
                            button.UpdatePressed(inputAction.IsPressed());
                        }
                        
                        if (inputAction.type == InputActionType.Value)
                        {
                            button.vector2 = inputAction.ReadValue<Vector2>();
                        }
                    }
                }
            }
            
            foreach (var entity in controlLookingDirectionFilter.Value)
            {
                ref var controlComponent = ref controlLookingDirectionFilter.Pools.Inc1.Get(entity);
                var lookingDirection = controlLookingDirectionFilter.Pools.Inc2.Get(entity);

                if (controlComponent.direction().vector2.x > 0 && lookingDirection.value.x >= 0)
                {
                    controlComponent.forward().name = controlComponent.right().name;
                    controlComponent.backward().name = controlComponent.left().name;
                    controlComponent.forward().UpdatePressed(true);
                    controlComponent.backward().UpdatePressed(false);
                } else if (controlComponent.direction().vector2.x < 0 && lookingDirection.value.x < 0)
                {
                    controlComponent.forward().name = controlComponent.left().name;
                    controlComponent.backward().name = controlComponent.right().name;
                    controlComponent.forward().UpdatePressed(true);
                    controlComponent.backward().UpdatePressed(false);
                } else if (controlComponent.direction().vector2.x < 0 && lookingDirection.value.x >= 0)
                {
                    controlComponent.backward().name = controlComponent.left().name;
                    controlComponent.forward().name = controlComponent.right().name;
                    controlComponent.forward().UpdatePressed(false);
                    controlComponent.backward().UpdatePressed(true);
                } else if (controlComponent.direction().vector2.x > 0 && lookingDirection.value.x < 0)
                {
                    controlComponent.backward().name = controlComponent.right().name;
                    controlComponent.forward().name = controlComponent.left().name;
                    controlComponent.forward().UpdatePressed(false);
                    controlComponent.backward().UpdatePressed(true);
                }
            }
        }
    }
}