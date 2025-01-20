using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class LookingDirectionTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Fixed = 0,
            Random = 1,
            Target = 2
        }
        
        public TriggerTarget triggerTarget;

        public ActionType actionType;

        [ConditionalField(nameof(actionType), false, ActionType.Fixed)]
        public Vector3 direction;
        
        [ConditionalField(nameof(actionType), false, ActionType.Target)]
        public TriggerTarget lookAtTarget;
        
        public override string GetObjectName()
        {
            return $"SetLookingDirection({triggerTarget})"; 
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();

            if (triggerTarget.Get(entities, world, activator))
            {
                foreach (var entity in entities)
                {
                    ref var lookingDirection = ref world.GetComponent<LookingDirection>(entity);
                    
                    if (actionType == ActionType.Fixed)
                    {
                        lookingDirection.value = direction;
                    }
                    else if (actionType == ActionType.Random)
                    {
                        lookingDirection.value = RandomExtensions.RandomVector2(1, 1, 0, 360);
                    } else if (actionType == ActionType.Target)
                    {
                        var lookAtTargetEntity = world.GetTriggerFirstEntity(null, lookAtTarget, null);
                        lookingDirection.value = lookAtTargetEntity.Get<PositionComponent>().value 
                                                 - entity.Get<PositionComponent>().value;
                    }
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}