using System;
using UnityEngine;

namespace Gemserk.Utilities
{
    [AttributeUsage(AttributeTargets.Field)]
    public abstract class BaseObjectTypeAttribute : PropertyAttribute
    {
        /// <summary>
        /// This is used to prefilter objects/prefabs/assets having that string in the name.
        /// </summary>
        public string filterString = null;

        public bool disableSceneReferences;
        public bool disablePrefabReferences;
        public bool disableAssetReferences;
        
        public bool sceneReferencesOpen => !disableSceneReferences;
        public bool prefabReferencesOpen => !disablePrefabReferences;
        public bool assetReferencesOpen => !disableAssetReferences;
        
        public FindObjectsInactive sceneReferencesFilter = FindObjectsInactive.Include;

        public string folders = "Assets";

        public string[] GetFolders()
        {
            return folders.Split(";");
        }

        public abstract Type GetPropertyType();
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class ObjectTypeAttribute : BaseObjectTypeAttribute
    {
        public readonly Type typeToSelect;

        public ObjectTypeAttribute(Type typeToSelect)
        {
            this.typeToSelect = typeToSelect;
        }

        public override Type GetPropertyType()
        {
            return typeToSelect;
        }
    }
    
    [AttributeUsage(AttributeTargets.Field)]
    public class InterfaceReferenceTypeAttribute : BaseObjectTypeAttribute
    {
        public override Type GetPropertyType()
        {
            return null;
        }
    }
}