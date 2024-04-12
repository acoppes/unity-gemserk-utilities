using System;
using UnityEngine;

namespace Gemserk.Utilities
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ObjectTypeAttribute : PropertyAttribute
    {
        public Type typeToSelect;

        public bool disableSceneReferences;
        public bool disablePrefabReferences;
        public bool disableAssetReferences;

        public ObjectTypeAttribute(Type typeToSelect)
        {
            this.typeToSelect = typeToSelect;
        }
    }
}