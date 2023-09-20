using Game.Components;
using Game.Utilities;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;

namespace Game.Queries
{
    public struct HealthParameter : IQueryParameter
    {
        public HealthComponent.AliveType aliveType; 

        public HealthParameter(HealthComponent.AliveType aliveType)
        {
            this.aliveType = aliveType;
        }
        
        public bool MatchQuery(Entity entity)
        {
            if (!entity.Has<HealthComponent>())
                return false;

            var hitPointsComponent = entity.Get<HealthComponent>();

            return hitPointsComponent.aliveType.HasAliveFlag(aliveType);
        }
    }
    
    public class HealthQueryParameter : QueryParameterBase
    {
        public HealthComponent.AliveType aliveType; 
        
        public override bool MatchQuery(Entity entity)
        {
            return new HealthParameter(aliveType).MatchQuery(entity);
        }

        public override string ToString()
        {
            return $"health:{aliveType}";
        }
    }
}