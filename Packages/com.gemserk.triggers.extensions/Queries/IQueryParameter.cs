using Gemserk.Leopotam.Ecs;

namespace Beatemup.Queries
{
    public interface IQueryParameter
    {
        bool MatchQuery(World world, Entity entity);
    }
}