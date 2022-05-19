using System;
using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs
{
    public class WorldSharedData
    {
        public object sharedData;
        
        public readonly IDictionary<string, int> singletonEntities = 
            new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        
    }
}