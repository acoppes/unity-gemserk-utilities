using System;
using System.Collections.Generic;
using Gemserk.Utilities;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    public static class WorldExtensions
    {
        public static Entity CreateEntity(this World world, UnityEngine.Object definition)
        {
#if UNITY_EDITOR
            if (!definition)
            {
                Debug.LogError("Trying to create Entity with null or missing definition object");
            }
#endif
            return world.CreateEntity(definition.GetInterface<IEntityDefinition>());
        }
        
        public static Entity CreateEntity(this World world, UnityEngine.Object definition, 
            IEnumerable<IEntityInstanceParameter> parameters, Action<Entity> configuration = null)
        {
#if UNITY_EDITOR
            if (!definition)
            {
                Debug.LogError("Trying to create Entity with null or missing definition object");
            }
#endif
            return world.CreateEntity(definition.GetInterface<IEntityDefinition>(), parameters, configuration);
        }
        
    }
}