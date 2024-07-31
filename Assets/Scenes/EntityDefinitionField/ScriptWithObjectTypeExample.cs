using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Scenes.EntityDefinitionField
{
    public class ScriptWithObjectTypeExample : MonoBehaviour
    {
        public const string SuperFolder = "Assets/Scenes/Definitions/SuperWeapons;Assets/Scenes/Definitions/SuperArmors";
        // public const string[] SuperFolderArray = ["Super"];
        
        [ObjectType(typeof(IEntityDefinition))]
        public Object referenceExample;
        
        [ObjectType(typeof(IEntityDefinition), disableAssetReferences = true, disablePrefabReferences = true)]
        public Object onlyScene;

        [ObjectType(typeof(IEntityDefinition), disableSceneReferences = true, disableAssetReferences = true)]
        public Object onlyPrefabs;

        [ObjectType(typeof(IEntityDefinition), disablePrefabReferences = true, disableSceneReferences = true)]
        public Object onlyAssets;

        [ObjectType(typeof(IEntityDefinition), filterString = "Weapon")]
        public Object weaponDefinitions;
        
        [ObjectType(typeof(IEntityDefinition), folders = SuperFolder, 
            disableSceneReferences = true, disableAssetReferences = true, disablePrefabReferences = false)]
        public Object onlyInFolder;
        
        [InterfaceReferenceType]
        public InterfaceReference<IEntityDefinition> reference;
    
        [InterfaceReferenceType(sceneReferencesFilter = FindObjectsInactive.Exclude)]
        public InterfaceReference<IEntityDefinition> referenceExcludeDisabled;
    }
}