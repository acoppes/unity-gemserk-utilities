using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct ModelInterpolationComponent : IEntityComponent
    {
        public Vector3 previousPosition;
        public Vector3 currentPosition;

        public float time;
        public float t;

        public bool disabled;

        public Vector3 position => Vector3.Lerp(previousPosition, currentPosition, t);
    }
    
    public class ModelInterpolationComponentDefinition : ComponentDefinitionBase
    {
        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new ModelInterpolationComponent());
        }
    }
}