using System;
using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs
{
    public static class SharedDataWorldExtensions
    {
        public static Entity GetEntityByName(this World world, string entityName)
        {
            if (world.sharedData.singletonByNameEntities.ContainsKey(entityName))
            {
                return world.sharedData.singletonByNameEntities[entityName];
            }
            return Entity.NullEntity;
        }
    }
    
    public class WorldSharedData
    {
        public object sharedData;
        
        public readonly IDictionary<string, Entity> singletonByNameEntities = 
            new Dictionary<string, Entity>(StringComparer.InvariantCultureIgnoreCase);
        
    }
}