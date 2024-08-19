using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using UnityEngine.InputSystem;

namespace Game.Components
{
    public class PlayerInputComponentDefinition : ComponentDefinitionBase
    {
        public List<InputActionReference> predefinedInputActions;

        public override void Apply(World world, Entity entity)
        {
            Dictionary<string, InputAction> inputActions = null;
            
            if (predefinedInputActions.Count > 0)
            {
                inputActions = new Dictionary<string, InputAction>();
                
                foreach (var inputActionReference in predefinedInputActions)
                {
                    inputActions[inputActionReference.action.name] = inputActionReference.action;
                }    
            }
            
            world.AddComponent(entity, new PlayerInputComponent()
            {
                inputActions = inputActions
            });
        }
    }
}