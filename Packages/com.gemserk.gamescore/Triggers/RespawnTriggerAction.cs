using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;

namespace Game.Triggers
{
    public class RespawnTriggerAction : WorldTriggerAction
    {
        public TriggerTarget triggerTarget;

        public override string GetObjectName()
        {
            return $"Respawn({triggerTarget})"; 
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(triggerTarget, activator);
            
            foreach (var entity in entities)
            {
                entity.Add(new RespawnActionComponent());
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}