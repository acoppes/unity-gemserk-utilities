using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public interface IQueryParameter
    {
        bool MatchQuery(World world, Entity entity);
    }
}