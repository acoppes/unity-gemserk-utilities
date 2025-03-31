using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class HealthTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            SetInvulnerable = 0,
            SetVulnerable = 1,
            SetTotal = 2,
            Fill = 3,
            SetFactor = 4
        }

        public ActionType actionType;
        public TriggerTarget triggerTarget;

        [ConditionalField(nameof(actionType), false, ActionType.SetTotal)]
        public float total;
        
        [ConditionalField(nameof(actionType), false, ActionType.SetFactor)]
        [Range(0, 1)]
        public float factor;
        
        public override string GetObjectName()
        {
            return $"Health{actionType}({triggerTarget})"; 
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            world.GetTriggerTargetEntities(null, triggerTarget, activator, entities);
            
            foreach (var entity in entities)
            {
                ref var health = ref world.GetComponent<HealthComponent>(entity);
                
                // TODO: might want to delegate to a system or similar in order to react better to changes.
                if (actionType == ActionType.SetInvulnerable)
                {
                    health.invulnerableCount++;
                }
                else if (actionType == ActionType.SetVulnerable)
                {
                    health.invulnerableCount--;
                } else if (actionType == ActionType.SetTotal)
                {
                    health.total = total;
                } else if (actionType == ActionType.Fill)
                {
                    health.current = health.total;
                } else if (actionType == ActionType.SetFactor)
                {
                    health.factor = factor;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}