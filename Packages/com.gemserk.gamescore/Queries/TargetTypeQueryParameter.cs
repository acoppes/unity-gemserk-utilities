using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public readonly struct TargetTypeParameter : IQueryParameter
    {
        private readonly TargetType targetType;

        public TargetTypeParameter(TargetType targetType)
        {
            this.targetType = targetType;
        }
        
        public bool MatchQuery(Entity entity)
        {
            if (!entity.Has<TargetComponent>())
            {
                return false;
            }

            var entityTargetType = entity.Get<TargetComponent>().target.targetType;
            return targetType.HasTargetFlag(entityTargetType);
        }
    }
    
    public class TargetTypeQueryParameter : QueryParameterBase
    {
        public TargetType targetType;
        
        public override bool MatchQuery(Entity entity)
        {
            return new TargetTypeParameter(targetType).MatchQuery(entity);
        }

        public override string ToString()
        {
            return $"targetType:{targetType}";
        }
    }
}