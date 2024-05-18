using UnityEngine;

namespace Gemserk.Triggers
{
    public class GameObjectActivationTriggerAction : TriggerAction
    {
        public enum ActionType
        {
            Activate = 0,
            Deactivate = 1, 
            Toggle = 2
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
            if (actionType == ActionType.Activate)
            {
                target.SetActive(true);
            } else if (actionType == ActionType.Deactivate)
            {
                target.SetActive(false);
            } else if (actionType == ActionType.Toggle)
            {
                target.SetActive(!target.activeSelf);
            }
            return ITrigger.ExecutionResult.Completed;
        }
    }
}