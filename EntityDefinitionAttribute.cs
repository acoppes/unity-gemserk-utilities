using System;
using Gemserk.Gameplay;

namespace Gemserk.Leopotam.Ecs
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class EntityDefinitionAttribute : TypeValidationAttribute
    {
        public EntityDefinitionAttribute() : base(typeof(IEntityDefinition))
        {
            
        }
    }
}
