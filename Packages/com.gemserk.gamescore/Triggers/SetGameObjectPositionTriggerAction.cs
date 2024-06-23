using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class SetGameObjectPositionTriggerAction : TriggerAction
    {
        public string gameObjectName;
        public Transform targetPosition;

        public override string GetObjectName()
        {
            if (targetPosition)
                return $"SetPosition({gameObjectName}, {targetPosition.position})";
            return $"SetPosition({gameObjectName})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var targetGameObject = GameObject.Find(gameObjectName);
            targetGameObject.transform.SetPositionAndRotation(targetPosition.position, targetPosition.rotation);
            return ITrigger.ExecutionResult.Completed;
        }
    }
}