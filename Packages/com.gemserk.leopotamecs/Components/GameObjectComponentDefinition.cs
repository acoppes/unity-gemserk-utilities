using Gemserk.Leopotam.Ecs.Controllers;
using MyBox;
using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Components
{
    public struct GameObjectComponent : IEntityComponent
    {
        public GameObject gameObject;
        public GameObject prefab;
        
        public bool reusablePrefab;
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

        [ConditionalField(nameof(prefab))]
        public bool reusable = true;

        public CopyPositionType copyPositionType = CopyPositionType.CopyFromEntity;

        public bool isController;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new GameObjectComponent
            {
                gameObject = linkedObjectInstance,
                prefab = prefab,
                reusablePrefab = reusable
            });

            if (copyPositionType == CopyPositionType.CopyFromEntity)
            {
                world.AddComponent<CopyPositionFromEntityComponent>(entity);
            } else if (copyPositionType == CopyPositionType.CopyFromGameObject)
            {
                world.AddComponent<CopyPositionFromGameObjectComponent>(entity);
            }

            if (isController)
            {
                entity.Add(new ControllerFromGameObject());
            }
        }
    }
}