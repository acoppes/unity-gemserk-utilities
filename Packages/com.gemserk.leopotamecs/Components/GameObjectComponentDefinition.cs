using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Components
{
    public struct GameObjectComponent : IEntityComponent
    {
        public GameObject gameObject;
        public GameObject prefab;
    }

    public struct CopyPositionFromEntityComponent : IEntityComponent
    {
        
    }
    
    public struct CopyPositionFromGameObjectComponent : IEntityComponent
    {
        
    }
    
    public class GameObjectComponentDefinition : ComponentDefinitionBase
    {
        public enum CopyPositionType
        {
            CopyFromEntity = 0,
            CopyFromGameObject = 1, 
            None
        }
        
        public GameObject linkedObjectInstance;
        public GameObject prefab;

        public CopyPositionType copyPositionType = CopyPositionType.CopyFromEntity;
        
        public override string GetComponentName()
        {
            return nameof(GameObjectComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new GameObjectComponent
            {
                gameObject = linkedObjectInstance,
                prefab = prefab
            });

            if (copyPositionType == CopyPositionType.CopyFromEntity)
            {
                world.AddComponent<CopyPositionFromEntityComponent>(entity);
            } else if (copyPositionType == CopyPositionType.CopyFromGameObject)
            {
                world.AddComponent<CopyPositionFromGameObjectComponent>(entity);
            }
        }
    }
}