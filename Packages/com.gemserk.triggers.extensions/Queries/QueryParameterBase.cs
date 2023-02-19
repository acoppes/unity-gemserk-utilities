using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Gemserk.Triggers.Queries
{
    public abstract class QueryParameterBase : MonoBehaviour, IQueryParameter
    {
        public abstract bool MatchQuery(World world, Entity entity);
    }
}