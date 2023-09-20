using System.Collections.Generic;
using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace Game.Triggers
{
    public class EnableDisableSpawnerAction : WorldTriggerAction
    {
        public enum ActionType
        {
            Disable = 0,
            Enable = 1
        }
        
        public ActionType actionType;
        public TriggerTarget triggerTarget = new TriggerTarget();

        public override string GetObjectName()
        {
            return $"{actionType}Spawners({triggerTarget})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = new List<Entity>();
            world.GetTriggerTargetEntities(null, triggerTarget, activator, entities);
            
            foreach (var entity in entities)
            {
                ref var spawnerComponent = ref world.GetComponent<SpawnerComponent>(entity);
                spawnerComponent.paused = actionType == ActionType.Disable;
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}