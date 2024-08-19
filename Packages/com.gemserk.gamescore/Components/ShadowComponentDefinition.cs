using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct ShadowComponent : IEntityComponent
    {
        public LayerMask layerMask;
        public Entity target;
        public float distanceToGround;
    }
    
    public class ShadowComponentDefinition : ComponentDefinitionBase
    {
        public LayerMask projectionObstaclesLayerMask;

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ShadowComponent()
            {   
                layerMask = projectionObstaclesLayerMask
            });
        }
    }
}