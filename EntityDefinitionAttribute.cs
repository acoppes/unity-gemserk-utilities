using System;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class TypeValidationAttribute : PropertyAttribute
    {
        public Type typeToValidate;

        public TypeValidationAttribute(Type typeToValidate)
        {
            this.typeToValidate = typeToValidate;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Class)]
    public class EntityDefinitionAttribute : TypeValidationAttribute
    {
        public EntityDefinitionAttribute() : base(typeof(IEntityDefinition))
        {
            
        }
    }
}
