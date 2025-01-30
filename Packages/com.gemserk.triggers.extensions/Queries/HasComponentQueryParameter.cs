using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public struct HasComponent<T> : IQueryParameter where T : struct, IEntityComponent
    {
        public bool MatchQuery(Entity entity)
        {
            return entity.Has<T>();
        }
    }
    
    public abstract class HasComponentQueryParameter<T> : QueryParameterBase where T : struct, IEntityComponent
    {
        public override bool MatchQuery(Entity entity)
        {
            return entity.Has<T>();
        }

        public override string ToString()
        {
            return $"Has({typeof(T).Name})";
        }
    }
}