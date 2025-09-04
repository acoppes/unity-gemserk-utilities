using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Triggers
{
    public class SetPositionTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        public TriggerTarget positionTarget;

        public override string GetObjectName()
        {
            return $"SetPosition({target},{positionTarget})";
        }
        
        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();

            var positionEntity = world.GetTriggerFirstEntity(null, positionTarget, activator);
            Assert.IsTrue(positionEntity.Exists());
            
            target.Get(entities, world, activator);
            
            var position = positionEntity.Get<PositionComponent>().value;
                
            foreach (var entity in entities)
            {
                entity.Get<PositionComponent>().value = position;

                if (entity.Has<GameObjectComponent>())
                {
                    entity.Get<GameObjectComponent>().transform.position =
                        position;
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}