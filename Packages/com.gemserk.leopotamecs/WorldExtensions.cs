using System.Collections.Generic;
using Gemserk.Utilities;

namespace Gemserk.Leopotam.Ecs
{
    public static class WorldExtensions
    {
        public static Entity CreateEntity(this World world, UnityEngine.Object definition)
        {
            return world.CreateEntity(definition.GetInterface<IEntityDefinition>());
        }
        
        public static Entity CreateEntity(this World world, UnityEngine.Object definition, 
            IEnumerable<IEntityInstanceParameter> parameters)
        {
            return world.CreateEntity(definition.GetInterface<IEntityDefinition>(),  parameters);
        }
    }
}