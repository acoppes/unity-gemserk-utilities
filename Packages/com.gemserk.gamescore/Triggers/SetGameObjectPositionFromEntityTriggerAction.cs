using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class SetGameObjectPositionFromEntityTriggerAction : WorldTriggerAction
    {
        public GameObject targetGameObject;
        public TriggerTarget positionTarget;

        public override string GetObjectName()
        {
            if (targetGameObject)
                return $"SetPosition({targetGameObject.name}, {positionTarget})";
            return $"SetPosition({positionTarget})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();

            positionTarget.Get(entities, world, activator);
            
            foreach (var entity in entities)
            {
                targetGameObject.transform.position = entity.Get<PositionComponent>().value;
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}