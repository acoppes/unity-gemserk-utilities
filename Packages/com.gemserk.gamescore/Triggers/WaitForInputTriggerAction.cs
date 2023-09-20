using Gemserk.Triggers;
using UnityEngine.InputSystem;

namespace Game.Triggers
{
    public class WaitForInputTriggerAction : WorldTriggerAction
    {
        public InputAction action;

        private void OnEnable()
        {
            action.Enable();
        }

        private void OnDestroy()
        {
            action.Disable();
        }

        public override string GetObjectName()
        {
            return $"WaitForAction({action})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            if (action.WasReleasedThisFrame())
            {
                return ITrigger.ExecutionResult.Completed;
            }
            
            return ITrigger.ExecutionResult.Running;
        }
    }
}