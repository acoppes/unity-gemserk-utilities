using Game.Components;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers;

namespace Game.Triggers.Conditions
{
    public class IsControlledByTriggerCondition : WorldTriggerCondition
    {
        public TriggerTarget target;
        public ControllableByComponent.ControllableType controllableType;
        
        public override string GetObjectName()
        {
            return $"IsControlledBy({target}, {controllableType})";
        }

        public override bool Evaluate(object activator = null)
        {
            var targetEntity = target.Get(world, activator);

            if (targetEntity.Has<ControllableByComponent>())
            {
                return targetEntity.Get<ControllableByComponent>().controllableType.HasControllableType(controllableType);
            }

            return false;
        }
    }
}