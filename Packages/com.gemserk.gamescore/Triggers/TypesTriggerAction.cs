using System.Collections.Generic;
using Game.Components;
using Game.Components.Abilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace Game.Triggers
{
    public class TypesTriggerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            AddType = 0,
            RemoveType = 1
        }
        
        public ActionType actionType = ActionType.AddType;
        
        public TriggerTarget target;
        public string type;

        public override string GetObjectName()
        {
            return $"{actionType}({type}, {target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            
            target.Get(entities, world, activator);
            
            foreach (var e in entities)
            {
                ref var types = ref e.Get<TypesComponent>();
                if (actionType == ActionType.AddType)
                {
                    types.types.Add(type);
                } else if (actionType == ActionType.RemoveType)
                {
                    types.types.Remove(type);
                }
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}