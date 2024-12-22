using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;
using Gemserk.Triggers.Queries;

namespace Game.Triggers
{
    public class LevelUpTriggerAction : WorldTriggerAction
    {
        public TriggerTarget target;
        public int levels;
        
        public override string GetObjectName()
        {
            return $"LevelUp({levels}, {target})";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(target, activator);

            foreach (var entity in entities)
            {
                entity.Get<LevelComponent>().QueueLevelUp(levels);
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}