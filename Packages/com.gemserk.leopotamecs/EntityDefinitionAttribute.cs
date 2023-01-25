using System;
using Gemserk.Utilities;

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
