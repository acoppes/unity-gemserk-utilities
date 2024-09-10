using Gemserk.Triggers;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Game.Triggers
{
    public class SimulateUnityInputTriggerAction : TriggerAction
    {
        public string deviceNameOrLayout = "keyboard";
        public string controlName = "escape";
        public float value = 1f;
        
        public override string GetObjectName()
        {
            return $"SimulateUnityInputTriggerAction({deviceNameOrLayout}, {controlName})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var device = InputSystem.GetDevice(deviceNameOrLayout);
            var control = device[controlName];
            
            using (StateEvent.From(device, out var eventPtr))
            {
                control.WriteValueIntoEvent(value, eventPtr);
                InputSystem.QueueEvent(eventPtr);
            }
            return ITrigger.ExecutionResult.Completed;
        }
    }
}