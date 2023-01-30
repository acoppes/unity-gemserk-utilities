using Gemserk.Triggers;
using UnityEngine;

namespace Gemserk.Actions
{
    public class GameObjectActivationTriggerAction : TriggerAction
    {
        public enum ActionType
        {
            Activate = 0,
            Deactivate = 1
        }
        
        public GameObject target;
        public ActionType actionType;

        public override string GetObjectName()
        {
            if (target == null)
            {
                return $"{actionType}()";
            }
            
            return $"{actionType}({target.name})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            target.SetActive(actionType == ActionType.Activate);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}