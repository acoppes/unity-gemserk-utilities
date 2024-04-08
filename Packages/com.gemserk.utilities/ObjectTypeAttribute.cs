using System;
using UnityEngine;

namespace Gemserk.Utilities
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ObjectTypeAttribute : PropertyAttribute
    {
        public Type typeToSelect;

        public ObjectTypeAttribute(Type typeToSelect)
        {
            this.typeToSelect = typeToSelect;
        }
    }
}