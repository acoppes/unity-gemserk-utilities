using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public static class QueryUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MatchQuery(this Query query, World world, Entity entity)
        {
            return query.GetEntityQuery().MatchQuery(world, entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MatchQuery(this EntityQuery query, Entity entity)
        {
            for (var i = 0; i < query.parameters.Count; i++)
            {
                if (!query.parameters[i].MatchQuery(entity))
                {
                    return false;
                }
            }
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MatchQuery(this EntityQuery query, World world, int entity)
        {
            return query.MatchQuery(world.GetEntity(entity));
        }

        public static List<Entity> GetEntities(this World world, Query query)
        {
            return world.GetEntities(query.GetEntityQuery());
        }
        
        public static List<Entity> GetEntities(this World world, EntityQuery query)
        {
            var results = new List<Entity>();
            world.GetEntities(query, results);
            return results;
        }
        
        public static List<Entity> GetEntities(this World world, TriggerTarget target, object activator)
        {
            var results = new List<Entity>();
            target.Get(results, world, activator);
            return results;
        }

        public static void GetEntities(this World world, EntityQuery query, List<Entity> results)
        {
            foreach (var entity in world.GetFilter<QueryableComponent>().End())
            {
                if (!query.MatchQuery(world, world.GetEntity(entity)))
                {
                    continue;
                }
                
                results.Add(world.GetEntity(entity));
            }
        }
        
        public static Entity GetFirstOrDefault(this World world, Query query)
        {
            return world.GetFirstOrDefault(query.GetEntityQuery());
        }
        
        public static Entity GetFirstOrDefault(this World world, EntityQuery query)
        {
            foreach (var entity in world.GetFilter<QueryableComponent>().End())
            {
                if (!query.MatchQuery(world, world.GetEntity(entity)))
                {
                    continue;
                }

                return world.GetEntity(entity);
            }

            return Entity.NullEntity;
            
            // throw new Exception($"Failed to get singleton entity from query {query}");
        }
    }
}