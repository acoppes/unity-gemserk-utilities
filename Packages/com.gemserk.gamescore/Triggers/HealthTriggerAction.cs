using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class HealthTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            SetInvulnerable,
            SetVulnerable
        }

        public ActionType actionType;
        public TriggerTarget triggerTarget;
        
        public override string GetObjectName()
        {
            return $"{actionType}({triggerTarget})"; 
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            world.GetTriggerTargetEntities(null, triggerTarget, activator, entities);
            
            foreach (var entity in entities)
            {
                ref var health = ref world.GetComponent<HealthComponent>(entity);
                if (actionType == ActionType.SetInvulnerable)
                {
                    health.invulnerableCount++;
                }
                else if (actionType == ActionType.SetVulnerable)
                {
                    health.invulnerableCount--;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}