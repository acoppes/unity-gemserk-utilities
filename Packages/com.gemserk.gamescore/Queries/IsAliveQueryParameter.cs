using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public struct IsAliveMatcher : IEntityMatcher
    {
        public HealthComponent.AliveType aliveType; 

        public IsAliveMatcher(HealthComponent.AliveType aliveType)
        {
            this.aliveType = aliveType;
        }
        
        public bool Match(Entity entity)
        {
            if (!entity.Has<HealthComponent>())
                return false;

            var hitPointsComponent = entity.Get<HealthComponent>();

            return hitPointsComponent.aliveType.HasAliveFlag(aliveType);
        }
    }
    
    public class IsAliveQueryParameter : EntityMatcherBase
    {
        public HealthComponent.AliveType aliveType; 
        
        public override bool Match(Entity entity)
        {
            return new IsAliveMatcher(aliveType).Match(entity);
        }

        public override string ToString()
        {
            return $"health:{aliveType}";
        }
    }
}