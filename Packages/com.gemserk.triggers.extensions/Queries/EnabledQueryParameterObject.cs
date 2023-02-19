using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public class EnabledQueryParameterObject : QueryParameterBase
    {
        public enum EnabledType
        {
            IsEnabled = 0,
            IsDisabled = 1
        }

        public EnabledType enabledType;
        
        public override bool MatchQuery(World world, Entity entity)
        {
            var disabled = world.HasComponent<DisabledComponent>(entity);

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

        public override string ToString()
        {
            return $"{enabledType}";
        }
    }
}