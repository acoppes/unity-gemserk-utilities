using Gemserk.Leopotam.Ecs;
using Gemserk.Utilities;
using UnityEngine;

namespace Scenes.EntityDefinitionField
{
    public class ScriptWithObjectTypeExample : MonoBehaviour
    {
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
    }
}