using System;
using System.Collections.Generic;
using Gemserk.Leopotam.Ecs;
using Gemserk.Triggers.Queries;
using Gemserk.Utilities;
using MyBox;

namespace Gemserk.Triggers
{
    public static class TriggerTargetExtensions
    {
        public static void GetTriggerTargetEntities(this World world, Query query, TriggerTarget triggerTarget, object activator, List<Entity> entities)
        {
            if (query)
            {
                world.GetEntities(query.GetEntityQuery(), entities);
            }
            else
            {
                triggerTarget.Get(entities, world, activator);
            }
        }
        
        public static Entity GetTriggerFirstEntity(this World world, Query query, TriggerTarget triggerTarget, object activator)
        {
            if (query)
            {
                return world.GetFirstOrDefault(query.GetEntityQuery());
            }
            
            return triggerTarget.Get(world, activator);
        }
    }
    
    [Serializable]
    public class TriggerTarget
    {
        public enum QuerySourceType
        {
            Identifier = 0,
            TriggerActivator = 1,
            Query = 2
        }
        
        public QuerySourceType sourceType = QuerySourceType.Identifier;

        [ConditionalField(nameof(sourceType), false, QuerySourceType.Query)]
        [ObjectType(typeof(Query), disableAssetReferences = true, prefabReferencesOnWhenStart = false)]
        public Query query;

        [ConditionalField(nameof(sourceType), false, QuerySourceType.Identifier)]
        public string identifier;

        public bool Get(List<Entity> result, World world, object activator = null)
        {
            if (sourceType == QuerySourceType.TriggerActivator)
            {
                if (activator == null)
                    return false;
                result.Add((Entity)activator);
                return true;
            }

            if (sourceType == QuerySourceType.Identifier)
            {
                var queryEntities = world.GetEntities(new EntityQuery(new NameParameter(identifier)));
                if (queryEntities.Count == 0)
                    return false;
                result.AddRange(queryEntities);
                return true;
            }

            if (sourceType == QuerySourceType.Query)
            {
                var queryEntities = world.GetEntities(query.GetEntityQuery());
                if (queryEntities.Count == 0)
                    return false;
                result.AddRange(queryEntities);
                return true;
            }

            return false;
        }
        
        public Entity Get(World world, object activator = null)
        {
            if (sourceType == QuerySourceType.TriggerActivator)
            {
                if (activator != null)
                {
                    return (Entity) activator;
                }
            }

            if (sourceType == QuerySourceType.Identifier)
            {
                return world.GetFirstOrDefault(new EntityQuery(new NameParameter(identifier)));
            }

            if (sourceType == QuerySourceType.Query)
            {
                return world.GetFirstOrDefault(query.GetEntityQuery());
            }

            return Entity.NullEntity;
        }

        public override string ToString()
        {
            if (sourceType == QuerySourceType.Identifier)
            {
                return $"[{sourceType}:{identifier}]";
            }
            
            if (sourceType == QuerySourceType.Query)
            {
                return query ? $"[{sourceType}:{query.name}]" : $"[{sourceType}]";
            }
            
            return $"[{sourceType}]";
        }
    }
}