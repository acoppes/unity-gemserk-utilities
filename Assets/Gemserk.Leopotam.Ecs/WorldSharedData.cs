using System;
using System.Collections.Generic;

namespace Gemserk.Leopotam.Ecs
{
    public class WorldSharedData
    {
        public object sharedData;
        
        public readonly IDictionary<string, int> singletonByNameEntities = 
            new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
        
    }
}