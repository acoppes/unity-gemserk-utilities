using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using UnityEngine;

namespace Game.Triggers
{
    public class PerformHealTriggerAction : WorldTriggerAction
    {
        public TriggerTarget triggerTarget;

        public float heal;
        
        public override string GetObjectName()
        {
            return $"Heal({heal}, {triggerTarget})"; 
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(triggerTarget, activator);
            
            foreach (var entity in entities)
            {
                ref var healthComponent = ref world.GetComponent<HealthComponent>(entity);

                var position = Vector3.zero;

                if (entity.Has<PositionComponent>())
                {
                    position = entity.Get<PositionComponent>().value;
                }
                
                healthComponent.healEffects.Add(new HealthChangeData()
                {
                    value = heal,
                    position = position
                });
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}