using Gemserk.Utilities;

namespace Gemserk.Leopotam.Ecs
{
    public interface IEntityDefinition
    {
        void Apply(World world, Entity entity);
    }

    public class EntityDefinitionAttribute : ObjectTypeAttribute
    {
        public EntityDefinitionAttribute() : base(typeof(IEntityDefinition))
        {
            filterString = "_Definition";
            disableAssetReferences = true;
            sceneReferencesOnWhenStart = true;
            prefabReferencesOnWhenStart = true;
        }
    }
}