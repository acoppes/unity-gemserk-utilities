using Gemserk.Leopotam.Ecs;

namespace Game.Queries
{
    public class QueryCacheSystem : BaseSystem
    {
        // I could create entities with cached queries, mainly for initial filtering, and use 
        // those cached queries as parameters for the EntityQuery, we could even use the same API.
    }
}