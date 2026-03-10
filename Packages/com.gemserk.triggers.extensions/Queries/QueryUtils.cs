using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Gemserk.Leopotam.Ecs;
using Leopotam.EcsLite;

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
            GetEntities(world, query, results, world.GetFilter<QueryableComponent>().End());
        }
        
        public static void GetEntities(this World world, EntityQuery query, List<Entity> results, EcsFilter filter)
        {
            foreach (var entity in filter)
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

        public static EcsWorld.Mask GetDefaultFilterMask(this World world)
        {
            return world.GetFilter<QueryableComponent>();
        }
        
        public static Entity GetFirstOrDefault(this World world, EntityQuery query)
        {
            return GetFirstOrDefault(world, query, world.GetFilter<QueryableComponent>().End());
        }
        
        public static Entity GetFirstOrDefault(this World world, EntityQuery query, EcsFilter filter)
        {
            foreach (var entity in filter)
            {
                if (!query.MatchQuery(world, world.GetEntity(entity)))
                {
                    continue;
                }

                return world.GetEntity(entity);
            }

            return Entity.NullEntity;
        }
    }
}