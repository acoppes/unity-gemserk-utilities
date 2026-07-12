using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Gemserk.Triggers.Queries
{
    public abstract class EntityMatcherBase : MonoBehaviour, IEntityMatcher
    {
        public abstract bool Match(Entity entity);
    }
}