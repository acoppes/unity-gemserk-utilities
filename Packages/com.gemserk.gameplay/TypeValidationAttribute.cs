using System;
using UnityEngine;

namespace Gemserk.Gameplay
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
}