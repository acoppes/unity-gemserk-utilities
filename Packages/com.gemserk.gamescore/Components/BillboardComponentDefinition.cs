using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct BillboardComponent : IEntityComponent
    {
        public Vector3 cameraDirection;
        public Vector3 lookAtPosition;
        public float cameraAngle;
    }
    
    public class BillboardComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new BillboardComponent());
        }
    }
}