using Gemserk.Leopotam.Ecs;
using UnityEngine;

namespace Game.Components
{
    public struct DashComponent : IEntityComponent
    {
        public float speed;
        public float duration;

        public Vector3 offset;

        public bool interruptWithJump;
    }
    
    public class DashComponentDefinition : ComponentDefinitionBase
    {
        public float speed;
        public float duration;
        public Vector3 offset;

        public bool interruptWithJump = true;
        
        public override string GetComponentName()
        {
            return nameof(DashComponent);
        }

        public override void Apply(World world, Entity entity)
        {
            world.AddComponent(entity, new DashComponent
            {
                speed = speed,
                duration = duration,
                offset = offset,
                interruptWithJump = interruptWithJump
            });
        }
    }
}