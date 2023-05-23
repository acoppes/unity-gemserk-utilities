using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public struct EnabledParameter : IQueryParameter
    {
        public enum EnabledType
        {
            IsEnabled = 0,
            IsDisabled = 1
        }
        
        public EnabledType enabledType;

        public EnabledParameter(EnabledType enabledType)
        {
            this.enabledType = enabledType;
        }
        
        public bool MatchQuery(Entity entity)
        {
            var disabled = entity.Has<DisabledComponent>();

            if (enabledType == EnabledType.IsDisabled)
            {
                return disabled;
            }

            if (enabledType == EnabledType.IsEnabled)
            {
                return !disabled;
            }

            return false;
        }
    }
    
    public class EnabledQueryParameter : QueryParameterBase
    {
        public EnabledParameter.EnabledType enabledType;
        
        public override bool MatchQuery(Entity entity)
        {
            return new EnabledParameter(enabledType).MatchQuery(entity);
        }

        public override string ToString()
        {
            return $"{enabledType}";
        }
    }
}