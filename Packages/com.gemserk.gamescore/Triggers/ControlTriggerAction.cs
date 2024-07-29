using System;
using System.Collections.Generic;
using Game.Components;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Triggers
{
    public class ControlTriggerAction : WorldTriggerAction
    {
        [Serializable]
        public class CustomAction
        {
            public string name;
            public InputActionType type = InputActionType.Button;
            
            [ConditionalField(nameof(type), false, InputActionType.Button)]
            public bool button;
            
            [ConditionalField(nameof(type), false, InputActionType.Value)]
            public Vector2 value;
        } 
        
        [DisplayInspector]
        public Query query;

        public Vector2 direction;
        
        public bool left;
        public bool right;
        
        public bool button1;
        public bool button2;
        public bool button3;

        public List<CustomAction> customActions = new List<CustomAction>();

        public override string GetObjectName()
        {
            return $"OverrideControls({query})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(query.GetEntityQuery());
            
            foreach (var entity in entities)
            {
                ref var control = ref world.GetComponent<InputComponent>(entity);
                control.direction().vector2 = direction;
                
                if (control.IsActionDefined(InputComponent.InputAction.DefaultLeft))
                {
                    control.left().UpdatePressed(left);
                }

                if (control.IsActionDefined(InputComponent.InputAction.DefaultRight))
                {
                    control.right().UpdatePressed(right);
                }
                if (control.IsActionDefined(nameof(button1)))
                {
                    control.button1().UpdatePressed(button1);
                }
                
                if (control.IsActionDefined(nameof(button2)))
                {
                    control.button2().UpdatePressed(button2);
                }

                if (control.IsActionDefined(nameof(button3)))
                {
                    control.actions[nameof(button3)].UpdatePressed(button3);
                    // control.button3.UpdatePressed(button3);
                }

                foreach (var customAction in customActions)
                {
                    if (customAction.type == InputActionType.Button)
                    {
                        control.actions[customAction.name].UpdatePressed(customAction.button);
                    } else if (customAction.type == InputActionType.Value)
                    {
                        control.actions[customAction.name].vector2 = customAction.value;
                    }
                }
               
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}