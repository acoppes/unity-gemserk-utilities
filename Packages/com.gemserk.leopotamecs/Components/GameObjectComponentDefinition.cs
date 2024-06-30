using UnityEngine;

namespace Gemserk.Leopotam.Ecs.Components
{
    public struct GameObjectComponent : IEntityComponent
    {
        public GameObject gameObject;
        public GameObject prefab;
    }
    
    public class GameObjectComponentDefinition : ComponentDefinitionBase
    {
        public GameObject linkedObjectInstance;
        public GameObject prefab;
        
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
        }
    }
}