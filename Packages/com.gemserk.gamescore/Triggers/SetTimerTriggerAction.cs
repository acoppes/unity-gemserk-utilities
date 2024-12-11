using System;
using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;

namespace Game.Triggers
{
    public class SetTimerTriggerAction : WorldTriggerAction
    {
        [Obsolete("Use TriggerTarget instead")]
        public Query query;

        public TriggerTarget target;
        
        public float time;
        
        public override string GetObjectName()
        {
            if (query != null)
            {
                return $"SetTimer({time}, {query})";
            }

            return $"SetTimer({time}, {target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            world.GetTriggerTargetEntities(query, target, activator, entities);
            
            foreach (var entity in entities)
            {
                ref var timerComponent = ref world.GetComponent<TimerComponent>(entity);
                timerComponent.timer = new Cooldown(time);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}