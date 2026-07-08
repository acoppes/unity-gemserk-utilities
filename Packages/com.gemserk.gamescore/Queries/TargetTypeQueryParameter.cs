using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public readonly struct TargetTypeParameter : IEntityMatcher
    {
        private readonly TargetType targetType;

        public TargetTypeParameter(TargetType targetType)
        {
            this.targetType = targetType;
        }
        
        public bool Match(Entity entity)
        {
            if (!entity.Has<TargetComponent>())
            {
                return false;
            }

            var entityTargetType = entity.Get<TargetComponent>().target.targetType;
            return targetType.HasTargetFlag(entityTargetType);
        }
    }
    
    public class TargetTypeQueryParameter : EntityMatcherBase
    {
        public TargetType targetType;
        
        public override bool Match(Entity entity)
        {
            return new TargetTypeParameter(targetType).Match(entity);
        }

        public override string ToString()
        {
            return $"targetType:{targetType}";
        }
    }
}