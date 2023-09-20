using System;
using Gemserk.Triggers;
using MyBox;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Game.Triggers
{
    public class ControlTriggerEvent : TriggerEvent
    {
        public enum EventType
        {
            AnyKey = 0, 
            InputAction = 1
        }

        public EventType eventType = EventType.AnyKey;

        [ConditionalField(nameof(eventType), false, EventType.InputAction)]
        public InputAction inputAction;
        
        private IDisposable subscription;

        public override string GetObjectName()
        {
            if (eventType == EventType.InputAction)
            {
                return $"Control{eventType}({inputAction})";
            }
            return $"Control{eventType}()";
        }

        private void OnEnable()
        {
            if (eventType == EventType.AnyKey)
            {
                subscription = InputSystem.onAnyButtonPress.Call(OnAnyKeyPressed);
            }

            if (eventType == EventType.InputAction)
            {
                inputAction.performed += OnInputActionPerformed;
                inputAction.Enable();
            }
        }

        private void OnDisable()
        {
            if (subscription != null)
            {
                subscription.Dispose();
                subscription = null;
            }
            
            if (eventType == EventType.InputAction)
            {
                inputAction.performed -= OnInputActionPerformed;
                inputAction.Disable();
            }
        }

        private void OnAnyKeyPressed(InputControl inputControl)
        {
            trigger.QueueExecution();
        }
        
        private void OnInputActionPerformed(InputAction.CallbackContext obj)
        {
            trigger.QueueExecution();
        }
    }
}