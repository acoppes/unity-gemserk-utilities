using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gemserk.Leopotam.Ecs;

namespace Gemserk.Triggers.Queries
{
    public static class QueryUtils
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool MatchQuery(this EntityQuery query, World world, Entity entity)
        {
            foreach (var parameter in query.parameters)
            {
                if (!parameter.MatchQuery(world, world.GetEntity(entity)))
                {
                    return false;
                }
            }
            return true;
        }

        public static List<Entity> GetEntities(this World world, Query query)
        {
            return GetEntities(world, query.GetEntityQuery());
        }
        
        public static List<Entity> GetEntities(this World world, EntityQuery query)
        {
            var results = new List<Entity>();
            GetEntities(world, query, results);
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
        
        public static Entity GetSingletonEntity(this World world, Query query)
        {
            return GetSingletonEntity(world, query.GetEntityQuery());
        }
        
        public static Entity GetSingletonEntity(this World world, EntityQuery query)
        {
            foreach (var entity in world.GetFilter<QueryableComponent>().End())
            {
                if (!query.MatchQuery(world, world.GetEntity(entity)))
                {
                    continue;
                }

                return world.GetEntity(entity);
            }

            throw new Exception($"Failed to get singleton entity from query {query}");
        }
    }
}