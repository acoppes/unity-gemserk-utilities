using System;
using UnityEngine;

namespace Gemserk.Utilities
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ObjectTypeAttribute : PropertyAttribute
    {
        public Type typeToSelect;

        /// <summary>
        /// This is used to prefilter objects/prefabs/assets having that string in the name.
        /// </summary>
        public string filterString = null;

        public bool disableSceneReferences;
        public bool disablePrefabReferences;
        public bool disableAssetReferences;

        public ObjectTypeAttribute(Type typeToSelect)
        {
            this.typeToSelect = typeToSelect;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class InterfaceReferenceTypeAttribute : PropertyAttribute
    {
        /// <summary>
        /// This is used to prefilter objects/prefabs/assets having that string in the name.
        /// </summary>
        public string filterString = null;

        public bool disableSceneReferences;
        public bool disablePrefabReferences;
        public bool disableAssetReferences;

        public InterfaceReferenceTypeAttribute()
        {
            
        }
    }
}