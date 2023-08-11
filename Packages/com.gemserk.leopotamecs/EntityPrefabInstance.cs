using Gemserk.Utilities;
using UnityEngine.Serialization;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstance : BaseEntityPrefabInstance
    {
        [FormerlySerializedAs("entityDefinitionPrefab")] 
        public UnityEngine.Object entityDefinition;


        public override IEntityDefinition GetEntityDefinition()
        {
            return entityDefinition.GetInterface<IEntityDefinition>();
        }
    }
}