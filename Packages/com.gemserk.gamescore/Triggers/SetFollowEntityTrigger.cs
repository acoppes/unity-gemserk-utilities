using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;
using MyBox;
using UnityEngine;

namespace Game.Triggers
{
    public class SetFollowEntityTrigger : WorldTriggerAction
    {
        [DisplayInspector]
        public Query query;
        
        [DisplayInspector]
        public Query targetQuery;

        public Vector3 offset;

        public override string GetObjectName()
        {
            if (query != null)
            {
                if (targetQuery != null)
                {
                    return $"SetFollowEntity({query}, {targetQuery})";
                }
                
                return $"SetFollowEntity({query}, null)";
            }    
            return "SetFollowEntity()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(query.GetEntityQuery());

            var target = Entity.NullEntity;
            
            if (targetQuery != null)
            {
                target = world.GetFirstOrDefault(targetQuery);
            }
            
            foreach (var entity in entities)
            {
                ref var follow = ref world.GetComponent<FollowEntityComponent>(entity);
                follow.target = target;
                follow.offset = offset;
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}