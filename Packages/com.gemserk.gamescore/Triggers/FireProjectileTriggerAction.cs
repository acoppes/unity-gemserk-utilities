using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using UnityEngine;

namespace Game.Triggers
{
    public class FireProjectileTriggerAction : WorldTriggerAction
    {
        public TriggerTarget triggerTarget = new TriggerTarget();

        public Vector3 direction;

        public override string GetObjectName()
        {
            if (triggerTarget == null)
            {
                return $"FireProjectile(null, {direction})";        
            }
            
            return $"FireProjectile({triggerTarget}, {direction})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            triggerTarget.Get(entities, world, activator);
            
            // var entities = world.GetEntities(query.GetEntityQuery());
            
            foreach (var entity in entities)
            {
                // ref var projectile = ref world.GetComponent<ProjectileComponent>(entity);
                // projectile.initialVelocity = entity.Get<LookingDirection>().value;
                
                if (entity.Has<ProjectileComponent>())
                {
                    entity.Add(new ProjectileFireComponent()
                    {
                        direction = direction
                    });
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}