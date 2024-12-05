using System;
using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class PerformDamageTriggerAction : WorldTriggerAction
    {
        [Obsolete("Use TriggerTarget instead")]
        public Query query;
        
        [ConditionalField(nameof(query), true)]
        public TriggerTarget triggerTarget;

        public float damage;
        
        public override string GetObjectName()
        {
            if (query != null)
            {
                return $"Damage({damage}, {query})";
            }
            
            return $"Damage({damage}, {triggerTarget})"; 
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            world.GetTriggerTargetEntities(query, triggerTarget, activator, entities);
            
            foreach (var entity in entities)
            {
                ref var healthComponent = ref world.GetComponent<HealthComponent>(entity);

                var position = Vector3.zero;

                if (entity.Has<PositionComponent>())
                {
                    position = entity.Get<PositionComponent>().value;
                }
                
                healthComponent.damages.Add(new DamageData()
                {
                    value = damage,
                    position = position,
                    knockback = false,
                    source = Entity.NullEntity,
                    vfxDefinition = null
                });
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}