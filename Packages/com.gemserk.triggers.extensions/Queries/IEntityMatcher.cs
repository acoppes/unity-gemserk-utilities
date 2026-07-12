using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public interface IEntityMatcher
    {
        bool Match(Entity entity);
    }
}