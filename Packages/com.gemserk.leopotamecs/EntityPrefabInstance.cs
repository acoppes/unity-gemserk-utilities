using Gemserk.Utilities;
using UnityEngine.Serialization;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstance : BaseEntityPrefabInstance
    {
        [FormerlySerializedAs("entityDefinitionPrefab")] 
        [ObjectType(typeof(IEntityDefinition), filterString = "Definition", assetReferencesOnWhenStart = false, prefabReferencesOnWhenStart = true)]
        public UnityEngine.Object entityDefinition;
        
        public override IEntityDefinition GetEntityDefinition()
        {
            return entityDefinition.GetInterface<IEntityDefinition>();
        }
    }
}