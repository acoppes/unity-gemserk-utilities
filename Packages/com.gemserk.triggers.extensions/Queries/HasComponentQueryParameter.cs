using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public struct HasComponent<T> : IEntityMatcher where T : struct, IEntityComponent
    {
        public bool Match(Entity entity)
        {
            return entity.Has<T>();
        }
    }
    
    public abstract class HasComponentQueryParameter<T> : EntityMatcherBase where T : struct, IEntityComponent
    {
        public override bool Match(Entity entity)
        {
            return entity.Has<T>();
        }

        public override string ToString()
        {
            return $"Has({typeof(T).Name})";
        }
    }
}