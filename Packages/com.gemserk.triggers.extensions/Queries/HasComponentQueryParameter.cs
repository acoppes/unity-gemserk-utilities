using Gemserk.Leopotam.Ecs;
using UnityEngine.Assertions;

namespace Gemserk.Triggers.Queries
{
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