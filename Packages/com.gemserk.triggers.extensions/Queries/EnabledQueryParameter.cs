using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public struct IsEnabledMatcher : IEntityMatcher
    {
        public enum EnabledType
        {
            IsEnabled = 0,
            IsDisabled = 1
        }
        
        public EnabledType enabledType;

        public IsEnabledMatcher(EnabledType enabledType)
        {
            this.enabledType = enabledType;
        }
        
        public bool Match(Entity entity)
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
    
    public class EnabledQueryParameter : EntityMatcherBase
    {
        public IsEnabledMatcher.EnabledType enabledType;
        
        public override bool Match(Entity entity)
        {
            return new IsEnabledMatcher(enabledType).Match(entity);
        }

        public override string ToString()
        {
            return $"{enabledType}";
        }
    }
}