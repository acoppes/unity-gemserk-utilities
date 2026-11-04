using System;
using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public interface IEntityMatcher
    {
        bool Match(Entity entity);
    }
    
    public class EntityMatcherCallback : IEntityMatcher
    {
        private readonly Func<Entity, bool> callback;

        public EntityMatcherCallback(Func<Entity, bool> callback)
        {
            this.callback = callback;
        }

        public bool Match(Entity entity)
        {
            return callback(entity);
        }
    }
}