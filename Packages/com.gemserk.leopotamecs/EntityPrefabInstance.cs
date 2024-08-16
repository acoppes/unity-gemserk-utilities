using Gemserk.Utilities;

namespace Gemserk.Leopotam.Ecs
{
    public class EntityPrefabInstance : BaseEntityPrefabInstance
    {
        [EntityDefinition]
        public UnityEngine.Object entityDefinition;
        
        public override IEntityDefinition GetEntityDefinition()
        {
            return entityDefinition.GetInterface<IEntityDefinition>();
        }
    }
}