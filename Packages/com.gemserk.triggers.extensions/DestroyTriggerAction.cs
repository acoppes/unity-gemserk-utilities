using Gemserk.Leopotam.Ecs.Components;
using Gemserk.Triggers.Queries;
using MyBox;

namespace Gemserk.Triggers
{
    public class DestroyTriggerAction : WorldTriggerAction
    {
        [DisplayInspector]
        public Query query;

        public override string GetObjectName()
        {
            if (query != null)
            {
                return $"Destroy({query})";
            }

            return "Destroy()";
        }

        public override ITrigger.ExecutionResult Execute(object activator = null)
        {
            var entities = world.GetEntities(query.GetEntityQuery());
            
            foreach (var entity in entities)
            {
                ref var destroyableComponent = ref world.GetComponent<DestroyableComponent>(entity);
                destroyableComponent.destroy = true;
            }
            
            return ITrigger.ExecutionResult.Completed;
        }
    }
}